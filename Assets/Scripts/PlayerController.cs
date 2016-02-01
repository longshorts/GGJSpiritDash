using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	[Header("Properties")]
	public float movementSpeed = 15f * 1.2f;
	public int playerNumber;
	public int numberOfPlayers = 2;

	// Window Keys
	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode leftKey;
	private KeyCode rightKey;

	private KeyCode FreezeKey;
	private KeyCode BlockKey;
	private KeyCode DashKey;
	private KeyCode BombKey;

	private KeyCode FreezeButton;
	private KeyCode BlockButton;
	private KeyCode DashButton;
	private KeyCode BombButton;

	private Animator animator;

	// Movement
	public Vector2 velocity;
	public bool isFrozen;
	public bool ControlsLocked;
	private Rigidbody rigidBody;

	public Vector3 DirectionVector;
	public Vector3 LocalDirectionVector;

	private AbilityController Abilities;

	public AudioClip blockSound;
	public AudioClip bombSound;
	public AudioClip dashSound;
	public AudioClip freezeSound;
	private AudioSource audio;

	void Start ()
	{
		// assign the character rigid body to this movement script
		rigidBody = GetComponent<Rigidbody> ();
	
		AssignInput();

		Abilities = gameObject.GetComponent<AbilityController>();

		isFrozen = false;
		animator.SetBool ("isFrozen", isFrozen);
		
		audio = GetComponent<AudioSource> ();
		DirectionVector = new Vector3 (0, 0, -1);
		LocalDirectionVector = new Vector3(0, -1, 0);
	}

	private void AssignInput()
	{
		switch (playerNumber)
		{
		case 1:
			upKey = KeyCode.W;
			downKey = KeyCode.S;
			leftKey = KeyCode.A;
			rightKey = KeyCode.D;
			FreezeKey = KeyCode.Alpha1;
			DashKey = KeyCode.Alpha2;
			BlockKey = KeyCode.Alpha3;
			BombKey = KeyCode.Alpha4;
			FreezeButton = KeyCode.Joystick1Button0;
			DashButton = KeyCode.Joystick1Button1;
			BlockButton = KeyCode.Joystick1Button2;
			BombButton = KeyCode.Joystick1Button3;
			break;

		case 2:
			upKey = KeyCode.UpArrow;
			downKey = KeyCode.DownArrow;
			leftKey = KeyCode.LeftArrow;
			rightKey = KeyCode.RightArrow;
			FreezeKey = KeyCode.Alpha9;
			DashKey = KeyCode.Alpha0;
			BlockKey = KeyCode.Minus;
			BombKey = KeyCode.Equals;
			FreezeButton = KeyCode.Joystick2Button0;
			DashButton = KeyCode.Joystick2Button1;
			BlockButton = KeyCode.Joystick2Button2;
			BombButton = KeyCode.Joystick2Button3;
			break;
			
		default:
			Debug.LogError ("Unknown playerNumber, input not set");
			break;
		}
		
		this.animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		animator.SetBool ("isFrozen", isFrozen);

		// Controls disabled
		if(ControlsLocked)
			return;

		// If we are frozen we cant move
		if(isFrozen)
			return;

		// Handle movement
		HandleMovement();
		HandleAbility();

		// move the character
		rigidBody.velocity = new Vector3 (velocity.x, 0, velocity.y);
	}

	private void HandleMovement ()
	{
		// reset velocity
		velocity.x = 0;
		velocity.y = 0;

		float h = Input.GetAxis ("HorizontalLeftAnalogP" + playerNumber);
		float v = Input.GetAxis ("VerticalLeftAnalogP" + playerNumber);

		velocity.x = h;
		velocity.y = v * -1;

		if (velocity.x == 0 && velocity.y == 0)
		{
			if (Input.GetKey (upKey))
			{
				velocity.y = movementSpeed;
			}
			if (Input.GetKey (downKey))
			{
				velocity.y = -1 * movementSpeed;
			}
			if (Input.GetKey (leftKey))
			{
				velocity.x = -1 * movementSpeed;
			}
			if (Input.GetKey (rightKey))
			{
				velocity.x = movementSpeed;
			}
		}
		
		if (animator != null)
		{
			animator.SetFloat ("velocity", Mathf.Sqrt(velocity.x*velocity.x + velocity.y * velocity.y));
		}

		if(velocity.x != 0 || velocity.y != 0)
			transform.rotation = Quaternion.Euler (new Vector3(90,0,Mathf.Rad2Deg * Mathf.Atan2 (velocity.y, velocity.x) + 90));

		velocity = velocity.normalized * movementSpeed;

		CalculateDirectionVector ();
	}

	private void HandleAbility()
	{
		if(Input.GetKeyDown(FreezeKey) || Input.GetKeyDown(FreezeButton))
		{
			if (playerNumber == 1){
				Abilities.Freeze.UseAbility(Abilities.PlayerTwo);
				audio.PlayOneShot(freezeSound, 0.7f);
			} else if (playerNumber == 2){
				Abilities.Freeze.UseAbility(Abilities.PlayerOne);
				audio.PlayOneShot(freezeSound, 0.7f);
			}

		}
		if(Input.GetKeyDown(DashKey) || Input.GetKeyDown(DashButton))
		{
			Abilities.Dash.UseAbility();
			audio.PlayOneShot(dashSound, 0.7f);
		}
		if(Input.GetKeyDown(BlockKey) || Input.GetKeyDown(BlockButton))
		{
			Abilities.Block.UseAbility();
			audio.PlayOneShot(blockSound, 0.7f);
		}
		if(Input.GetKeyDown(BombKey) || Input.GetKeyDown(BombButton))
		{
			Abilities.Bomb.UseAbility();
			audio.PlayOneShot(bombSound, 0.7f);
		}
	}

	private void CalculateDirectionVector()
	{
		// Dont recalculate if we arent moving
		if (velocity.x + velocity.y == 0)
			return;

		// Create a target
		Vector3 Target = transform.position + new Vector3 (velocity.x, 0, velocity.y);

		DirectionVector = Target - transform.position;
		DirectionVector.Normalize ();

		// Reformat to Local
		LocalDirectionVector = new Vector3(DirectionVector.x, DirectionVector.z, 0.0f);

		Debug.Log ("Direction : " + DirectionVector);
	}

	public void Knockback(Vector3 position, float force)
	{
		// Calculate a direction between player and object
		Vector3 amount = (transform.position - position);
		amount.Normalize();

		// Apply force
		amount *= force;

		// Set new position
		// will trap player in the wall
		// feature?
		transform.position = transform.position + amount;
	}

	public void Freeze(bool Flag)
	{
		isFrozen = Flag;
		rigidBody.velocity = new Vector3 (0, 0, 0);
	}

	public void LockControls(bool Flag)
	{
		ControlsLocked = Flag;
	}
}