using UnityEngine;
using System.Collections;

public class Scene_transition : MonoBehaviour {

	public void transition() {
		if (Application.loadedLevelName == ("SplashScreen")) {

			Application.LoadLevel ("TheMainLevel");
		} else {
			Application.LoadLevel ("SplashScreen");
		}
	}

	public void end_game(bool p1) {
		if (p1) {
			Application.LoadLevel ("P1EndScreen");
		} else {
			Application.LoadLevel ("P2EndScreen");
		}
	}

	public void help() {

		Application.LoadLevel ("HelpScreen");
	}

	public void credits() {
		
		Application.LoadLevel ("Credits");
	}
}
