using UnityEngine;
using System.Collections;

public class AbilityController : MonoBehaviour
{
	[Header("Properties")]
	public string PlayerOneTag = "Player1";
	public string PlayerTwoTag = "Player2";

	public GameObject PlayerOne;
	public GameObject PlayerTwo;

	[Header("Abilties")]
	public FreezeAbility Freeze;
	public DashAbility Dash;
	public BlockAbility Block;
	public BombAbility Bomb;

	void Start()
	{
		PlayerOne = GameObject.Find (PlayerOneTag);
		PlayerTwo = GameObject.Find (PlayerTwoTag);

		// Add Abilities
		Freeze = this.gameObject.GetComponent<FreezeAbility>();
		Dash = this.gameObject.GetComponent<DashAbility>();
		Block = this.gameObject.GetComponent<BlockAbility>();
		Bomb = this.gameObject.GetComponent<BombAbility>();
	}
}