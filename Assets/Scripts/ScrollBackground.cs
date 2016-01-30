using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollBackground : MonoBehaviour
{
	public float moveSpeed = 0.5f;

	private Image BackgroundImg;

	private float StartPos;
	private float MinX;

	// Use this for initialization
	void Start ()
	{
		BackgroundImg = this.GetComponent<Image>();

		StartPos = BackgroundImg.transform.position.x;
		MinX = StartPos - BackgroundImg.sprite.rect.width / 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 Pos = BackgroundImg.transform.position;
		Pos -= new Vector3(moveSpeed, 0, 0);
		if(Pos.x < MinX)
			Pos.x = StartPos;

		BackgroundImg.transform.position = Pos;
	}
}
