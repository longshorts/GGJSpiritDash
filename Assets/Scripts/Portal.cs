using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {


	// reference to player 1
	// reference to player 2
	private Game_Controller gameController;
	private BoxCollider thisCollider;

	public AudioClip ggSound;
	public AudioClip portalSound;
	private AudioSource audio;

	// Use this for initialization
	void Start () {

		thisCollider = GetComponent<BoxCollider> ();
		thisCollider.enabled = false;
		//assign reference
		gameController = GameObject.Find ("GameController").GetComponent<Game_Controller> ();
		audio = GetComponent<AudioSource> (); 
	}
	
	// Update is called once per frame
	void Update () {
	
		if (gameController.PlayerOne.Complete | gameController.PlayerTwo.Complete) {

			thisCollider.enabled = true;
			audio.PlayOneShot(portalSound, 0.7f);

	}


	}

	void OnTriggerEnter (Collider other) {
	
		if(gameController.PlayerOne.Complete & (other.tag == "Player1")) {

			gameController.PlayerOne.Victory = true;
			audio.PlayOneShot(ggSound, 0.7f);
			print ("victory for Player 1");
		} else if (gameController.PlayerTwo.Complete & (other.tag == "Player2")) {
			gameController.PlayerTwo.Victory = true;
			audio.PlayOneShot(ggSound, 0.7f);
			print ("victory for Player 2");
		}
	}
}
