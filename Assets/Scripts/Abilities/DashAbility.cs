using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashAbility : Ability
{
	[Header("Spell Specific")]
	public LayerMask outsideLayer;				// Layer for the outside walls
	public LayerMask obstacleLayer;				// Layer for the inside obstacles
	public float dashDistance = 15.0f;			// How far to dash
	public float dashBacktrackAmount = 2.0f;	// How far to step backwards
	public float dashSpeed = 2.5f;				// How long to dash from A to B
	public bool isDashing = false;
	public Vector3 dashStart;					// Initial position
	public Vector3 dashFinish;					// Final position
	private float dashFrame;					// Animating frame for interpolation
	private Rigidbody rigidBody;

	void Start()
	{
		playerController = GetComponent<PlayerController>();
		rigidBody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if(isDashing)
			return;

		Debug.DrawRay(transform.position, playerController.directionVector3D * dashDistance, Color.yellow);
	}
		
	public override void CastAbility ()
	{
		if(isDashing)
		{
			//Debug.Log ("Already dashing");
			return;
		}

		dashFrame = 0.0f;

		//Debug.Log ("Cast Dash");
		
		// Freeze controls
		playerController.LockControls(true);
		rigidBody.isKinematic = true;
		
		// Flag we are now dashing and cannot do so again yet
		isDashing = true;

		// Calculate start and end points
		dashStart = transform.position;
		dashFinish = dashStart + (playerController.directionVector3D * dashDistance);

		// Find a suitable location
		CheckDashLocation();
		
		StartCoroutine(Dash ());
		
		// Start Cooldown
		StartCooldown();
	}

	private bool CheckDashLocation()
	{
		RaycastHit hit;

		// Raycast forward, check for collision with the outside wall
		if(Physics.Raycast(dashStart, playerController.directionVector3D, out hit, dashDistance, outsideLayer))
		{
			Debug.Log ("Found an outside wall, backtracking!");

			// Set final point to collision point
			dashFinish.x = hit.point.x;
			dashFinish.z = hit.point.z;

			// Backtrack until we are safe
			dashFinish -= playerController.directionVector3D * dashBacktrackAmount;

			return true;
		}
		else
		{
			// Check if there is something at the target destination
			if(Physics.CheckSphere(dashFinish, 0.01f))
			{
				while(true)
				{
					// Otherwise check if the position will be inside a wall
					if(Physics.CheckSphere(dashFinish, 0.01f))
					{
						dashFinish += playerController.directionVector3D;
						Debug.Log ("Moving");
					}
					else
					{
						Debug.Log ("All clear");
						break;
					}
				}
			}
		}

		return true;
	}

	private IEnumerator Dash()
	{
		while(true)
		{
			// Interpolate between A and B
			dashFrame += Time.deltaTime;
			transform.position = Vector3.Lerp(dashStart, dashFinish, dashFrame / dashSpeed);

			// If we have finished
			if(transform.position.Equals(dashFinish))
			{
				break;
			}
			else
			{
				yield return null;
			}
		}

		isDashing = false;
		playerController.LockControls(false);
		rigidBody.isKinematic = false;

//		Debug.Log ("Finished");
		yield return new WaitForEndOfFrame();
	}
}
