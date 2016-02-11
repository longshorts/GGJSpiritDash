using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour
{
	public enum GameWinState { PLAYERONE, PLAYERTWO, DRAW };

	[Header("Game Properties")]
	public int shrinesRequired = 4;		// Shrines required to win
	public int maxShrines = 12;			// How many shrines in the level
	public bool isRunning;				// Flag for checking if the game is running

	[Header("Objects")]
	public PlayerController playerOne;
	public PlayerController playerTwo;
	public List<Shrine> gameShrines;			// Array of references to shrine scripts
	public List<GameObject> gameRespawns;		// List of spawn points
	public Portal portal;						// Access to portal object
	public GameWinState gameWinner;				// Who won

	private SceneTransition sceneTransition;
	
	void Start ()
	{
		// initialize game state variables
		isRunning = false;
		sceneTransition = GetComponent<SceneTransition> ();

		// Initialise Players
		playerOne = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>();
		playerTwo = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>();

		// Retrieve gameobjects for our shrines
		GameObject[] shrine = GameObject.FindGameObjectsWithTag("Shrine");

		// Compile list of shrine capture scripts
		gameShrines = new List<Shrine>();
		foreach(GameObject obj in shrine)
		{
			gameShrines.Add (obj.GetComponent<Shrine>());
		}

		// Get a list of respawn points
		gameRespawns = GameObject.FindGameObjectsWithTag("Respawn").ToList();

		// Allocate shrines
		AllocateShrines();

		// Flag we are playing
		isRunning = true;

		// Flag we don't wanna destroy this
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// if this is the first cycle of this match
		if (!isRunning)
		{
			return;
		}

		// Check if either player has won
		if(playerOne.isWinner)
		{
			gameWinner = GameWinState.PLAYERONE;
			isRunning = false;
			sceneTransition.GoToGameComplete ();
		}
		else if(playerTwo.isWinner)
		{
			gameWinner = GameWinState.PLAYERTWO;
			isRunning = false;
			sceneTransition.GoToGameComplete();
		}
		else
		{
			// check whether the player controls all of their shrines
			CheckProgress ();
		}
	}

	void CheckProgress()
	{
		// Check how many shrines have been collected by each player
		CheckPlayerWin(ref playerOne, Shrine.CaptureState.PLAYERONE);
		CheckPlayerWin(ref playerTwo, Shrine.CaptureState.PLAYERTWO);

		if(playerOne.isComplete || playerTwo.isComplete)
		{
			portal.Activate();
		}
	}

	public void GetRespawnLocation(GameObject player)
	{
		// Sort respawns by closest
		gameRespawns = gameRespawns.OrderBy(tile => Vector3.Distance(player.transform.position, tile.transform.position)).ToList();

		// Update player position
		player.transform.position = gameRespawns[0].transform.position;
	}

	private void CheckPlayerWin(ref PlayerController player, Shrine.CaptureState state)
	{
		int Count = 0;
		
		foreach(Shrine shrine in player.Objectives)
		{
			if(shrine.ownerState == state)
			{
				if(shrine.IsCaptured())
					Count++;
			}
		}
		
		if(Count >= shrinesRequired)
		{
			player.isComplete = true;
		}
	}

	private void AllocateShrines()
	{
		// Shuffle the list
		gameShrines.Shuffle();

		// Take the number of shrines we need
		Shrine[] playerOneShrines = gameShrines.Take(shrinesRequired).ToArray();

		// Shuffle the list
		gameShrines.Shuffle();

		// Take the number of shrines we need
		Shrine[] playerTwoShrines = gameShrines.Take(shrinesRequired).ToArray();

		// Give the objectives to the player
		AllocateShrinesToPlayer(ref playerOne, playerOneShrines);
		AllocateShrinesToPlayer(ref playerTwo, playerTwoShrines);
	}
	
	private void AllocateShrinesToPlayer(ref PlayerController player, Shrine[] PlayerShrines)
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