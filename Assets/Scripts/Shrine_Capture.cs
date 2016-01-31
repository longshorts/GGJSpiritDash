using UnityEngine;
using System.Collections;

public class Shrine_Capture : MonoBehaviour
{		
	public enum CaptureState {
		UNCAPTURED,
		PLAYERONE,
		PLAYERTWO
	};
	
	public int shrineID;					// int referring to shrine number
	public CaptureState ownerState;
	public GameObject owner;				// reference to the player that owns the shrine

	[Header("Capturing")]
	public float conversionSpeed = 1.0f;	// speed at which the shrine is captured
	public float captureProgress;

	[Header("Objects")]
	public GameObject PlayerOne;
	public GameObject PlayerTwo;
	public TileBlender Blender;

	// Use this for initialization
	void Start ()
	{
		// initialise the shrine
		captureProgress = 0.0f;

		// Set initial state to unowned
		ownerState = CaptureState.UNCAPTURED;

		// Find objects
		PlayerOne = GameObject.Find ("Player1");
		PlayerTwo = GameObject.Find ("Player2");

		// Access tile blender
		Blender = GetComponent<TileBlender>();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	private void UpdateOwner()
	{
		bool Check;

		// Check if its unowned
		Check = RangeCheck (captureProgress, -50, 50);
		if(Check)
		{
			ownerState = CaptureState.UNCAPTURED;
			Blender.BlendToNeutral();
		}

		// Check for player one
		Check = RangeCheck (captureProgress, 50, 150);
		if(Check)
		{
			ownerState = CaptureState.PLAYERONE;
			Blender.BlendToPlayerOne();
		}

		// Check for player two
		Check = RangeCheck (captureProgress, -150, -50);
		if(Check)
		{
			ownerState = CaptureState.PLAYERTWO;
			Blender.BlendToPlayerTwo();
		}
				
		switch(ownerState)
		{
			case CaptureState.UNCAPTURED:
				owner = null;
				break;

			case CaptureState.PLAYERONE:
				owner = PlayerOne;
				break;

			case CaptureState.PLAYERTWO:
				owner = PlayerTwo;
				break;
		}
	}

	void OnTriggerStay(Collider other)
	{

		if (other.gameObject.tag == "Player1")
		{
			captureProgress -= conversionSpeed * Time.deltaTime;
			Clamp(ref captureProgress, -150, 150);
			UpdateOwner();
		}
		else if (other.gameObject.tag == "Player2")
		{	
			captureProgress += conversionSpeed * Time.deltaTime;
			Clamp(ref captureProgress, -150, 150);
			UpdateOwner();
		}
	}

	public bool CheckOwner(CaptureState state)
	{
		return ownerState.Equals(state);
	}

	private bool RangeCheck(float value, float min, float max)
	{
		return (value >= min) && (value <= max);
	}

	private void Clamp(ref float value, float min, float max)
	{
		if(value < min)
			value = min;
		else if(value > max)
			value = max;
	}
}
