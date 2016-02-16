using UnityEngine;
using System.Collections;

public class BlockAbility : Ability
{
	[Header("Spell Specific")]
	public GameObject blockPrefab;
    public GameObject placementPrefab;
    public float abilityDuration = 5.0f;
	public float spawnDistance = 8.0f;
    private GameObject placementObj;
	private GameObject blockObj;
    public bool placementActive = false;
	
	void Start()
	{
		playerController = GetComponent<PlayerController> ();
	}
	
	public override void CastAbility ()
	{
		//Debug.Log ("Block Cast");
		
        // If the ability has already been cast
        if (placementActive)
        {
            // Cancel the block placement
            StopCoroutine(Placement());

            // The ability has now not been cast
            placementActive = false;

            // Check if we have a placement block
            if (placementObj)
            {
                // Destroy
                Destroy(placementObj);
                placementObj = null;
            }
        }
        // If the ability has not been cast
        else
        {
            // The ability has now been cast
            placementActive = true;

            // Start the block placement
            StartCoroutine(Placement());
        }
	}

    public void DropBlock()
    {
        if (placementActive)
        {
            //Debug.Log("Place Block");

            // Place the Block
            StartCoroutine(PlaceBlock());

            // Start Cooldown
            StartCooldown();

            // Cancel the block placement sequence
            StopCoroutine(Placement());

            // The ability has now not been cast
            placementActive = false;

            // Check if we have a placement block
            if (placementObj)
            {
                // Destroy
                Destroy(placementObj);
                placementObj = null;
            }
        }
        else
        {
            //Debug.Log("Block Placement not started");
        }
    }

    private IEnumerator Placement()
    {
        // Local variables for position calculations
        float angle = 0;
        float u;
        float v;

        // Get initial position of placement block
        Vector3 placementPosition = transform.position + (spawnDistance * playerController.directionVector2D * -1);
        
        // Create placement block
        placementObj = (GameObject)Instantiate(placementPrefab, placementPosition, new Quaternion(0, 1, 0, 0)) as GameObject;
        placementObj.name = "Placed placementBlockObj";

        // While the player has cast the ability
        while (placementActive)
        {
            // Get right stick position
            // XBOX CONTROLLER
            u = Input.GetAxis("HorizontalRightAnalogP" + playerController.playerNumber);
            v = Input.GetAxis("VerticalRightAnalogP" + playerController.playerNumber);

            // If the right stick is not in the centre
            if (u != 0 | v !=0)
            {
                // Get angle of right stick
                angle = Mathf.Atan(v / u);
                if (u < 0)
                {
                    if (v > 0)
                    {
                        angle = (180 * Mathf.Deg2Rad) + angle;
                    }
                    else
                    {
                        angle = (-180 * Mathf.Deg2Rad) + angle;
                    }
                }
            }

            // Get new position of placement block
            placementPosition.x = transform.position.x + (spawnDistance * Mathf.Cos(angle));
            placementPosition.y = 0;
            placementPosition.z = transform.position.z + (spawnDistance * Mathf.Sin(angle));

            // Set the placement block in its new position
            placementObj.transform.position = placementPosition;

            yield return null;
        }
    }


    private IEnumerator PlaceBlock()
	{
		Vector3 blockPosition;

        // Place at position of placement block
        blockPosition = placementObj.GetComponent<Transform>().position;
        blockObj = (GameObject)Instantiate(blockPrefab, blockPosition, new Quaternion(0, 1, 0, 0)) as GameObject;
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

