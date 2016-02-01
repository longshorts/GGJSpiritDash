using UnityEngine;
using System.Collections;

public class BombAbility : MonoBehaviour
{
	[Header("Properties")]
	public GameObject BombPrefab;
	public float BombSpeed = 15.0f;
	public float CooldownDuration = 10.0f;
	public float bombDist;
	
	[Header("Usability")]
	public bool HasAbility = true;
	public bool CanUseAbility = true;

	[Header("Access")]
	public PlayerController playerController;
	
	void Start()
	{
		playerController = gameObject.GetComponent<PlayerController>();
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

		// Throw the bomb
		GameObject CreatedBomb = (GameObject)Instantiate(BombPrefab, transform.position + (playerController.DirectionVector * 3), Quaternion.Euler(new Vector3(90,0,0))) as GameObject;
		CreatedBomb.GetComponent<Bomb>().Initialise(playerController.DirectionVector, BombSpeed);

		Debug.Log ("Fire in the hole!");

		// Start Cooldown
		StartCoroutine(Cooldown());
	}
	
	private IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(CooldownDuration);
		CanUseAbility = true;
	}
}