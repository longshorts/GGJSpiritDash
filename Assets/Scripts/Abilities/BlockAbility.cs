using UnityEngine;
using System.Collections;
using UnityEditor;

public class BlockAbility : MonoBehaviour
{
	[Header("Properties")]
	public GameObject BlockPrefab;
	public float CooldownDuration = 10.0f;
	public float BlockDuration = 5.0f;
	
	[Header("Usability")]
	public bool HasAbility = false;
	public bool CanUseAbility = false;
	
	private Player_Control Player;
	private GameObject Block;
	
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
		StartCoroutine(Cooldown());

		// Place Block
		StartCoroutine(BlockPlacement());
	}
	
	private IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(CooldownDuration);
		CanUseAbility = true;
	}

	private IEnumerator BlockPlacement()
	{
		Block = (GameObject)Instantiate(BlockPrefab, transform.position, new Quaternion()) as GameObject;
		Block.name = "Placed Block";

		// Wait for x seconds
		yield return new WaitForSeconds(BlockDuration);

		// If we have a block still, destroy it
		if(Block)
		{
			Destroy (Block);
            Block = null;
        }
	}
}

