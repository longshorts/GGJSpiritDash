using UnityEngine;
using System.Collections;

public class BombAbility : Ability
{
	[Header("Spell Specific")]
	public GameObject BombPrefab;
	public float BombSpeed = 15.0f;
	public float bombDist;
	
	void Start()
	{
		playerController = GetComponent<PlayerController>();
	}
		
	public override void CastAbility ()
	{
		Debug.Log ("Cast Bomb");

		// Throw the bomb
		GameObject createdBomb = (GameObject)Instantiate(BombPrefab, transform.position + playerController.directionVector3D, Quaternion.Euler(new Vector3(90,0,0))) as GameObject;
		createdBomb.GetComponent<Bomb>().Initialise(playerController.directionVector3D, BombSpeed);

		// Tell physics engine to ignore collision between the bomb and the player that cast
		Physics.IgnoreCollision(createdBomb.GetComponent<Collider>(), GetComponent<Collider>());
		
		// Start Cooldown
		StartCooldown();
	}
}