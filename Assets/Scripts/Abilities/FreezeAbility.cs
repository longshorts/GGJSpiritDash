using UnityEngine;
using System.Collections;

public class FreezeAbility : Ability
{
	[Header("Spell Specific")]
	public float freezeDuration = 5.0f;					// How long to freeze anything inside the radius
	public float freezeRadius = 5.0f;					// How big the radius of the cast is
	public float freezeSpeed = 1.0f;					// How long it take to radiate
	public Vector3 startScale = new Vector3(0,0,0);		// Initial scale (cant be seen)
	public Vector3 targetScale = new Vector3(1,1,1);	// Final scale at end of cast
	public GameObject frostPrefab;						// Access to the prefab of the frost spell
	public SphereCollider sphereCollider;				// Sphere collider for checking collision
	public float freezeFrame = 0.0f;					// Interpolating frame
	private GameObject frostEffect;						// Access to runtime object
	private GameObject targetPlayer;					// Access to your opponent

	void Start()
	{
		// Get access to the opposing player
		if(gameObject.tag.Equals("Player1"))
		{
			targetPlayer = GameObject.FindGameObjectWithTag("Player2");
			playerController = targetPlayer.GetComponent<PlayerController>();
		}
		else if(gameObject.tag.Equals("Player2"))
		{
			targetPlayer = GameObject.FindGameObjectWithTag("Player1");
			playerController = targetPlayer.GetComponent<PlayerController>();
		}

		// Define target scale from radius
		targetScale = new Vector3(freezeRadius, freezeRadius, freezeRadius);

		// Initialise the effect
		frostEffect = (GameObject)Instantiate(frostPrefab) as GameObject;
		frostEffect.SetActive(false);

		// Create a sphere collider and attach to the player
		sphereCollider = GetComponent<SphereCollider>();
		sphereCollider.isTrigger = true;
		sphereCollider.enabled = false;
		sphereCollider.radius = 0.0f;

		Physics.IgnoreCollision(GetComponent<Collider>(), sphereCollider);
	}

	void OnTriggerEnter(Collider collider)
	{
		// Make sure the collider isn't ours and its not a trigger
		if(collider.gameObject.tag.Equals(targetPlayer.gameObject.tag) && !collider.isTrigger)
		{
			// If the player isnt already frozen
			if(!playerController.isFrozen)
			{
				// Freeze player
				StartCoroutine(FreezePlayer(collider.gameObject));
			}
		}
	}

	public override void CastAbility ()
	{
		Debug.Log ("Cast Freeze");

		// Initialise animation
		freezeFrame = 0.0f;

		// Initialise the object
		frostEffect.SetActive(true);
		frostEffect.transform.localScale = startScale;
		frostEffect.transform.position = transform.position;
		sphereCollider.enabled = true;
		sphereCollider.radius = frostEffect.transform.localScale.x;

		// Start Casting
		StartCoroutine(EmitFreeze());

		// Start Cooldown
		StartCooldown();
	}

	private IEnumerator EmitFreeze()
	{
		while(true)
		{
			// Increase interpolation
			freezeFrame += Time.deltaTime;

			// Increase scale through interpolation
			frostEffect.transform.localScale = Vector3.Lerp(startScale, targetScale, freezeFrame / freezeSpeed);

			// Increase radius
			sphereCollider.radius = frostEffect.transform.localScale.x;

			// Check if we have finished
			if(sphereCollider.radius.Equals(freezeRadius))
			{
				Debug.Log("Finished");
				break;
			}

			yield return null;
		}

		// Deactivate objects
		sphereCollider.enabled = false;
		frostEffect.SetActive(false);

		yield return new WaitForEndOfFrame();
	}

	private IEnumerator FreezePlayer(GameObject obj)
	{
		// Freeze the Player
		obj.GetComponent<PlayerController>().Freeze(true);

		// Wait for unfreeze
		yield return new WaitForSeconds(freezeDuration);

		// Unfreeze player
		obj.GetComponent<PlayerController>().Freeze (false);
	}
}