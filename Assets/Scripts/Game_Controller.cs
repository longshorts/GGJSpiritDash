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
	
	void Start ()
	{
		// initialize game state variables
		GameRunning = false;

		// Initialise Player One
		PlayerOne = new Player();
		PlayerOne.Objectives = new List<Shrine>();
		PlayerOne.Complete = false;

		// Initialise Player Two
		PlayerTwo = new Player();
		PlayerTwo.Objectives = new List<Shrine>();
		PlayerTwo.Complete = false;

		// Retrieve gameobjects for our shrines
		GameObject[] shrine = GameObject.FindGameObjectsWithTag("Shrine");

		// Compile list of shrine capture scripts
		LevelShrines = new List<Shrine>();
		foreach(GameObject obj in shrine)
		{
			Shrine temp = new Shrine();
			temp.Info = obj.GetComponent<Shrine_Capture>();
			temp.Obtained = false;
			LevelShrines.Add(temp);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// if this is the first cycle of this match
		if (!GameRunning)
		{
			AllocateShrines();	// randomly assign the players objectives
			GameRunning = true;		// prevents this occurring again during this match
		}

		// If the game has been complete then stop
		if(PlayerOne.Complete || PlayerTwo.Complete)
		{
			// We will have UI here
			return;
		}

		CheckProgress ();			// check which shrines the players control
		CheckShrinesCollected ();	// check whether the player controls all of their shrines
	}

	void CheckProgress()
	{
		// Check each shrine in player one objectives
		for(int i = 0; i < PlayerOne.Objectives.Count; i++)
		{
			// Set its obtained if its owned by player one
			PlayerOne.Objectives.ToArray()[i].Obtained = PlayerOne.Objectives[i].Info.CheckOwner(Shrine_Capture.CaptureState.PLAYERONE);
		}
		
		// Check each shrine in player two objectives
		for(int i = 0; i < PlayerTwo.Objectives.Count; i++)
		{
			// Set its obtained if its owned by player two
			PlayerTwo.Objectives.ToArray()[i].Obtained = PlayerTwo.Objectives[i].Info.CheckOwner(Shrine_Capture.CaptureState.PLAYERTWO);
		}
	}

	void CheckShrinesCollected()
	{
		// Check how many shrines have been collected by each player
		CheckPlayerWin(ref PlayerOne);
		CheckPlayerWin(ref PlayerTwo);

		if(PlayerOne.Complete && PlayerTwo.Complete)
		{
			Debug.Log ("Draw");
		}
		else if(PlayerOne.Complete)
		{
			Debug.Log ("Player One Wins");
		}
		else if(PlayerTwo.Complete)
		{
			Debug.Log ("Player Two Wins!");
		}
	}

	private void CheckPlayerWin(ref Player player)
	{
		int Count = 0;
		
		foreach(Shrine shrine in player.Objectives)
		{
			if(shrine.Obtained)
			{
				Count++;
			}
		}
		
		if(Count == shrinesRequired)
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
			Debug.Log ("ID : " + shrine.Info.shrineID);
		}

		// Shuffle the list
		LevelShrines.Shuffle();

		// Take the number of shrines we need
		Shrine[] PlayerTwoShrines = LevelShrines.Take(shrinesRequired).ToArray();
		
		Debug.Log ("Player Two--");
		foreach(Shrine shrine in PlayerTwoShrines)
		{
			Debug.Log ("ID : " + shrine.Info.shrineID);
		}
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