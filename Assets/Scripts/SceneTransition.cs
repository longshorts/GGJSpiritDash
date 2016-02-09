using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
	public void PlayGame()
	{
		if (Application.loadedLevelName == ("SplashScreen"))
		{
			Application.LoadLevel ("Level_1");
		}
		else
		{
			Application.LoadLevel ("SplashScreen");
		}
	}

	public void EndGame()
	{
		Application.LoadLevel("GameComplete");
	}

	public void HelpScreen()
	{
		Application.LoadLevel ("HelpScreen");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void Credits()
	{
		Application.LoadLevel ("Credits");
	}
}
