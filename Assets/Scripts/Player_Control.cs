using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Control : MonoBehaviour {

	[Header("Properties")]
	public float movementSpeed = 1.2f;
	public int playerNumber;
	public int numberOfPlayers = 2;

	// Window Keys
	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode leftKey;
	private KeyCode rightKey;

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

		velocity.x = movementSpeed * h;
		velocity.y = movementSpeed * v * -1;

		if (velocity.x != 0 && velocity.y != 0)
			return;

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

		if(Input.GetKey(KeyCode.P))
		{
			Abilities.Freeze.GrantAbility();
			Abilities.Dash.GrantAbility();
			Abilities.Block.GrantAbility();
			Abilities.Bomb.GrantAbility();
		}
	}

	public void Freeze(bool Flag)
	{
		isFrozen = Flag;
	}
}
