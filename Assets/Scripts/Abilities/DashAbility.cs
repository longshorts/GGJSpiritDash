using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashAbility : Ability
{
	[Header("Spell Specific")]
	public float dashDistance = 15.0f;
	public float dashSpeed = 2.5f;
	public bool isDashing = false;
	public Vector3 dashStart;
	public Vector3 dashFinish;
	private Rigidbody rigidBody;

	void Start()
	{
		playerController = GetComponent<PlayerController>();
		rigidBody = GetComponent<Rigidbody> ();
	}
		
	public override void CastAbility ()
	{
		Debug.Log ("Cast Dash");
		if(isDashing)
		{
			Debug.Log ("Already dashing");
			return;
		}
		
		// Freeze controls
		playerController.LockControls(true);
		
		// Flag we are now dashing and cannot do so again yet
		isDashing = true;
		canUse = false;

		dashStart = transform.position;
		dashFinish = dashStart + (dashDistance * playerController.directionVector3D);
		rigidBody.velocity = dashDistance * playerController.directionVector3D;


		// Start Cooldown
		StartCoroutine (Cooldown ());

		// Start Dash
		StartCoroutine(Dash ());
	}

	private bool CheckLocation ()
	{
		Ray ray = new Ray (transform.position, playerController.directionVector3D);
		Debug.DrawRay(ray.origin, (dashDistance * ray.direction), Color.green, 30.0f,false);
		
		if (Physics.Raycast (ray, out hit, 20.0f))
		{
			if (!hit.collider.isTrigger)
			{
				if (hit.distance < 4.0f)
				{
					ray.origin = transform.position + (dashDistance * new Vector3 (playerController.moveVelocity.x, 0, playerController.moveVelocity.y));
					Vector3 facing2 = new Vector3 (playerController.moveVelocity.x, 0, playerController.moveVelocity.y);
					facing2 = facing2.normalized;
					ray.direction = playerController.directionVector3D;

					// change this float for being able to jump across small obstacles(go lower) - will get stuck in large obstacles
					if (Physics.Raycast (ray, out hit, 11.0f))
					{
						if (!hit.collider.isTrigger)
						{
							Debug.Log ("Wall in the way");
							return false;
						}
						else
						{
							return true;
						}
					}
				}
				else if (hit.distance < 16.0f)
				{
					Debug.Log ("Wall in the way");
					return false;
				}

			}
			else
			{
				return true;
			}
		}

		return true;

	}

	private IEnumerator Dash()
	{
		//while(true)
		//{
			//yield return null;
		//}

		Debug.Log ("Finished");
		yield return new WaitForEndOfFrame();

		// Start cooldown
		StartCoroutine(Cooldown ());
	}
}
