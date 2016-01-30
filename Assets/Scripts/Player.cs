using System;
using System.Collections.Generic;

public class Player
{
	public List<Shrine> Objectives;
	public bool Complete;
	public float SpiritPower;
	public bool Victory;

	// GUI HERE

	public Player ()
	{
		SpiritPower = 0.0f;
		Objectives = new List<Shrine>();
		Complete = false;
	}

	public void UpdateGUI()
	{
		//
	}
}