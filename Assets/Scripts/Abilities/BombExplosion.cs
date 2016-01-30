using UnityEngine;
using System.Collections;

public class BombExplosion : MonoBehaviour
{
	[Header("Properties")]
	public float ExplosionForce = 10.0f;
	public float ExplosionRadius = 10.0f;
	private Rigidbody rigidBody;
	private RaycastHit hit;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Check for collision
		if(Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
		{
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);

			foreach (Collider col in colliders)
			{
				Rigidbody rb = col.GetComponent<Rigidbody>();
				
				if (rb != null)
				{
					rb.AddExplosionForce(ExplosionForce, explosionPos, ExplosionRadius, 3.0F);
				}
			}
			
			Destroy (gameObject);
		}
	}
}
