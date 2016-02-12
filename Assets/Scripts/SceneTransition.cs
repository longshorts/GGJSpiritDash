using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour
{	
	public void GoToTitleScreen()
	{
		Application.LoadLevel ("TitleScreen");
	}
	
	public void GoToHelp()
	{
		Application.LoadLevel ("HelpScreen");
	}

	public void GoToCredits()
	{
		Application.LoadLevel ("Credits");
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}

	public void GoToGame()
	{
		Application.LoadLevel ("Level_1");

		// Clear all prefs if we have them
		if(PlayerPrefs.HasKey("RoundNo"))
		{
			PlayerPrefs.DeleteAll();
		}
	}
	public void GoToGameComplete()
	{
		Application.LoadLevel("GameComplete");
	}
}
