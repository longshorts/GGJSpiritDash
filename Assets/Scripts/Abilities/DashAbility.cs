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

	private Player_Control Player;
	private RaycastHit hit;
	private Ray ray;

	void Start()
	{
		Player = GetComponent<Player_Control>();
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

		transform.position = Vector3.Lerp(transform.position, DashTargetLocation, DashSpeed);

		if(transform.position.Equals(DashTargetLocation))
		{
			IsDashing = false;
			Player.Freeze(false);
			StartCoroutine(Cooldown());
		}
	}

	bool CheckLocation () {

		ray.origin = transform.position;
		Vector3 facing = new Vector3 (Player.velocity.x, 0, Player.velocity.y);
		facing = facing.normalized;
		ray.direction = facing;
		Debug.DrawRay(ray.origin, (DashAmount * facing), Color.green, 30.0f,false);
		
		if (Physics.Raycast (ray, out hit, 20.0f)) {

			if (!hit.collider.isTrigger)
			{
				if (hit.distance < 4.0f)
				{
					ray.origin = transform.position + (DashAmount * new Vector3 (Player.velocity.x, 0, Player.velocity.y));
					Vector3 facing2 = new Vector3 (Player.velocity.x, 0, Player.velocity.y);
					facing2 = facing2.normalized;
					ray.direction = facing;
					
					if (Physics.Raycast (ray, out hit, 11.0f)) {	// change this float for being able to jump across small obstacles(go lower) - will get stuck in large obstacles

						if (!hit.collider.isTrigger)
						{
							Debug.Log ("Wall in the way");
							return false;
						} else {
							return true;
						}
					}
				} else if (hit.distance < 16.0f) {
					Debug.Log ("Wall in the way");
					return false;
				}

			} else {
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

		if (!CheckLocation ()) {
			return;
		}

		// Freeze controls
		Player.Freeze(true);

		// Flag we are now dashing and cannot do so again yet
		IsDashing = true;
		CanUseAbility = false;

		DashTargetLocation = transform.position + (DashAmount * new Vector3 (Player.velocity.x, 0, Player.velocity.y));
		/*
		// Calculate how much to move
		Vector3 MoveAmount = this.gameObject.transform.forward * DashAmount;

		// Calculate our new target position
		DashTargetLocation = transform.position + MoveAmount;
		*/
	}

	private IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(CooldownDuration);
		CanUseAbility = true;
	}
}
