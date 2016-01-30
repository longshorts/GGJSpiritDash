using UnityEngine;
using System.Collections;

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

		// Freeze controls
		Player.Freeze(true);

		// Flag we are now dashing and cannot do so again yet
		IsDashing = true;
		CanUseAbility = false;

		// Calculate how much to move
		Vector3 MoveAmount = this.gameObject.transform.forward * DashAmount;

		// Calculate our new target position
		DashTargetLocation = transform.position + MoveAmount;
	}

	private IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(CooldownDuration);
		CanUseAbility = true;
	}
}
