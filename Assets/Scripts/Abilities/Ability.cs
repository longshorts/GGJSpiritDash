using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{
	[Header("Limitations")]
	public bool hasAbility = false;
	public bool canUse = false;
	public float abilityCooldown = 10.0f;

	[Header("Component")]
	public PlayerController playerController;

	[Header("Collision")]
	public RaycastHit hit;

	void Start ()
	{
	
	}

	// FUNCTIONALITY
	public void GiveAbility()
	{
		hasAbility = true;
		canUse = true;
	}
	public void TakeAbility()
	{
		hasAbility = false;
		canUse = false;
	}

	// CASTING
	public void UseAbility()
	{
		// Make sure we have the ability
		if (!hasAbility)
			return;

		// Make sure we can use it
		if (!canUse)
			return;

		// Alls good so use it
		CastAbility ();
	}

	public virtual void CastAbility()
	{
		// OVERRIDE THIS
	}

	// COOLDOWN
	public IEnumerator Cooldown()
	{
		canUse = false;
		yield return new WaitForSeconds(abilityCooldown);
		canUse = true;
	}
}