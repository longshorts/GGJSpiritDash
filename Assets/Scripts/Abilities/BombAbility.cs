using UnityEngine;
using System.Collections;

public class BombAbility : MonoBehaviour
{
	[Header("Properties")]
	public GameObject BombPrefab;
	public float BombSpeed = 15.0f;
	public float CooldownDuration = 10.0f;
	
	[Header("Usability")]
	public bool HasAbility = false;
	public bool CanUseAbility = false;
	
	private Player_Control Player;
	
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

		// Throw the bomb
		GameObject Bomb = (GameObject)Instantiate(BombPrefab, transform.position + transform.forward, new Quaternion()) as GameObject;
		Bomb.name = "Thrown Bomb";
		Bomb.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * BombSpeed;

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