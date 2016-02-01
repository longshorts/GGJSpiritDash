using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
	[Header("Properties")]
	public float ExplosionForce = 10.0f;
	public float ExplosionRadius = 10.0f;
	public Vector3 MoveDirection;
	public Vector3 LocalMoveDirection;
	private Rigidbody rigidBody;
	private RaycastHit hit;

	[Header("Audio")]
	public AudioClip explodeSound;
	private AudioSource audio;

	[Header("Animation")]
	public Animator animator;
	public Animation ExplodeAnimation;

	public bool IsExploding = false;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource> (); 
		animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.Log ("Move Direction : " + MoveDirection);

		// Check for collision
		if(Physics.Raycast(transform.position, MoveDirection, out hit, 1.0f) && !IsExploding)
		{
			// Trigger explosion
			animator.SetTrigger("Explode");
			IsExploding = true;
			rigidBody.velocity = new Vector3();
			transform.localScale = new Vector3(3,3,3);

			// Wait for animation to finish then destroy
			StartCoroutine(Explode());

			Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

			foreach (Collider col in colliders)
			{
				if((col.tag == "Player1") || (col.tag == "Player2"))
				{
					// Play Audio
					audio.PlayOneShot(explodeSound, 0.7f);

					// Knockback the player
					col.GetComponent<PlayerController>().Knockback(transform.position, ExplosionForce);
				}
			}
		}
	}

	public void Initialise(Vector3 Direction, float Speed)
	{
		// Store Direction
		MoveDirection = Direction;
		LocalMoveDirection = new Vector3(MoveDirection.x, 0, MoveDirection.y);

		// Apply velocity to bomb
		GetComponent<Rigidbody>().velocity = MoveDirection * Speed;

		// Update Name
		gameObject.name = "Player Bomb";
	}

	private IEnumerator Explode()
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("Bomb_Move"))
		{
			yield return null;
		}

		// Wait the length of the explosion
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		
		// Delete self
		Debug.Log ("Gone!");
		Destroy (gameObject);

		yield return new WaitForEndOfFrame();
	}
}