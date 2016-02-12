using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameCompleteUIManager : MonoBehaviour
{
	public GameController gameController;
	public Text uiText;

	void Start ()
	{
		// Check for the winner
		if(PlayerPrefs.GetInt("PlayerOneWins") >= 2)
		{
			uiText.text = "GAME OVER\n\nPLAYER ONE WINS";
		}
		else if(PlayerPrefs.GetInt("PlayerTwoWins") >= 2)
		{
			uiText.text = "GAME OVER\n\nPLAYER TWO WINS";
		}
	}
}
