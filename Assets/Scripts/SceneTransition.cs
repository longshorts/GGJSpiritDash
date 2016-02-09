using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
	public void GoToGame()
	{
		Application.LoadLevel ("Level_1");
	}

	public void GoToTitleScreen()
	{
		Application.LoadLevel ("TitleScreen");
	}

	public void GoToGameComplete()
	{
		Application.LoadLevel("GameComplete");
	}

	public void GoToHelp()
	{
		Application.LoadLevel ("HelpScreen");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void GoToCredits()
	{
		Application.LoadLevel ("Credits");
	}
}
