using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	[Header("Properties")]
	public float movementSpeed = 18.0f;
	public int playerNumber = 1;
	public List<Shrine> Objectives = new List<Shrine>();
	public bool isComplete;
	public bool isWinner;
	private float maxRespawnTime = 1f;
	private float respawnTimer = 0.0f;
	private bool isAlive = true;

	[Header("Windows Input")]
	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode leftKey;
	private KeyCode rightKey;
	private KeyCode freezeKey;
	private KeyCode blockKey;
	private KeyCode dashKey;
	private KeyCode bombKey;
	private KeyCode attackKey;
    private KeyCode dropBlockKey;

	[Header("Xbox Input")]
	private KeyCode freezeButton;
	private KeyCode blockButton;
	private KeyCode dashButton;
	private KeyCode bombButton;
	private KeyCode attackButton;
    private KeyCode dropBlockButton;
    private bool attack = false;

	// Movement
	[Header("Movement")]
	public Vector2 moveVelocity;
	public bool isFrozen;
	public bool controlsLocked;
	public Vector3 directionVector3D;
	public Vector3 directionVector2D;
	private Rigidbody rigidBody;

	// Components
	[Header("Components")]
	public GameController gameController;
	private AbilityController abilityController;
	private Animator animator;

	[Header("Audio")]
	public AudioClip blockSound;
	public AudioClip bombSound;
	public AudioClip dashSound;
	public AudioClip freezeSound;
	public AudioClip attackSound;
	private AudioSource audioSource;

	void Start ()
	{

        // Access components
		abilityController = GetComponent<AbilityController>();
		audioSource = GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();

        // Set input keys
        AssignInput();

		// Initialise animation
		isFrozen = false;
		animator.SetBool ("isFrozen", isFrozen);

		// Initialise direction vectors
		directionVector3D = new Vector3 (0, 0, -1);		// World Space
		directionVector2D = new Vector3(0, -1, 0);		// Local 2D

		// Initialise
		isComplete = false;
		isWinner = false;
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
				freezeKey = KeyCode.Alpha1;
				dashKey = KeyCode.Alpha2;
				blockKey = KeyCode.Alpha3;
				bombKey = KeyCode.Alpha4;
				attackKey = KeyCode.Alpha5;
                dropBlockButton = KeyCode.Alpha6;
				freezeButton = KeyCode.Joystick1Button2;    // X - Button
				dashButton = KeyCode.Joystick1Button0;      // A - Button
				blockButton = KeyCode.Joystick1Button3;     // Y - Button
				bombButton = KeyCode.Joystick1Button1;      // B - Button
				attackButton = KeyCode.Joystick1Button5;
                dropBlockButton = KeyCode.Joystick1Button4;
				break;

			case 2:
				upKey = KeyCode.UpArrow;
				downKey = KeyCode.DownArrow;
				leftKey = KeyCode.LeftArrow;
				rightKey = KeyCode.RightArrow;
				freezeKey = KeyCode.Alpha9;
				dashKey = KeyCode.Alpha0;
				attackKey = KeyCode.Alpha8;
                dropBlockKey = KeyCode.Alpha7;
				blockKey = KeyCode.Minus;
				bombKey = KeyCode.Equals;
				freezeButton = KeyCode.Joystick2Button2;    // X - Button
                dashButton = KeyCode.Joystick2Button0;      // A - Button
                blockButton = KeyCode.Joystick2Button3;     // Y - Button
                bombButton = KeyCode.Joystick2Button1;      // B - Button
                attackButton = KeyCode.Joystick2Button5;
                dropBlockButton = KeyCode.Joystick2Button4;
				break;
			
			default:
				Debug.LogError ("Unknown playerNumber, input not set");
				break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Controls disabled
		if(controlsLocked)
			return;

		// If we are frozen we cant move
		if(isFrozen)
			return;

		if (CheckRespawn())
			Respawn ();

		if (!isAlive)
			return;

		// Handle movement
		HandleMovement();
		HandleAbility();

		// move the character
		rigidBody.velocity = new Vector3 (moveVelocity.x, 0, moveVelocity.y);
	}

	private void HandleMovement ()
	{
		// Reset velocity
		moveVelocity.x = 0;
		moveVelocity.y = 0;

		// XBOX CONTROLLER
		float h = Input.GetAxis ("HorizontalLeftAnalogP" + playerNumber);
		float v = Input.GetAxis ("VerticalLeftAnalogP" + playerNumber);
        float a = Input.GetAxis("AttackP" + playerNumber);

        if (a > 0)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }

		moveVelocity.x = h;
		moveVelocity.y = v * -1;

		// WINDOWS
		if (moveVelocity.x == 0 && moveVelocity.y == 0)
		{
			if (Input.GetKey (upKey))
			{
				moveVelocity.y = 1;
			}
			if (Input.GetKey (downKey))
			{
				moveVelocity.y = -1;
			}
			if (Input.GetKey (leftKey))
			{
				moveVelocity.x = -1;
			}
			if (Input.GetKey (rightKey))
			{
				moveVelocity.x = 1;
			}
		}

		// If we have an animator
		if (animator)
		{
			// Animate walk sequence
			animator.SetFloat ("velocity", Mathf.Sqrt(moveVelocity.x*moveVelocity.x + moveVelocity.y * moveVelocity.y));
		}
		
		// Calculate rotation based on velocity -- THIS CAN BE REPLACED WITH DIRECTION VECTOR NOW!
		if(moveVelocity.x != 0 || moveVelocity.y != 0)
		{
			transform.rotation = Quaternion.Euler (new Vector3(90,0,Mathf.Rad2Deg * Mathf.Atan2 (moveVelocity.y, moveVelocity.x) + 90));
		}

		// Calculate final velocity
		moveVelocity = moveVelocity.normalized * movementSpeed;

		// Calculate 2D and 3D direction vector
		CalculateDirectionVectors ();
	}

	private void HandleAbility()
	{
		// Windows
		CastAbility(abilityController.Freeze, freezeKey, freezeSound, 0.7f);
		CastAbility(abilityController.Dash, dashKey, dashSound, 0.7f);
		CastAbility(abilityController.Block, blockKey, blockSound, 0.7f);
        CastAbility(abilityController.Block, attackKey, blockSound, 0.7f);
        CastAbility(abilityController.Bomb, bombKey, bombSound, 0.7f);
		CastAbility(abilityController.Attack, attackKey, attackSound, 0.7f);

		// Xbox
		CastAbility(abilityController.Freeze, freezeButton, freezeSound, 0.7f);
		CastAbility(abilityController.Dash, dashButton, dashSound, 0.7f);
		CastAbility(abilityController.Block, blockButton, blockSound, 0.7f);
        //CastAbility(abilityController.Block, attackButton, blockSound, 0.7f);
        CastAbility(abilityController.Bomb, bombButton, bombSound, 0.7f);
		CastAbility(abilityController.Attack, attackButton, attackSound, 0.7f);
        
	}

	private void CastAbility(Ability ability, KeyCode key, AudioClip clip, float volume)
	{
		// Make sure we can use the ability
		if(!ability.canUse)
			return;

        // For all abilites except attack and block
        if ((ability != abilityController.Attack) & (ability != abilityController.Block))
        {
            // Make sure the correct key has been pressed
            if (!Input.GetKeyDown(key))
                return;
        }
        else if (!attack)
        {
            // for attack check if attack has been used
            if (ability == abilityController.Attack)
                return;
            if (ability == abilityController.Block)
            {
                // Make sure the correct key has been pressed
                if (!Input.GetKeyDown(key))
                    return;
            }
        }


        // OLD IMPLEMENTATION // Attack only interacts with block if the block is being placed
        /*
        if (((key == attackButton | key == attackKey) & (ability == abilityController.Block)) & !abilityController.Block.placementActive)
            return;
*/
        // Attack only interacts with block if the block is being placed
        if (((attack) & (ability == abilityController.Block)) & !abilityController.Block.placementActive)
            return;

        // If the block is being placed
        if (abilityController.Block.placementActive)
        {
            if (attack)
            {
                // Cancel block placement
                ability.UseAbility();
            }
            // If the block button is pressed again
            else if (key == blockButton | key == blockKey) 
            {
                // Place the block
                abilityController.Block.DropBlock();
            }
            // If the attack is pressed
            
        }
        else
        {
            // Use ability
            ability.UseAbility();
        }
		
		// Play audio
		audioSource.PlayOneShot(clip, volume);
	}

	private void CalculateDirectionVectors()
	{
		// Dont recalculate if we arent moving
		if (moveVelocity.x + moveVelocity.y == 0)
			return;

		// Create a target
		Vector3 Target = transform.position + new Vector3 (moveVelocity.x, 0, moveVelocity.y);

		directionVector3D = Target - transform.position;
		directionVector3D.Normalize ();

		// Reformat to Local
		directionVector2D = new Vector3(directionVector3D.x, directionVector3D.z, 0.0f);
	}

	public void Knockback(Vector3 position, float force)
	{
		//return;

		Vector3 difference, direction;
		float t, magnitude, maxPos, finalForce;

		// Calculate difference between bomb explosion position and our position
		difference = transform.position - position;

		// Calculate direction and magnitude of the vector
		direction = difference.normalized;
		magnitude = difference.magnitude;

		// Get the magitude of the explosion
		maxPos = (direction*force).magnitude;

		// Calculate how much to explode
		t = (magnitude - 0) / (maxPos - 0);
		t = Mathf.Clamp01(t);

		finalForce = Mathf.Lerp(force, 0, t);

		transform.position = transform.position + (direction*finalForce);
	}

	//Kills this player. Returns false if player already dead.
	public bool Kill()
	{
		if (isAlive)
		{
			isAlive = false;
			respawnTimer = 0.0f;
			GetComponent<Renderer>().enabled = isAlive;
			GetComponent<Animator>().enabled = isAlive;
			GetComponent<Collider>().enabled = isAlive;
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool CheckRespawn()
	{
		if (!isAlive && respawnTimer >= maxRespawnTime)
		{
			return true;
		}
		else
		{
			respawnTimer += Time.deltaTime;
			return false;
		}
	}

	private void Respawn()
	{
        // Find the closest respawn point
        gameController.GetRespawnLocation(gameObject);

        // Reset player allowing thme to move
        isAlive = true;
		GetComponent<Renderer>().enabled = isAlive;
		GetComponent<Animator>().enabled = isAlive;
		GetComponent<Collider>().enabled = isAlive;
	}

	public void Freeze(bool Flag)
	{
		isFrozen = Flag;
		rigidBody.velocity = new Vector3 (0, 0, 0);
		animator.SetBool ("isFrozen", isFrozen);
	}

	public void LockControls(bool Flag)
	{
		controlsLocked = Flag;
	}
}