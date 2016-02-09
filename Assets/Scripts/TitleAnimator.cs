using UnityEngine;
using System.Collections;

public class TitleAnimator : MonoBehaviour
{
	public float animationSpeed = 2;
	public float moveMultiplier = 2;
	private Vector3 startPos;
	public float moveFrame;
	
	void Start ()
	{
		startPos = transform.position;
		moveFrame = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		moveFrame += Time.deltaTime * animationSpeed;

		transform.position = startPos + new Vector3(0, Mathf.Sin(moveFrame) * moveMultiplier, 0);
	}
}
