using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour
{
	public enum GameWinState { PLAYERONE, PLAYERTWO, NONE };

	[Header("Game Properties")]
	public int shrinesRequired = 4;							// Shrines required to win
	public GameWinState gameWinner = GameWinState.NONE;		// Who won
	public bool portalActivated = false;					// Flag for whether portal is open or not

	[Header("Objects")]
	public PlayerController playerOne;			// Access to player one
	public PlayerController playerTwo;			// Access to player one
	public List<GameObject> gameRespawns;		// List of spawn points
	public List<GameObject> blueShrines;		// List of blue player shrines
	public List<GameObject> redShrines;			// List of red player shrines
	public Portal portal;						// Access to portal object

	private SceneTransition sceneTransition;	// Scene switching
	
	void Start ()
	{
		// initialize game state variables
		sceneTransition = GetComponent<SceneTransition> ();

		// Initialise Players
		playerOne = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>();
		playerTwo = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>();

		// Retrieve access to blue and red shrines
		blueShrines = GameObject.FindGameObjectsWithTag("BlueShrine").ToList();
		redShrines = GameObject.FindGameObjectsWithTag("RedShrine").ToList();

		// Get a list of respawn points
		gameRespawns = GameObject.FindGameObjectsWithTag("Respawn").ToList();

		// Allocate shrines
		GenerateListOfShrines(ref playerOne, blueShrines, redShrines, 3, 1);
		GenerateListOfShrines(ref playerTwo, redShrines, blueShrines, 3, 1);

		// Flag we don't wanna destroy this when we move to the game win scene
		DontDestroyOnLoad(this);
	}

	void Update ()
	{
		// Check for the winner for the game
		if(portalActivated)
		{
			// Check who for a winner
			if(playerOne.isWinner)
			{
				Debug.Log ("Player One Wins");
				gameWinner = GameWinState.PLAYERONE;
				sceneTransition.GoToGameComplete();
			}
			else if(playerTwo.isWinner)
			{
				Debug.Log ("Player Two Wins");
				gameWinner = GameWinState.PLAYERTWO;
				sceneTransition.GoToGameComplete();
			}
		}

		// Check for progression
		CheckShrineProgress ();
	}

	void CheckShrineProgress()
	{
		bool resultPlayer1, resultPlayer2;

		// Check how many shrines have been collected by each player
		resultPlayer1 = CheckPlayerWin(ref playerOne, Shrine.CaptureState.PLAYERONE);
		resultPlayer2 = CheckPlayerWin(ref playerTwo, Shrine.CaptureState.PLAYERTWO);

		// Check if either player has captured their objectives
		if(resultPlayer1 || resultPlayer2)
		{
			// Activate the portal if it hasnt already been
			if(!portalActivated)
			{
				portal.Activate();
				portalActivated = true;
			}
		}
		else
		{
			// If both players havent captured their objectives
			if(!resultPlayer1 && !resultPlayer2)
			{
				if(portalActivated)
				{
					// Deactivate portal
					portal.Deactivate();
					portalActivated = false;
				}
			}
		}
	}

	public void GetRespawnLocation(GameObject player)
	{
		// Update player position to a random respawn point
		player.transform.position = gameRespawns[Random.Range(0, gameRespawns.Count - 1)].transform.position;
	}

	private bool CheckPlayerWin(ref PlayerController player, Shrine.CaptureState state)
	{
		int capturedCount = 0;

		// Loop through each shrine and check their owner
		foreach(Shrine shrine in player.Objectives)
		{
			if(shrine.IsCaptured() && shrine.ownerState.Equals(state))
			{
				capturedCount++;
			}
		}

		// Check if we have won
		if(capturedCount.Equals(shrinesRequired))
		{
			// Flag the player has captured their objectives
			player.isComplete = true;

			return true;
		}
		else
		{
			// Deactivate player winning
			player.isComplete = false;
			player.isWinner = false;
		}

		return false;
	}

	private void GenerateListOfShrines(ref PlayerController player, List<GameObject> listOne, List<GameObject> listTwo, int NoFromOne, int NoFromTwo)
	{
		// Randomise each list
		listOne.Shuffle();
		listTwo.Shuffle();

		// Take x amount from each list
		List<GameObject> objectivesList = new List<GameObject>();
		objectivesList.AddRange( listOne.Take(NoFromOne) );
		objectivesList.AddRange( listTwo.Take(NoFromTwo) );
		
		// Extract the shrine component from each shrine in the list and add to the objectives
		player.Objectives = new List<Shrine>();
		foreach(GameObject obj in objectivesList)
		{
			player.Objectives.Add(obj.GetComponent<Shrine>());
		}
	}
}