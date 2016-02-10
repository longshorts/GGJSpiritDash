using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
	public void GoToGame()
	{
		SceneManager.LoadScene ("Level_1");
	}

	public void GoToTitleScreen()
	{
		SceneManager.LoadScene ("TitleScreen");
	}

	public void GoToGameComplete()
	{
		SceneManager.LoadScene("GameComplete");
	}

	public void GoToHelp()
	{
		SceneManager.LoadScene ("HelpScreen");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void GoToCredits()
	{
		SceneManager.LoadScene ("Credits");
	}
}
