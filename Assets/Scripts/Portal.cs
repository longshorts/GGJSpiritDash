using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {


	// reference to player 1
	// reference to player 2
	private Game_Controller gameController;
	private BoxCollider thisCollider;

	// Use this for initialization
	void Start () {

		thisCollider = GetComponent<BoxCollider> ();
		thisCollider.enabled = false;
		//assign reference
		gameController = GameObject.Find ("GameController").GetComponent<Game_Controller> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (gameController.PlayerOne.Complete | gameController.PlayerTwo.Complete) {

		thisCollider.enabled = true;
	}


	}

	void OnTriggerEnter (Collider other) {
	
		if(gameController.PlayerOne.Complete & (other.tag == "Player1")) {

			gameController.PlayerOne.Victory = true;
			print ("victory for Player 1");
		} else if (gameController.PlayerTwo.Complete & (other.tag == "Player2")) {
			gameController.PlayerTwo.Victory = true;
			print ("victory for Player 2");
		}



	}
}
