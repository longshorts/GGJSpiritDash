using UnityEngine;
using System.Collections;

public class Player_Control : MonoBehaviour {

	public float movementSpeed = 15f * 1.2f;
	//public float maxCombinedVelocity
	public int playerNumber;
	public int numberOfPlayers = 2;

	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode leftKey;
	private KeyCode rightKey;

	private Animator animator;

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

		this.animator = this.GetComponent<Animator> ();
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
}
