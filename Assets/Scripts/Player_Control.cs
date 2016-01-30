using UnityEngine;
using System.Collections;

public class Player_Control : MonoBehaviour {

	public float movementSpeed = 1.0f;
	public int playerNumber;
	public int numberOfPlayers = 2;

	private Vector2 velocity;
//	private KeyCode p1L, p1R, p1U, p1D, p1A1, p1A2, p1A3;
//	private KeyCode p2L, p2R, p2U, p2D, p2A1, p2A2, p2A3;
//	private KeyCode[] left, right, up, down, ap1, ap2, ap3;


	// Use this for initialization
	void Start () {
	/*
		KeyCode[] left = new KeyCode[2];
		KeyCode[] right = new KeyCode[2];
		KeyCode[] up = new KeyCode[2];
		KeyCode[] down = new KeyCode[2];
		KeyCode[] ap1 = new KeyCode[2];
		KeyCode[] ap2 = new KeyCode[2];
		KeyCode[] ap3 = new KeyCode[2]; 

		left[0] = KeyCode.A;
		right[0] = KeyCode.D;
		up[0] = KeyCode.W;
		down[0] = KeyCode.S;
		ap1[0] = KeyCode.Alpha1;
		ap2[0] = KeyCode.Alpha2;
		ap3[0] = KeyCode.Alpha3;

		left[1] = KeyCode.LeftArrow;
		right[1] = KeyCode.RightArrow;
		up[1] = KeyCode.UpArrow;
		down[1] = KeyCode.DownArrow;
		ap1[1] = KeyCode.Keypad1;
		ap2[1] = KeyCode.Keypad2;
		ap3[1] = KeyCode.Keypad3;
		*/
	}
	
	// Update is called once per frame
	void Update () {

		GetInput ();

		// move the character

		transform.Translate (velocity.x * Time.deltaTime, 0, velocity.y * Time.deltaTime);

	}
	

	void GetInput () {

		// reset velocity
		velocity.x = 0;
		velocity.y = 0;

		if (playerNumber == 1) {
		
			// apply velocity depending on player control
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
		
			// apply velocity depending on player control
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
