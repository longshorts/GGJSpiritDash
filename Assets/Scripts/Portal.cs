using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
	public GameController gameController;
	private BoxCollider boxCollider;

	public AudioClip ggSound;
	public AudioClip portalSound;
	private AudioSource audio;

	public GameObject Trapdoor;

	// Use this for initialization
	void Start () {

		// Disable collider
		boxCollider = GetComponent<BoxCollider> ();
		boxCollider.enabled = false;

		//assign reference
		audio = GetComponent<AudioSource> (); 
		Trapdoor.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (gameController.PlayerOne.Complete || gameController.PlayerTwo.Complete)
		{

			Trapdoor.SetActive(true);
			boxCollider.enabled = true;
			audio.PlayOneShot(portalSound, 0.7f);

	}


	}

	void OnTriggerEnter (Collider other)
	{
		if(gameController.PlayerOne.Complete & (other.tag == "Player1"))
		{
			gameController.PlayerOne.Victory = true;
			audio.PlayOneShot(ggSound, 0.7f);
			print ("victory for Player 1");
		}
		else if (gameController.PlayerTwo.Complete & (other.tag == "Player2"))
		{
			gameController.PlayerTwo.Victory = true;
			audio.PlayOneShot(ggSound, 0.7f);
			print ("victory for Player 2");
		}
	}
}
