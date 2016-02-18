using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour
{
	[Header("Game Properties")]
	public int shrinesRequired = 4;							// Shrines required to win
	public int roundCount = 3;								// Number of rounds
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
		// Initialize game state variables
		sceneTransition = GetComponent<SceneTransition> ();

		// Initialise Players
		playerOne = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>();
		playerTwo = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>();

		// Get a list of respawn points
		gameRespawns = GameObject.FindGameObjectsWithTag("Respawn").ToList();

		// Initialise round system
		InitialiseRounds();

		// Get a list of shrines and split them between the two players
		InitialiseShrines();
	}

	void Update ()
	{
		// Check for the winner for the game
		if(portalActivated)
		{
			// Check who for a winner
			if(playerOne.isWinner)
			{
				Debug.Log ("Player One Wins the Round");
				PlayerPrefs.SetInt("PlayerOneWins", PlayerPrefs.GetInt("PlayerOneWins")+1);
				ProcessRoundComplete();
			}
			else if(playerTwo.isWinner)
			{
				Debug.Log ("Player Two Wins the Round");
				PlayerPrefs.SetInt("PlayerTwoWins", PlayerPrefs.GetInt("PlayerTwoWins")+1);
				ProcessRoundComplete();
			}
		}

		// Check for progression
		CheckShrineProgress ();
	}

	private void InitialiseRounds()
	{
		// Check what round we are on
		if(PlayerPrefs.HasKey("RoundNo"))
		{
			// Reset round count
			if(PlayerPrefs.GetInt("RoundNo") >= roundCount)
			{
				PlayerPrefs.SetInt("RoundNo", 1);
				PlayerPrefs.SetInt("PlayerOneWins", 0);
				PlayerPrefs.SetInt("PlayerTwoWins", 0);

				Debug.Log("Reset rounds!");
			}
			else
			{
				// Increase round count
				PlayerPrefs.SetInt("RoundNo", PlayerPrefs.GetInt("RoundNo") + 1);

				Debug.Log("Next Round: " + PlayerPrefs.GetInt("RoundNo"));
			}
		}
		else
		{
			// Initialise for first time
			PlayerPrefs.SetInt("RoundNo", 1);
			PlayerPrefs.SetInt("PlayerOneWins", 0);
			PlayerPrefs.SetInt("PlayerTwoWins", 0);

			PlayerPrefs.GetInt("First Time Use!");
		}
	}

	private void InitialiseShrines()
	{
		// Retrieve access to blue and red shrines
		blueShrines = GameObject.FindGameObjectsWithTag("BlueShrine").ToList();
		redShrines = GameObject.FindGameObjectsWithTag("RedShrine").ToList();

		// Make sure they are sorted by name
		blueShrines = blueShrines.OrderBy(shrine => shrine.name).ToList();
		redShrines = redShrines.OrderBy(shrine => shrine.name).ToList();

		// Add static shrines to each player
		playerOne.Objectives.Add(blueShrines[0].GetComponent<Shrine>());
		playerTwo.Objectives.Add(redShrines[0].GetComponent<Shrine>());
		blueShrines.Remove(blueShrines[0]);
		redShrines.Remove(redShrines[0]);

		// Allocate shrines
		GenerateListOfShrines(ref playerOne, blueShrines, redShrines, 2, 1);
		GenerateListOfShrines(ref playerTwo, redShrines, blueShrines, 2, 1);
	}

	private void CheckShrineProgress()
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
	
	private void ProcessRoundComplete()
	{
		// Check if we have reached last round
		if(PlayerPrefs.GetInt("RoundNo") == roundCount || PlayerPrefs.GetInt("PlayerOneWins") >= 2 || PlayerPrefs.GetInt("PlayerTwoWins") >= 2)
		{
			// Go to game complete
			sceneTransition.GoToGameComplete();
		}
		else
		{
			Application.LoadLevel(Application.loadedLevel);
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
		// Create a list to store the objectives
		List<GameObject> objectivesList = new List<GameObject>();

		// Shuffle both lists
		listOne.Shuffle();
		listTwo.Shuffle();

		// Take x amount from each list
		objectivesList.AddRange( listOne.Take(NoFromOne) );
		objectivesList.AddRange( listTwo.Take(NoFromTwo) );

		listOne.RemoveRange(0, NoFromOne);
		listTwo.RemoveRange(0, NoFromTwo);
		
		// Extract the shrine component from each shrine in the list and add to the objectives
		foreach(GameObject obj in objectivesList)
		{
			player.Objectives.Add(obj.GetComponent<Shrine>());
		}
	}
}