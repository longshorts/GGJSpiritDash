using UnityEngine;
using System.Collections;

public class TileBlender : MonoBehaviour
{
	[Header("Textures")]
	public Texture PlayerOneTexture;
	public Texture PlayerTwoTexture;
	public Texture NeutralTexture;

	[Header("Properties")]
	public float BlendSpeed = 1.0f;

	public bool IsBlending;
	private float BlendAmount;
	private float BlendTarget;
	private Material ObjMaterial;

	void Start ()
	{
		// Get the objects material
		ObjMaterial = GetComponent<Renderer>().material;

		// Set initial properties
		IsBlending = false;
		BlendAmount = 0.0f;
		BlendTarget = 1.0f;

		// Set initial blend amount
		ObjMaterial.SetFloat("_BlendAmount", 0.0f);

		// Update texture
		ObjMaterial.SetTexture("_MainTex", NeutralTexture);
		ObjMaterial.SetTexture("_BlendTex", NeutralTexture);
	}

	void Update ()
	{
		// Dont bother progressing if we dont want to blend
		if(!IsBlending)
		{
			return;
		}

		// Lerp between our current amount and the target
		BlendAmount += BlendSpeed * Time.deltaTime;

		// Once we reach the target stop (Round to 3 decimal places)
		if(BlendAmount >= BlendTarget)
		{
			Debug.Log ("Done");
			IsBlending = false;
			BlendAmount = BlendTarget;
		}

		// Update the shader
		ObjMaterial.SetFloat("_BlendAmount", BlendAmount);
	}

	public void BlendToPlayerOne()
	{
		// Flag we should start blending
		IsBlending = true;

		// Reset blend amount
		BlendAmount = 0.0f;
		BlendTarget = 1.0f;

		// Update Textures
		UpdateTexture ("_MainTex", ObjMaterial.GetTexture("_BlendTex"));
		UpdateTexture ("_BlendTex", PlayerOneTexture);
	}

	public void BlendToPlayerTwo()
	{
		// Flag we should start blending
		IsBlending = true;
		
		// Reset blend amount
		BlendAmount = 0.0f;
		BlendTarget = 1.0f;
		
		// Update Textures
		UpdateTexture ("_MainTex", ObjMaterial.GetTexture("_BlendTex"));
		UpdateTexture ("_BlendTex", PlayerTwoTexture);
	}

	public void BlendToNeutral()
	{
		// Flag we should start blending
		IsBlending = true;
		
		// Reset blend amount
		BlendAmount = 0.0f;
		BlendTarget = 1.0f;

		// Update Textures
		UpdateTexture ("_MainTex", ObjMaterial.GetTexture("_BlendTex"));
		UpdateTexture ("_BlendTex", NeutralTexture);
	}

	public void UpdateTexture(string name, Texture tex)
	{
		// Only update the texture if its not already set to it
		if(ObjMaterial.GetTexture(name) != tex)
		{
			ObjMaterial.SetTexture(name, tex);
		}
	}
}
