using UnityEngine;
using System.Collections;

public class AbilityController : MonoBehaviour
{
	[Header("Properties")]
	public string PlayerOneName = "PlayerCharacter1";
	public string PlayerTwoName = "PlayerCharacter2";

	public GameObject PlayerOne;
	public GameObject PlayerTwo;

	[Header("Abilties")]
	public FreezeAbility Freeze;
	public DashAbility Dash;
	public BlockAbility Block;
	public BombAbility Bomb;

	void Start()
	{
		PlayerOne = GameObject.Find (PlayerOneName);
		PlayerTwo = GameObject.Find (PlayerTwoName);

		// Add Abilities
		Freeze = this.gameObject.GetComponent<FreezeAbility>();
		Dash = this.gameObject.GetComponent<DashAbility>();
		Block = this.gameObject.GetComponent<BlockAbility>();
		Bomb = this.gameObject.GetComponent<BombAbility>();
	}
}