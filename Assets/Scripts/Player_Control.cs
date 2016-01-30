using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Control : MonoBehaviour {

	[Header("Properties")]
	public float movementSpeed = 15f * 1.2f;
	public int playerNumber;
	public int numberOfPlayers = 2;

	// Window Keys
	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode leftKey;
	private KeyCode rightKey;


	private Animator animator;

	// Movement
	public Vector2 velocity;
	public bool isFrozen;
	private Rigidbody rigidBody;

	private AbilityController Abilities;

	void Start ()
	{
		// assign the character rigid body to this movement script
		rigidBody = GetComponent<Rigidbody> ();
	
		AssignInput();

		Abilities = gameObject.AddComponent<AbilityController>();

		isFrozen = false;
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
			break;
		case 2:
			upKey = KeyCode.UpArrow;
			downKey = KeyCode.DownArrow;
			leftKey = KeyCode.LeftArrow;
			rightKey = KeyCode.RightArrow;
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

		//velocity = velocity.normalized * movementSpeed;

		if (velocity.x == 0 && velocity.y == 0) {
			if (Input.GetKey (upKey)) {
				velocity.y = movementSpeed;
			}
			if (Input.GetKey (downKey)) {
				velocity.y = -1 * movementSpeed;
			}
			if (Input.GetKey (leftKey)) {
				velocity.x = -1 * movementSpeed;
			}
			if (Input.GetKey (rightKey)) {
				velocity.x = movementSpeed;
			}
		}
		
		if (animator != null) {
			animator.SetFloat ("velocity", Mathf.Sqrt(velocity.x*velocity.x + velocity.y * velocity.y));
		}

		if(velocity.x != 0 || velocity.y != 0)
			transform.rotation = Quaternion.Euler (new Vector3(90,0,Mathf.Rad2Deg * Mathf.Atan2 (velocity.y, velocity.x) + 90));

		velocity = velocity.normalized * movementSpeed;
	}

	private void HandleAbility()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			Abilities.Freeze.UseAbility(Abilities.PlayerTwo);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			Abilities.Dash.UseAbility();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			Abilities.Block.UseAbility();
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			Abilities.Bomb.UseAbility();
		}

		// LOLHAX
		if(Input.GetKey(KeyCode.P))
		{
			Abilities.Freeze.GrantAbility();
			Abilities.Dash.GrantAbility();
			Abilities.Block.GrantAbility();
			Abilities.Bomb.GrantAbility();
		}
	}

	public void Knockback(Vector3 position)
	{
		StartCoroutine(HandleKnockback(position));
	}

	public void Freeze(bool Flag)
	{
		isFrozen = Flag;
	}

	private IEnumerator HandleKnockback(Vector3 position)
	{
		// Freeze player so we can control movement
		isFrozen = true;

		// Grab the current position
		Vector3 CurPos;

		// Apply our move amount to our current to find our target
		Vector3 Target = transform.position + position;

		{
			// Lerp between current position and target 
			CurPos = Vector3.Lerp(transform.position, Target, movementSpeed * Time.deltaTime);
			transform.position = CurPos;
		}

		// Check if we have finished
		if(transform.position.Equals(position))
			break;

		// Allow player to move again
		isFrozen = false;

		yield return new WaitForEndOfFrame();
	}
}
