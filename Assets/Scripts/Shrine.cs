using UnityEngine;
using System.Collections;

public class Shrine : MonoBehaviour
{		
	public enum CaptureState { UNCAPTURED, PLAYERONE, PLAYERTWO };

	[Header("Textures")]
	public Sprite Uncaptured;
	public Sprite PlayerOneCaptured;
	public Sprite PlayerTwoCaptured;

	public Texture2D UncapturedTex;

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
	private float lastCaptureProgress = 76.0f;
	private GameObject currentCapturer = null;

	[Header("Objects")]
	public GameObject PlayerOne;
	public GameObject PlayerTwo;

	private Material material;
	private int ID = 0;

	public AudioClip shrineSound;
	private AudioSource audio;

	private Color captureColor;

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
		captureColor = new Color(1,1,1,1);
		material.SetColor("_IconColour", captureColor);

		// Create Textures
		UncapturedTex = ConvertToTexture(Uncaptured);

		UpdateShader(UncapturedTex);
		audio = GetComponent<AudioSource> ();
	}

	void Update()
	{
		// If we havent progressed in the capture, don't do anything
		//if(captureProgress.Equals(lastCaptureProgress))
			//return;

		// If we dont have a player attempting to capture, don't do anything
		if(!currentCapturer)
			return;

		// Update the last progress
		lastCaptureProgress = captureProgress;

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
			Debug.Log ("This does actually get called!");
			captureColor = NeutralColor;
		}

		//Debug.Log (captureColor);

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
			
			if(!audio.isPlaying)
			{
				audio.PlayOneShot(shrineSound, 0.6f);
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
			if(!audio.isPlaying)
			{
				audio.PlayOneShot(shrineSound, 0.6f);
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

	private void UpdateShader(Texture2D texture)
	{
		// Update
		material.SetTexture("_IconTex", texture);
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

	// THIS MAY BE DELETED AT SOME POINT - SEE HOW IT GOES
	public Texture2D ConvertToTexture(Sprite sprite)
	{
		if(sprite.rect.width != sprite.texture.width){
			Texture2D newText = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
			Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, 
			                                             (int)sprite.textureRect.y, 
			                                             (int)sprite.textureRect.width, 
			                                             (int)sprite.textureRect.height );
			newText.SetPixels(newColors);
			newText.Apply();
			return newText;
		} else
			return sprite.texture;
	}
}
