using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerScript : MonoBehaviour {

	float time;
	string mins;
	string seconds;

	public Text timerText;

	// Use this for initialization
	void Start () {
		reset ();
		timerText = GameObject.Find ("TimerText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		mins = Mathf.Floor (time / 60).ToString("00");
		seconds = (time % 60).ToString ("00");

		timerText.text = "" + mins + ":" + seconds;
	}

	public void reset(){
		mins = seconds = "00";
		time = 0;
	}
}
