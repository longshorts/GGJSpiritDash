using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashAbility : MonoBehaviour
{
	[Header("Properties")]
	public float DashAmount = 15.0f;
	public float DashSpeed = 2.5f;
	public float CooldownDuration = 10.0f;
	
	[Header("Usability")]
	public bool HasAbility = false;
	public bool CanUseAbility = false;
	public bool IsDashing = false;

	public Vector3 DashTargetLocation;

	private PlayerController playerController;
	private RaycastHit hit;
	private Ray ray;

	void Start()
	{
		playerController = GetComponent<PlayerController>();
	}

	public void GrantAbility()
	{
		HasAbility = true;
		CanUseAbility = true;
	}
	
	public void TakeAbility()
	{
		HasAbility = false;
		CanUseAbility = false;
	}

	void Update()
	{
		if(!IsDashing)
		{
			return;
		}

		return;

		transform.position = Vector3.Lerp(transform.position, DashTargetLocation, DashSpeed);

		if(transform.position.Equals(DashTargetLocation))
		{
			IsDashing = false;
			playerController.LockControls(false);
			StartCoroutine(Cooldown());
		}
	}

	bool CheckLocation ()
	{
		ray.origin = transform.position;
		ray.direction = playerController.DirectionVector;
		Debug.DrawRay(ray.origin, (DashAmount * ray.direction), Color.green, 30.0f,false);
		
		if (Physics.Raycast (ray, out hit, 20.0f))
		{
			if (!hit.collider.isTrigger)
			{
				if (hit.distance < 4.0f)
				{
					ray.origin = transform.position + (DashAmount * new Vector3 (playerController.velocity.x, 0, playerController.velocity.y));
					Vector3 facing2 = new Vector3 (playerController.velocity.x, 0, playerController.velocity.y);
					facing2 = facing2.normalized;
					ray.direction = playerController.DirectionVector;

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
	
	public void UseAbility ()
	{
		if(!HasAbility)
		{
			Debug.Log ("Cannot use ability");
			return;
		}

		if(!CanUseAbility)
		{
			Debug.Log ("Cannot use ability");
			return;
		}

		if(IsDashing)
		{
			Debug.Log ("Already dashing");
			return;
		}

		if (!CheckLocation ())
		{
			return;
		}

		// Freeze controls
		playerController.LockControls(true);

		// Flag we are now dashing and cannot do so again yet
		IsDashing = true;
		CanUseAbility = false;

		DashTargetLocation = transform.position + (DashAmount * playerController.DirectionVector);
		GetComponent<Rigidbody>().velocity = DashAmount * playerController.DirectionVector;

		StartCoroutine(Dash ());
	}

	private IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(CooldownDuration);
		CanUseAbility = true;
	}

	private bool Move()
	{
		transform.position = Vector3.Lerp(transform.position, DashTargetLocation, DashSpeed * Time.deltaTime);
		if(Vector3.Distance(transform.position, DashTargetLocation) < 0.5f)
		{
			GetComponent<Rigidbody>().velocity = new Vector3();
			IsDashing = false;
			playerController.LockControls(false);
			StartCoroutine(Cooldown());
			return false;
		}

		return true;
	}

	private IEnumerator Dash()
	{
		while(true)
		{
			if(Move ())
			{
				yield return null;
			}
			else
			{
				break;
			}
		}

		Debug.Log ("Finished");
		yield return new WaitForEndOfFrame();
	}
}
