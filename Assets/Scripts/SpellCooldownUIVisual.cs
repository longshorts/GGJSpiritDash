using UnityEngine;
using System.Collections;

public class SpellCooldownUIVisual : MonoBehaviour
{
	public RectTransform rectTransform;
	public int spriteHeight = 100;
	public AbilityController playerAbilities;
	public int stupidIncrease = 0;

	public int ID;

	public Vector3 startPosition;
	public Vector3 endPosition;


	void Start ()
	{
		startPosition = new Vector3(0, spriteHeight + stupidIncrease, 0);
		endPosition = new Vector3(0,stupidIncrease,0);

		rectTransform.localPosition = startPosition;
	}

	void Update ()
	{
		float value = 0;
		float top_value = 0;
		float bottom_value = 0;

		switch(ID)
		{
			case 0:
				value = playerAbilities.Freeze.cooldownProgress;
				top_value = playerAbilities.Freeze.abilityCooldown;
				break;
			case 1:
				value = playerAbilities.Dash.cooldownProgress;
				top_value = playerAbilities.Dash.abilityCooldown;
				break;
			case 2:
				value = playerAbilities.Block.cooldownProgress;
				top_value = playerAbilities.Block.abilityCooldown;
				break;
			case 3:
				value = playerAbilities.Bomb.cooldownProgress;
				top_value = playerAbilities.Bomb.abilityCooldown;
				break;
		}
			
		float range = top_value - bottom_value;
		float percent = (value - bottom_value) / range;

		rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, percent);
	}
}
