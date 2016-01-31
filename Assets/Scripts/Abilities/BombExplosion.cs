using UnityEngine;
using System.Collections;

public class BombExplosion : MonoBehaviour
{
	[Header("Properties")]
	public float ExplosionForce = 10.0f;
	public float ExplosionRadius = 10.0f;
	public Vector3 heading;
	private Rigidbody rigidBody;
	private RaycastHit hit;

	public AudioClip explodeSound;
	private AudioSource audio;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource> (); 
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Check for collision
		if(Physics.Raycast(transform.position, rigidBody.velocity.normalized, out hit, 1.0f))
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

			foreach (Collider col in colliders)
			{
				if((col.transform.gameObject.tag == "Player1") | (col.transform.gameObject.tag == "Player2"))
				{
					audio.PlayOneShot(explodeSound, 0.7f);
					// Knockback the player
					col.transform.gameObject.GetComponent<Player_Control>().Knockback(transform.position, ExplosionForce);
				}

				// Calculate a direction vector between the explosion and the collision
				// Create a knockback movement to the player based on direction * moveamount
			}
			
			Destroy (gameObject);
		}
	}
}