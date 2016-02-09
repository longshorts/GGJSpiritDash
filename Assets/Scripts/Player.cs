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

	void Start()
	{
		SpiritPower = 0.0f;
		Objectives = new List<Shrine>();
		Complete = false;
		Victory = false;
	}
}