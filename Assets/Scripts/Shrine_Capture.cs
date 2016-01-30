using UnityEngine;
using System.Collections;

public class Shrine_Capture : MonoBehaviour {
		
	public float conversionLimit = 3.0f;	// how many seconds for the shrine to be captured
	public bool unclaimed;					// if the shrine has no owner
	public bool player1Control;				// if the shrine is owned by player 1
	public bool player2Control;				// if the shrine is owned by player 2
	public GameObject owner;				// reference to the player that owns the shrine
	public int shrineID;					// int referring to shrine number

	private float conversionSpeed = 1.0f;	// speed at which the shrine is captured
	private float player1Progress;			// how much control the players have over the shrine (between -conversionLimit and +conversionLimit)
	private float player2Progress;

	// Use this for initialization
	void Start () {
	
		// initialise the shrine
		player1Progress = 0;
		player2Progress = 0;
		unclaimed = true;
		player1Control = false;
		player2Control = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if ((player1Progress < (conversionLimit / 2)) & (player2Progress < (conversionLimit / 2))) {	// if neither player has majority control over shrine

			unclaimed = true;			// shrine is unclaimed
			player1Control = false;
			player2Control = false;
		}

		// reference the to the player which owns the shrine
		if (player1Control) {
			owner = GameObject.Find ("PlayerCharacter1");
		} else if (player2Control) {
			owner = GameObject.Find ("PlayerCharacter2");
		} else {
			owner = null;
		}
	}

	void OnTriggerStay(Collider other) {	// if a player is on the shrine

		if (other.gameObject.tag == "Player1") {	// if it is player 1

			if (unclaimed | player2Control) {	// if the player does not already control it

				player1Progress += (conversionSpeed * Time.deltaTime);	// increase players control
				player2Progress -= (conversionSpeed * Time.deltaTime);	// decrease other player's control
			}
		}

		if (other.gameObject.tag == "Player2") {	// do the same for player 2

			if (unclaimed | player1Control) {
				
				player2Progress += (conversionSpeed * Time.deltaTime);
				player1Progress -= (conversionSpeed * Time.deltaTime);
			}
		}

		if (player1Progress > conversionLimit) {	// if the player has control over the shrine

			player1Control = true;		// set shrine control booleans
			unclaimed = false;
			player2Control = false;
		} else if (player2Progress > conversionLimit) { // do the same for the other player

			player2Control = true;
			unclaimed = false;
			player1Control = false;
		}
	}
}
