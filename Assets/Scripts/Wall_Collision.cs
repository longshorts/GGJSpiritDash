using UnityEngine;
using System.Collections;

public class Wall_Collision : MonoBehaviour
{
	private RaycastHit hit;
	private Ray ray;

	enum direction {up, down, left, right};

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "wall")
		{
		}
	}

	void lookAtWall(direction x, Collider other)
	{
		ray.origin = transform.position;
		//ray.direction = 
	}
}