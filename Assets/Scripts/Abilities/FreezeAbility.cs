using UnityEngine;
using System.Collections;

public class FreezeAbility : Ability
{
	[Header("Spell Specific")]
	public float freezeDuration = 5.0f;
	public float FreezeRadius = 20.0f;

    private bool opponentInRange = false;

    
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
        gameObject.GetComponent<SphereCollider>().radius = FreezeRadius;
	}

	public override void CastAbility ()
	{
        if (opponentInRange)
        {
            Debug.Log("Cast Freeze");

            // Start Cooldown
            StartCooldown();

            // Attack Player
            StartCoroutine(FreezePlayer(playerController.gameObject));
        }
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

    private void OnTriggerEnter(Collider other)
    {
        // if the other player is in range, allow the ability to be performed
        if (!other.isTrigger & other.gameObject.tag.Equals(playerController.gameObject.tag))
        {
            opponentInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if the other player is not in range, do not allow the ability to be performed
        if (!other.isTrigger & other.gameObject.tag.Equals(playerController.gameObject.tag))
        {
            opponentInRange = false;
        }
    }
}