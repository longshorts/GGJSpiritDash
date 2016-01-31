using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game_Controller : MonoBehaviour
{
	[Header("Game Properties")]
	public int shrinesRequired = 5;		// shrines required to win
	public int maxShrines = 12;			// how many shrines in the level
	public bool GameRunning;			// whether the level is GameRunning

	[Header("Objects")]
	public Player PlayerOne;
	public Player PlayerTwo;
	public List<Shrine> LevelShrines;	// array of references to shrine scripts

	private Scene_transition sceneTransition;
	
	void Start ()
	{
		// initialize game state variables
		GameRunning = false;
		sceneTransition = GetComponent<Scene_transition> ();

		// Initialise Players
		PlayerOne = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>();
		PlayerTwo = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>();

		// Retrieve gameobjects for our shrines
		GameObject[] shrine = GameObject.FindGameObjectsWithTag("Shrine");

		// Compile list of shrine capture scripts
		LevelShrines = new List<Shrine>();
		foreach(GameObject obj in shrine)
		{
			LevelShrines.Add (obj.GetComponent<Shrine>());
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// if this is the first cycle of this match
		if (!GameRunning)
		{
			AllocateShrines();
			GameRunning = true;
		}

		/*// If the game has been complete then stop
		if(PlayerOne.Complete || PlayerTwo.Complete)
		{
			// Start Portal
			return;
		}*/

		// check whether the player controls all of their shrines
		CheckProgress ();

		if (PlayerOne.Victory) {
			sceneTransition.end_game (true);
		} else if (PlayerTwo.Victory){
			Debug.Log ("gotoendscreen");
			sceneTransition.end_game (false);
		}
	}

	void CheckProgress()
	{
		// Check how many shrines have been collected by each player
		CheckPlayerWin(ref PlayerOne, Shrine.CaptureState.PLAYERONE);
		CheckPlayerWin(ref PlayerTwo, Shrine.CaptureState.PLAYERTWO);

		if(PlayerOne.Complete)
		{
			Debug.Log ("Player One collected all shrines");
		}
		else if(PlayerTwo.Complete)
		{
			Debug.Log ("Player Two collected all shrines");
		}
		else if(PlayerOne.Complete && PlayerTwo.Complete)
		{
			Debug.Log ("Draw");
		}
	}

	private void CheckPlayerWin(ref Player player, Shrine.CaptureState state)
	{
		int Count = 0;
		
		foreach(Shrine shrine in player.Objectives)
		{
			if(shrine.ownerState == state)
			{
				Count++;
			}
		}
		
		if(Count >= shrinesRequired)
		{
			player.Complete = true;
		}
	}

	private void AllocateShrines()
	{
		// Shuffle the list
		LevelShrines.Shuffle();

		// Take the number of shrines we need
		Shrine[] PlayerOneShrines = LevelShrines.Take(shrinesRequired).ToArray();

		Debug.Log ("Player One--");
		foreach(Shrine shrine in PlayerOneShrines)
		{
			Debug.Log (shrine.name);
		}

		// Shuffle the list
		LevelShrines.Shuffle();

		// Take the number of shrines we need
		Shrine[] PlayerTwoShrines = LevelShrines.Take(shrinesRequired).ToArray();

		Debug.Log ("Player Two--");
		foreach(Shrine shrine in PlayerTwoShrines)
		{
			Debug.Log (shrine.name);
		}

		// Give the objectives to the player
		AllocateShrinesToPlayer(ref PlayerOne, PlayerOneShrines);
		AllocateShrinesToPlayer(ref PlayerTwo, PlayerTwoShrines);
	}
	
	private void AllocateShrinesToPlayer(ref Player player, Shrine[] PlayerShrines)
	{
		// Initialise list
		player.Objectives = new List<Shrine>();

		// Loop through each generated shrine
		for(int i = 0; i < PlayerShrines.Length; i++)
		{
			// Add to the objectives
			player.Objectives.Add(PlayerShrines[i]);
		}
	}
}