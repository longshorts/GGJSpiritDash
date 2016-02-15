using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
	[Header("GUI")]
	public Image[] objectivesUI;
	public Text[] spellUI;

	[Header("Components")]
	public PlayerController player;
	public AbilityController abilityController;

	void Start ()
	{
		player = GetComponent<PlayerController>();
		abilityController = GetComponent<AbilityController>();
	}

	void Update ()
	{
		UpdateSpellGUI();
		UpdateObjectiveGUI();
	}

	private void UpdateSpellGUI()
	{
		if(spellUI.Length == 0 || spellUI.Length > 4)
			return;

		UpdateText(spellUI[0], abilityController.Freeze.cooldownProgress);
		UpdateText(spellUI[1], abilityController.Dash.cooldownProgress);
		UpdateText(spellUI[2], abilityController.Block.cooldownProgress);
		UpdateText(spellUI[3], abilityController.Bomb.cooldownProgress);
	}

	private void UpdateText(Text ui, float progress)
	{
		if(progress <= 0)
		{
			ui.text = "";
		}
		else
		{
			ui.text = ((int)progress).ToString();
		}
	}
	
	private void UpdateObjectiveGUI()
	{
		if(objectivesUI.Length == 0)
		{
			Debug.Log ("Warning! No Objectives!");
			return;
		}
		
		// Loop through and get the sprite
		for(int i = 0; i < objectivesUI.Length; i++)
		{
			objectivesUI[i].sprite = GetSprite(i);
		}
	}
	
	private Sprite GetSprite(int i)
	{
		if(player.Objectives.Count == 0)
		{
			Debug.Log("No Objectives");
			return null;
		}
		
		// Uncaptured
		if(player.Objectives[i].owner == null)
		{
			return player.Objectives[i].uncapturedSprite;
		}
		
		// If we own it then update to our colour version
		if(player.Objectives[i].owner.Equals(this.gameObject))
		{
			// We are the owner
			if(gameObject.tag == "Player1")
			{
				// We own it so set image as highlighted
				if(player.Objectives[i].captureProgress <= 5)
				{
					return player.Objectives[i].playerOneCapturedSprite;
				}
				else
				{
					return player.Objectives[i].uncapturedSprite;
				}
			}
			else
			{// We own it so set image as highlighted
				if(player.Objectives[i].captureProgress >= 147)
				{
					return player.Objectives[i].playerTwoCapturedSprite;
				}
				else
				{
					return player.Objectives[i].uncapturedSprite;
				}
			}
		}
		
		// We shouldnt make it here
		return player.Objectives[i].uncapturedSprite;
	}
}
