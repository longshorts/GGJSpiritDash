using UnityEngine;
using System.Collections;

public class WallCollision : MonoBehaviour
{
	public GameObject player;
	public Transform spawnPoint;
	public float TrappedDuration = 3.0f;
	public bool trapped;
	public bool stillTrapped;

//	enum direction {up, down, left, right};

	// Use this for initialization
	void Start ()
	{
		trapped = false;
		stillTrapped = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	void OnColliderStay(Collider other)
	{
		if (!trapped)
		{
			StartCoroutine (Trapped ());
			trapped = true;
		}

		if (stillTrapped)
		{
			player = other.gameObject;
			if (player.tag == ("Player1"))
			{
				spawnPoint = GameObject.Find ("Player1spawn").GetComponent<Transform>();
			}
			else if (player.tag == ("Player2"))
			{
				spawnPoint = GameObject.Find ("Player1spawn").GetComponent<Transform>();
			}
			player.transform.position = Vector3.Lerp(player.transform.position, spawnPoint.position, 100);
		//	player.transform.position = spawnPoint.position;
		}
	}

	void OnColliderExit(Collider other)
	{
		trapped = false;
		stillTrapped = false;
	}

	private IEnumerator Trapped()
	{
		yield return new WaitForSeconds(TrappedDuration);
		stillTrapped = true;
	}
}