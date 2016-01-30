using UnityEngine;
using System.Collections;

public class Player_Control : MonoBehaviour {

	public float movementSpeed = 1.0f;
	public int playerNumber;
	public int numberOfPlayers = 2;

	public Vector2 velocity;
	private Rigidbody thisRB;
	
	// Use this for initialization
	void Start () {

		thisRB = GetComponent<Rigidbody> ();	// assign the character rigid body to this movement script
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

		if (playerNumber == 1) {
		
			// get new velocity depending on player control
			if (Input.GetKey (KeyCode.W)) {
				velocity.y = movementSpeed;
			}
			if (Input.GetKey (KeyCode.S)) {
				velocity.y = -1 * movementSpeed;
			}
			if (Input.GetKey (KeyCode.D)) {
				velocity.x = movementSpeed;
			}
			if (Input.GetKey (KeyCode.A)) {
				velocity.x = -1 * movementSpeed;
			}
		} else if (playerNumber == 2) {
		
			// get new velocity depending on player control
			if (Input.GetKey (KeyCode.UpArrow)) {
				velocity.y = movementSpeed;
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				velocity.y = -1 * movementSpeed;
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				velocity.x = movementSpeed;
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				velocity.x = -1 * movementSpeed;
			}
		}



	}
}
