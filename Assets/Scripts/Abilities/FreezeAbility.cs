using UnityEngine;
using System.Collections;

public class FreezeAbility : Ability
{
	[Header("Spell Specific")]
	public float freezeDuration = 5.0f;
	public float FreezeRadius = 5.0f;

	void Start()
	{
		// Get access to the opposing player
		if(gameObject.tag.Equals("Player1"))
		{
			playerController = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>();
		}
		else if(gameObject.tag.Equals("Player2"))
		{
			playerController = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>();
		}
	}

	public override void CastAbility ()
	{
		Debug.Log ("Cast Freeze");

		// Start Cooldown
		StartCooldown();

		// Attack Player
		StartCoroutine(FreezePlayer(playerController.gameObject));
	}

	private IEnumerator FreezePlayer(GameObject Target)
	{
		// Freeze the Player
		playerController.Freeze(true);

		// Wait for unfreeze
		yield return new WaitForSeconds(freezeDuration);

		// Unfreeze player
		playerController.Freeze (false);
	}
}