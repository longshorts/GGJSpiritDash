using UnityEngine;
using System.Collections;

public class Shrine : MonoBehaviour
{		
	public enum CaptureState { UNCAPTURED, PLAYERONE, PLAYERTWO };

	[Header("Textures")]
	public Sprite uncapturedSprite;
	public Sprite playerOneCapturedSprite;
	public Sprite playerTwoCapturedSprite;
	public Texture2D uncapturedTexture;

	[Header("Colours")]
	public Color PlayerOneColor = new Color(0,0,1,1);
	public Color PlayerTwoColor = new Color(1,0,0,1);
	public Color NeutralColor = new Color(1,1,1,1);

	[Header("Ownership")]
	public CaptureState ownerState;
	public GameObject owner;

	[Header("Capturing")]
	public float conversionSpeed = 0.0f;
	public float captureProgress = 76.0f;
	private GameObject currentCapturer = null;

	[Header("Objects")]
	public GameObject PlayerOne;
	public GameObject PlayerTwo;

	private Material material;

	public AudioClip shrineSound;
	private AudioSource audioSource;

	private Color captureColor = new Color(1,1,1,1);

	// Use this for initialization
	void Start ()
	{
		// initialise the shrine
		captureProgress = 76.0f;
		conversionSpeed = 40.0f;

		// Set initial state to unowned
		ownerState = CaptureState.UNCAPTURED;

		// Find objects
		PlayerOne = GameObject.FindGameObjectWithTag("Player1");
		PlayerTwo = GameObject.FindGameObjectWithTag("Player2");

		// Access material and set initial colour
		material = GetComponent<Renderer>().material;
		material.SetColor("_IconColour", captureColor);
		material.SetTexture("_IconTex", uncapturedTexture);

		// Access audio source
		audioSource = GetComponent<AudioSource> ();
	}

	void Update()
	{
		// If we dont have a player attempting to capture, don't do anything
		if(!currentCapturer)
			return;

		float Percentage = 0.0f;

		if(ownerState == CaptureState.PLAYERONE)
		{
			Debug.Log ("Player One Capture!");
			Percentage = CalculatePercentage(captureProgress, 0, 50);

			// We already own so just continue lerping until
			captureColor = Color.Lerp(PlayerOneColor, NeutralColor, Percentage);
		}
		else if(ownerState == CaptureState.PLAYERTWO)
		{
			Debug.Log ("Player Two Capture!");
			Percentage = CalculatePercentage(captureProgress, 102, 151);

			captureColor = Color.Lerp(NeutralColor, PlayerTwoColor, Percentage);
		}
		else if(ownerState == CaptureState.UNCAPTURED)
		{
			captureColor = NeutralColor;
		}

		// Update material
		material.SetColor("_IconColour", captureColor);
	}

	public bool IsCaptured()
	{
		switch(ownerState)
		{
			case CaptureState.PLAYERONE:
				if(captureProgress <= 5)
					return true;

				break;

			case CaptureState.PLAYERTWO:
				if(captureProgress >= 147)
					return true;

				break;
		}

		return false;
	}

	private void UpdateOwner()
	{
		bool Check;
		
		// Check for player one
		Check = RangeCheck (captureProgress, 0, 50);
		if(Check)
		{
			ownerState = CaptureState.PLAYERONE;
			owner = PlayerOne;
			
			if(!audioSource.isPlaying)
			{
				audioSource.PlayOneShot(shrineSound, 0.6f);
			}
			
			return;
		}

		// Check if its unowned
		Check = RangeCheck (captureProgress, 51, 101);
		if(Check)
		{
			ownerState = CaptureState.UNCAPTURED;
			owner = null;
			
			return;
		}

		// Check for player two
		Check = RangeCheck (captureProgress, 102, 152);
		if(Check)
		{
			ownerState = CaptureState.PLAYERTWO;
			owner = PlayerTwo;
			if(!audioSource.isPlaying)
			{
				audioSource.PlayOneShot(shrineSound, 0.6f);
			}

			return;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player1")
		{
			currentCapturer = PlayerOne;
			captureProgress -= conversionSpeed * Time.deltaTime;
			Clamp(ref captureProgress, 0, 152);
			UpdateOwner();
		}
		else if (other.gameObject.tag == "Player2")
		{	
			currentCapturer = PlayerTwo;
			captureProgress += conversionSpeed * Time.deltaTime;
			Clamp(ref captureProgress, 0, 152);
			UpdateOwner();
		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log ("Exit!");
		currentCapturer = null;
	}

	//================
	// FUNCTIONALITY
	//================

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

	private float CalculatePercentage(float value, float lower, float upper)
	{
		float range = upper - lower;

		return (value - lower) / range;
	}
}
