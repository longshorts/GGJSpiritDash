using UnityEngine;
using System.Collections;

public class BlockAbility : MonoBehaviour
{
	[Header("Properties")]
	public GameObject BlockPrefab;
	public float CooldownDuration = 10.0f;
	public float BlockDuration = 5.0f;
	public float blockDist = 6.0f;

	[Header("Usability")]
	public bool HasAbility = true;
	public bool CanUseAbility = true;
	
	private PlayerController playerController;
	private GameObject Block;
	private Transform blockLocation;
	private Vector3 blockCalc;

	
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

		// Flag we are now dashing and cannot do so again yet
		CanUseAbility = false;

		// Start Cooldown
		StartCoroutine (Cooldown ());

		// Place Block
		StartCoroutine (BlockPlacement ());
	}
	
	private IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(CooldownDuration);
		CanUseAbility = true;
	}

	private IEnumerator BlockPlacement()
	{
		// Place behind us
		blockCalc = transform.position + (blockDist * playerController.LocalDirectionVector * -1);
		Block = (GameObject)Instantiate (BlockPrefab, blockCalc, new Quaternion (0, 1, 0, 0)) as GameObject;
		Block.name = "Placed Block"; 

		// Wait for x seconds
		yield return new WaitForSeconds (BlockDuration);

		// If we have a block still, destroy it
		if(Block)
		{
			Destroy (Block);
            Block = null;
        }
	}
}

