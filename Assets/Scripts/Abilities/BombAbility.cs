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
	
	private Player_Control Player;
	private Vector3 bombCalc;
	private Vector3 heading;
	
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
		bombCalc = transform.position + (Player.DirectionVector * 2);
		GameObject Bomb = (GameObject)Instantiate(BombPrefab, bombCalc, Quaternion.Euler(new Vector3(90,0,0))) as GameObject;
		Bomb.name = "Thrown Bomb";
		Bomb.GetComponent<Rigidbody>().velocity = Player.DirectionVector * BombSpeed;

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