﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	[Header("Properties")]
	public float movementSpeed = 18.0f;
	public int playerNumber = 1;

	// Window Keys
	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode leftKey;
	private KeyCode rightKey;

	private KeyCode FreezeKey;
	private KeyCode BlockKey;
	private KeyCode DashKey;
	private KeyCode BombKey;

	// Xbox Controller
	private KeyCode FreezeButton;
	private KeyCode BlockButton;
	private KeyCode DashButton;
	private KeyCode BombButton;

	private Animator animator;

	// Movement
	public Vector2 moveVelocity;
	public bool isFrozen;
	public bool controlsLocked;
	private Rigidbody rigidBody;

	public Vector3 directionVector3D;
	public Vector3 directionVector2D;

	private AbilityController abilityController;

	public AudioClip blockSound;
	public AudioClip bombSound;
	public AudioClip dashSound;
	public AudioClip freezeSound;
	private AudioSource audioSource;

	void Start ()
	{
		// assign the character rigid body to this movement script
		rigidBody = GetComponent<Rigidbody> ();
	
		// Set input keys
		AssignInput();

		// Access abilties
		abilityController = gameObject.GetComponent<AbilityController>();

		// Initialise to unfrozen
		isFrozen = false;
		animator.SetBool ("isFrozen", isFrozen);

		// Initialise direction vectors
		directionVector3D = new Vector3 (0, 0, -1);		// World Space
		directionVector2D = new Vector3(0, -1, 0);		// Local 2D

		// Initialise audio
		audioSource = GetComponent<AudioSource> ();
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
		
		animator = GetComponent<Animator> ();
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
		CastAbility(abilityController.Freeze, FreezeKey, freezeSound, 0.7f);
		CastAbility(abilityController.Dash, DashKey, dashSound, 0.7f);
		CastAbility(abilityController.Block, BlockKey, blockSound, 0.7f);
		CastAbility(abilityController.Bomb, BombKey, bombSound, 0.7f);

		// Xbox
		CastAbility(abilityController.Freeze, FreezeButton, freezeSound, 0.7f);
		CastAbility(abilityController.Dash, DashButton, dashSound, 0.7f);
		CastAbility(abilityController.Block, BlockButton, blockSound, 0.7f);
		CastAbility(abilityController.Bomb, BombButton, bombSound, 0.7f);
	}

	private void CastAbility(Ability ability, KeyCode key, AudioClip clip, float volume)
	{
		// Make sure we can use the ability
		if(!ability.canUse)
			return;

		// Make sure the correct key has been pressed
		if(!Input.GetKeyDown(key))
			return;

		// Use ability
		ability.UseAbility();

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

		transform.position = Vector3.Lerp (transform.position, transform.position + (direction*finalForce), t);
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