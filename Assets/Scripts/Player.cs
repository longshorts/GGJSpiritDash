using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public List<Shrine> Objectives;
	public bool Complete;
	public float SpiritPower;
	public bool Victory;

	public Image[] ObjectivesUI;

	void Start()
	{
		SpiritPower = 0.0f;
		Objectives = new List<Shrine>();
		Complete = false;
		Victory = false;

		UpdateGUI ();
	}

	void Update()
	{
		UpdateGUI();
	}

	public void UpdateGUI()
	{
		if(ObjectivesUI.Length == 0)
			return;

		// Loop through and get the sprite
		for(int i = 0; i < ObjectivesUI.Length; i++)
		{
			ObjectivesUI[i].sprite = GetSprite(i);
		}
	}

	private Sprite GetSprite(int i)
	{
		if(Objectives.Count == 0)
			return null;

		// Uncaptured
		if(Objectives[i].owner == null)
		{
			return Objectives[i].Uncaptured;
		}

		// If we own it then update to our colour version
		if(Objectives[i].owner.Equals(this.gameObject))
		{
			// We are the owner
			if(gameObject.tag == "Player1")
			{
				// We own it so set image as highlighted
				if(Objectives[i].captureProgress <= 5)
				{
					return Objectives[i].PlayerOneCaptured;
				}
				else
				{
					return Objectives[i].Uncaptured;
				}
			}
			else
			{// We own it so set image as highlighted
				if(Objectives[i].captureProgress >= 147)
				{
					return Objectives[i].PlayerTwoCaptured;
				}
				else
				{
					return Objectives[i].Uncaptured;
				}
			}
		}

		// We shouldnt make it here
		return Objectives[i].Uncaptured;
	}
}