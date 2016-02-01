using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
	public GameObject OurSplash;
	public GameObject TheirSplash;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(Pause());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator Pause()
	{
		OurSplash.SetActive(false);
		TheirSplash.SetActive(true);

		yield return new WaitForSeconds(5);

		OurSplash.SetActive(true);
		TheirSplash.SetActive(false);
	}
}
