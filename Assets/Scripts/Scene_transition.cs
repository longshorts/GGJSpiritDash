using UnityEngine;
using System.Collections;

public class Scene_transition : MonoBehaviour {

	public void transition() {
		Application.LoadLevel (Random.Range (1, 10));
	}
}
