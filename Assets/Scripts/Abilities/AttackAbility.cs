using UnityEngine;
using System.Collections;

public class AttackAbility : Ability
{
	[Header("Spell Specific")]
	public GameObject oppositionTarget;
	public float FOV = 45;
	public float attackRange = 3;
	public float attackDamage = 10;

	void Start()
	{
		// Get access to the opposing player
		if(gameObject.tag.Equals("Player1"))
		{
			oppositionTarget = GameObject.FindGameObjectWithTag("Player2");
		}
		else if(gameObject.tag.Equals("Player2"))
		{
			oppositionTarget = GameObject.FindGameObjectWithTag("Player1");
		}
	}

	void Update()
	{
		// Debugging
		Vector3 LeftRay = Quaternion.AngleAxis(-FOV/2, Vector3.up) * transform.GetComponent<PlayerController>().directionVector3D;
		Vector3 RightRay = Quaternion.AngleAxis(FOV/2, Vector3.up) * transform.GetComponent<PlayerController>().directionVector3D;
		Debug.DrawRay(transform.position, LeftRay * attackRange);
		Debug.DrawRay(transform.position, RightRay * attackRange);
		Debug.DrawRay(transform.position, transform.forward * attackRange);
	}

	public override void CastAbility ()
	{
		canUse = false;

		SlashForward();

		// Start Cooldown
		StartCoroutine (Cooldown ());
	}

	private void SlashForward()
	{


		// Calculate a direction vector between us and the opposition
		Vector3 direction = oppositionTarget.transform.position - transform.position;
		
		// Calculate the angle between the guard and animal
		float angle = Vector3.Angle(direction, transform.GetComponent<PlayerController>().directionVector3D);
		
		// Check if we are within the field of view
		if(angle < FOV * 0.5f)
		{
			RaycastHit hit;
			
			// Create a ray between the our position and the target position
			if(Physics.Raycast(transform.position, direction.normalized, out hit, attackRange))
			{
				// Check if we hit the player
				if(hit.collider.gameObject.tag == oppositionTarget.tag)
				{
					Debug.Log ("Found player");
					// Attack player -- TODO
					//oppositionTarget.GetComponent<Player>().ReceiveDamage(attackDamage);
				}
			}
		}
	}
}

