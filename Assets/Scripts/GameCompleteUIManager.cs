using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameCompleteUIManager : MonoBehaviour
{
	public GameController gameController;
	public Text uiText;

	void Start ()
	{
		GameObject controller = GameObject.Find ("GameController");
		if(controller)
		{
			gameController = controller.GetComponent<GameController>();
		}
		else
		{
			return;
		}

		// Set the text
		if(gameController.gameWinner == GameController.GameWinState.PLAYERONE)
		{
			uiText.text = "GAME OVER\n\nPLAYER ONE WINS";
		}
		else if(gameController.gameWinner == GameController.GameWinState.PLAYERTWO)
		{
			uiText.text = "GAME OVER\n\nPLAYER TWO WINS";
		}
		else
		{
			// WE SHOULDNT REACH THIS
		}

		// Destroy the game controller
		Destroy (gameController.gameObject);
	}
}
