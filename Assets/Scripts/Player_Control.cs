using UnityEngine;
using System.Collections;

public class Player_Control : MonoBehaviour {

	public float movementSpeed = 1.2f;
	public int playerNumber;
	public int numberOfPlayers = 2;

	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode leftKey;
	private KeyCode rightKey;

	public Vector2 velocity;
	private Rigidbody thisRB;
	
	// Use this for initialization
	void Start () {

		thisRB = GetComponent<Rigidbody> ();	// assign the character rigid body to this movement script
	
		switch (playerNumber) {
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
	void Update () {

		GetInput ();

		// move the character
		thisRB.velocity = new Vector3 (velocity.x, 0, velocity.y);
	}
	

	void GetInput () {

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
}
