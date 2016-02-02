using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
	public void PlayGame()
	{
		if (Application.loadedLevelName == ("SplashScreen"))
		{
			Application.LoadLevel ("TheMainLevel");
		}
		else
		{
			Application.LoadLevel ("SplashScreen");
		}
	}

	public void EndGame(bool p1)
	{
		if (p1)
		{
			Application.LoadLevel ("P1EndScreen");
		}
		else
		{
			Application.LoadLevel ("P2EndScreen");
		}
	}

	public void HelpScreen()
	{
		Application.LoadLevel ("HelpScreen");
	}

	public void Credits()
	{
		Application.LoadLevel ("Credits");
	}
}
