using UnityEngine;
using System.Collections;

public class FreezeAbility : MonoBehaviour
{
	[Header("Properties")]
	public float FreezeDuration = 5.0f;
	public float CooldownDuration = 10.0f;

	[Header("Usability")]
	public bool HasAbility = false;
	public bool CanUseAbility = false;

	void Start()
	{
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

	public void UseAbility (GameObject obj)
	{
		if(!HasAbility)
		{
			Debug.Log ("Cannot use ability");
			return;
		}

		if(!CanUseAbility)
		{
			Debug.Log ("On Cooldown!");
			return;
		}
	
		// Attack Player
		StartCoroutine(FreezePlayer(obj));
	}

	private IEnumerator FreezePlayer(GameObject Target)
	{
		CanUseAbility = false;

		// Freeze the Player
		Debug.Log ("Freeze motherfucker!");
		Target.GetComponent<PlayerController>().Freeze(true);

		// Wait for unfreeze
		yield return new WaitForSeconds(FreezeDuration);

		// Unfreeze player
		Target.GetComponent<PlayerController>().Freeze (false);
		Debug.Log ("Get your head into the game!");

		// Start Cooldown
		StartCoroutine(Cooldown());
	}

	private IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(CooldownDuration);
		CanUseAbility = true;
	}
}