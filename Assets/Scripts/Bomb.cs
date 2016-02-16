using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
	[Header("Properties")]
	public float explosionForce = 10.0f;
	public float explosionRadius = 2.0f;
	public Vector3 moveDirection3D;
	public Vector3 moveDirection2D;
	private Rigidbody rigidBody;
	private RaycastHit hit;

	[Header("Audio")]
	public AudioClip explodeSound;
	private AudioSource audioSource;

	[Header("Animation")]
	public Animator animator;
	public Animation ExplodeAnimation;

	public bool IsExploding = false;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource> (); 
		animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider collision)
	{
		if(!IsExploding)
		{
			// Trigger explosion
			animator.SetTrigger("Explode");
			IsExploding = true;
			rigidBody.velocity = new Vector3();
			GetComponent<SphereCollider>().radius = explosionRadius;

			// Play Audio
			audioSource.PlayOneShot(explodeSound, 0.7f);

			// Wait for animation to finish then destroy
			StartCoroutine(Explode());
		}

		// Loop through the colliders
		if(collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
		{			
			// Knockback the player
			collision.gameObject.GetComponent<PlayerController>().Kill();
		}
	}

	public void Initialise(Vector3 Direction, float Speed)
	{
		// Store Direction
		moveDirection3D = Direction;
		moveDirection2D = new Vector3(moveDirection3D.x, 0, moveDirection3D.y);

		// Apply velocity to bomb
		GetComponent<Rigidbody>().velocity = moveDirection3D * Speed;

		// Update Name
		gameObject.name = "Player Bomb";
	}

	private IEnumerator Explode()
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("Bomb_Move"))
		{
			yield return null;
		}

		// Scale size of explosion
		transform.localScale = new Vector3(3,3,3);

		// Wait the length of the explosion
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		
		// Delete self
		Destroy (gameObject);

		yield return new WaitForEndOfFrame();
	}
}