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
	public Texture2D PlayerOneTex;
	public Texture2D PlayerTwoTex;

	[Header("Ownership")]
	public CaptureState ownerState;
	public GameObject owner;

	[Header("Capturing")]
	public float conversionSpeed = 5.0f;
	public float captureProgress = 0.0f;

	[Header("Objects")]
	public GameObject PlayerOne;
	public GameObject PlayerTwo;

	private Material material;
	private int ID = 0;

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

		// Access material
		material = GetComponent<Renderer>().material;

		// Create Textures
		UncapturedTex = ConvertToTexture(Uncaptured);
		PlayerOneTex = ConvertToTexture(PlayerOneCaptured);
		PlayerTwoTex = ConvertToTexture(PlayerTwoCaptured);

		UpdateShader(UncapturedTex);
	}

	private void UpdateOwner()
	{
		bool Check;

		// Check if its unowned
		Check = RangeCheck (captureProgress, -50, 50);
		if(Check)
		{
			ownerState = CaptureState.UNCAPTURED;
			UpdateShader (UncapturedTex);
		}

		// Check for player one
		Check = RangeCheck (captureProgress, 50, 150);
		if(Check)
		{
			ownerState = CaptureState.PLAYERONE;
			UpdateShader (PlayerOneTex);
		}

		// Check for player two
		Check = RangeCheck (captureProgress, -150, -50);
		if(Check)
		{
			ownerState = CaptureState.PLAYERTWO;
			UpdateShader (PlayerTwoTex);
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
