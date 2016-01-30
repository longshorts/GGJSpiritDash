using UnityEngine;
using System.Collections;

public class Shrine_Capture : MonoBehaviour {

	public float conversionSpeed = 1.0f;
	public float conversionLimit = 3.0f;
	public bool unclaimed;
	public bool player1Control;
	public bool player2Control;
	public GameObject owner;

	private float player1Progress;
	private float player2Progress;

	// Use this for initialization
	void Start () {
	
		player1Progress = 0;
		player2Progress = 0;
		unclaimed = true;
		player1Control = false;
		player2Control = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if ((player1Progress < (conversionLimit / 2)) & (player2Progress < (conversionLimit / 2))) {

			unclaimed = true;
			player1Control = false;
			player2Control = false;
		}

		if (player1Control) {
			owner = GameObject.Find ("PlayerCharacter1");
		} else if (player2Control) {
			owner = GameObject.Find ("PlayerCharacter2");
		} else {
			owner = null;
		}
	}

	void OnTriggerStay(Collider other){

		if (other.gameObject.tag == "Player1") {
			if (unclaimed | player2Control) {

				player1Progress += (conversionSpeed * Time.deltaTime);
				player2Progress -= (conversionSpeed * Time.deltaTime);
			}
		}

		if (other.gameObject.tag == "Player2") {
			if (unclaimed | player1Control) {
				
				player2Progress += (conversionSpeed * Time.deltaTime);
				player1Progress -= (conversionSpeed * Time.deltaTime);
			}
		}

		if (player1Progress > conversionLimit) {

			player1Control = true;
			unclaimed = false;
			player2Control = false;
		} else if (player2Progress > conversionLimit) {

			player2Control = true;
			unclaimed = false;
			player1Control = false;
		}
	}
}
