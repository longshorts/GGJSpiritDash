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
		for(int i = 0; i < 5; i++)
		{
			ObjectivesUI[i].sprite = GetSprite(i);
		}
	}

	private Sprite GetSprite(int i)
	{
		if(Objectives.Count == 0)
			return null;

		// We own it so set image as highlighted
		return Objectives[i].Uncaptured;
	}
}