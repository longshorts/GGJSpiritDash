using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameCompleteUIManager : MonoBehaviour
{
	public GameController gameController;
	public Text uiText;

	void Start ()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();

		if(!gameController)
			return;

		// Set the text
		if(gameController.gameWinner == GameController.GameWinState.PLAYERONE)
		{
			uiText.text = "PLAYER ONE WINS";
		}
		else if(gameController.gameWinner == GameController.GameWinState.PLAYERTWO)
		{
			uiText.text = "PLAYER TWO WINS";
		}
		else
		{
			// WE SHOULDNT REACH THIS
		}

		// Destroy the game controller
		Destroy (gameController.gameObject);
	}
}
