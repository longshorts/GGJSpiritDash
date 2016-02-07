using UnityEngine;
using System.Collections;

public class AbilityController : MonoBehaviour
{
	[Header("Properties")]
	public GameObject PlayerOne;
	public GameObject PlayerTwo;

	[Header("Abilties")]
	public FreezeAbility Freeze;
	public DashAbility Dash;
	public BlockAbility Block;
	public BombAbility Bomb;

	void Start()
	{
		PlayerOne = GameObject.Find ("PlayerOne");
		PlayerTwo = GameObject.Find ("PlayerTwo");

		// Add Abilities
		Freeze = GetComponent<FreezeAbility>();
		Dash = GetComponent<DashAbility>();
		Block = GetComponent<BlockAbility>();
		Bomb = GetComponent<BombAbility>();

		GiveAllAbilities ();
	}

	private void GiveAllAbilities()
	{
		Freeze.GiveAbility ();
		Dash.GiveAbility ();
		Block.GiveAbility ();
		Bomb.GiveAbility ();
	}
}