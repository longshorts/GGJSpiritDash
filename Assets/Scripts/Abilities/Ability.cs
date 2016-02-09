using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{
	[Header("Limitations")]
	public bool hasAbility = false;
	public bool canUse = false;
	public float cooldownProgress;
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

	public void StartCooldown()
	{
		if(!canUse)
			return;

		// Disable
		canUse = false;

		// Start Cooldown
		cooldownProgress = abilityCooldown;
		StartCoroutine(Cooldown ());
	}

	// COOLDOWN
	public IEnumerator Cooldown()
	{
		while(true)
		{
			cooldownProgress -= Time.deltaTime;
			if(cooldownProgress <= 0)
			{
				// Stop
				break;
			}
			else
			{
				// Loop again
				yield return null;
			}
		}

		// Flag we can use
		canUse = true;

		yield return new WaitForEndOfFrame();
	}
}