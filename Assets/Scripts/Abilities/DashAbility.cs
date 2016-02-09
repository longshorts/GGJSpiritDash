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

		dashStart = transform.position;
		dashFinish = dashStart + (dashDistance * playerController.directionVector3D);
		
		Vector3 direction = dashStart - dashFinish;
		direction.Normalize();

		Debug.DrawLine(dashStart, dashFinish);
	}
		
	public override void CastAbility ()
	{
		if(isDashing)
		{
			Debug.Log ("Already dashing");
			return;
		}

		dashFrame = 0.0f;

		Debug.Log ("Cast Dash");
		
		// Freeze controls
		playerController.LockControls(true);
		rigidBody.isKinematic = true;
		
		// Flag we are now dashing and cannot do so again yet
		isDashing = true;

		// Calculate start and end points
		dashStart = transform.position;
		dashFinish = dashStart + (dashDistance * playerController.directionVector3D);

		// Find a suitable location
		CheckDashLocation();

		StartCoroutine(Dash ());
		
		// Start Cooldown
		StartCooldown();
	}

	private void CheckDashLocation()
	{
		Vector3 directionTo = dashFinish - dashStart;
		Vector3 directionFrom = dashStart - dashFinish;
		directionTo.Normalize();
		directionFrom.Normalize();

		float distance = Vector3.Distance(dashStart, dashFinish);

		// Raycast to target position
		// If the raycast hits an outside wall
		if(Physics.Raycast(dashStart, directionFrom, distance, 1 << outsideLayer))
		{
			Debug.Log ("Found an outside wall");

			// Backtrack position
			dashFinish = dashFinish + (directionFrom*dashBacktrackAmount);

			return;
		}
		else
		{
			Debug.Log ("Found a inside wall");

			// Otherwise check if the position will be inside a wall
			if(Physics.CheckSphere(dashFinish, 0.01f))
			{
				// If it is, increase length until its not
				while(true)
				{
					dashFinish += directionTo * Time.deltaTime;
					if(!Physics.CheckSphere(dashFinish, 0.01f))
					{
						break;
					}
				}
			}
		}
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

		Debug.Log ("Finished");
		yield return new WaitForEndOfFrame();
	}
}
