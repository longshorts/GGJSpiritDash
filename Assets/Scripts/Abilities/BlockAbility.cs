using UnityEngine;
using System.Collections;

public class BlockAbility : Ability
{
	[Header("Spell Specific")]
	public GameObject blockPrefab;
	public float abilityDuration = 5.0f;
	public float spawnDistance = 6.0f;
	private GameObject blockObj;
	private Transform blockLocation;
	
	void Start()
	{
		playerController = GetComponent<PlayerController> ();
	}
	
	public override void CastAbility ()
	{
		Debug.Log ("Cast Block");
		
		// Place blockObj
		StartCoroutine (PlaceBlock ());
		
		// Start Cooldown
		StartCooldown();
	}


	private IEnumerator PlaceBlock()
	{
		Vector3 blockPosition;

		// Place behind us
		blockPosition = transform.position + (spawnDistance * playerController.directionVector2D * -1);
		blockObj = (GameObject)Instantiate (blockPrefab, blockPosition, new Quaternion (0, 1, 0, 0)) as GameObject;
		blockObj.name = "Placed blockObj"; 

		// Wait for x seconds
		yield return new WaitForSeconds (abilityDuration);

		// Check if we have a placed block
		if(blockObj)
		{
			// Destroy
			Destroy (blockObj);
            blockObj = null;
        }
	}
}

