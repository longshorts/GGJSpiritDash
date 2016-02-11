using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal : MonoBehaviour
{
	public GameController gameController;
	private BoxCollider boxCollider;

	[Header("Sound")]
	public AudioClip soundGameOver;
	public AudioClip soundPortal;
	private AudioSource audioSource;

	[Header("Portal Opening")]
	public bool isActivated;
	public bool isOpened;
	public List<GameObject> portalParts = new List<GameObject>(4);
	public Transform rotatePoint;

	[Header("Animation Speeds")]
	public float spiralSpeed = 2.0f;						// How fast the background spiral rotates
	public float spiralCoreSpeed = 1.0f;					// How fast the core spiral rotates
	public float runeSpeed = 0.5f;							// How fast the runes rotate
	public float openSpeed = 10.0f;							// How long it takes to open
	public float closeSpeed = 10.0f;							// How long it takes to close
	private float portalFrame = 0.0f;						// Animating the opening of the portal

	private Vector3 startScale = new Vector3(0,0,0);		// Start scale for the portal
	public Vector3 portalScale = new Vector3(1,1,1);		// Final scale for the portal
	
	// Use this for initialization
	void Start ()
	{
		// Disable collider
		boxCollider = GetComponent<BoxCollider> ();
		boxCollider.enabled = false;

		// Get the audio source
		audioSource = GetComponent<AudioSource> (); 

		// Disable
		isActivated = false;

		// Deactivate the portal
		foreach(GameObject obj in portalParts)
		{
			// Deactive portal
			obj.SetActive(false);

			// Reduce scale
			obj.transform.localScale = new Vector3(0,0,0);
		}
	}

	void Update()
	{
		// Portal background
		portalParts[1].transform.RotateAround(rotatePoint.position, new Vector3(0, 1, 0), Time.deltaTime * spiralSpeed);

		// Portal foreground
		portalParts[2].transform.RotateAround(rotatePoint.position, new Vector3(0, 1, 0), Time.deltaTime * spiralCoreSpeed);

		// Runes
		portalParts[3].transform.RotateAround(rotatePoint.position, new Vector3(0, 1, 0), Time.deltaTime * runeSpeed);
	}
	
	public void Activate()
	{
		// Don't try activating again
		if(isActivated)
			return;
		
		// Activate the portal parts
		foreach(GameObject obj in portalParts)
		{
			obj.SetActive(true);
		}

		// Reset animation
		portalFrame = 0.0f;

		// Flag its been opened
		isActivated = true;
		
		// Scale rotation speeds
		spiralSpeed *= 10.0f;
		spiralCoreSpeed *= 10.0f;
		runeSpeed *= 10;
		
		// Open the portal
		StartCoroutine(PortalSimulation(startScale, portalScale, openSpeed, true));
	}
	
	public void Deactivate()
	{
		// Don't try activating again
		if(!isActivated)
			return;
		
		// Flag its been opened
		isActivated = false;

		// Reset animation
		portalFrame = 0.0f;

		// Scale rotation speeds
		spiralSpeed *= 10.0f;
		spiralCoreSpeed *= 10.0f;
		runeSpeed *= 10;

		// Deactivate
		boxCollider.enabled = false;

		// Close the portal
		StartCoroutine(PortalSimulation(portalScale, startScale, closeSpeed, false));
	}

	void OnTriggerEnter (Collider other)
	{
		if(!isActivated)
			return;

		if(!isOpened)
			return;

		// Get the player component
		PlayerController player = other.gameObject.GetComponent<PlayerController>();
		if(!player)
			return;

		// If the colliding player has captured their objectives
		if(player.isComplete)
		{
			// Flag we have won
			player.isWinner = true;
			audioSource.PlayOneShot(soundGameOver, 0.7f);
			Debug.Log ("Victory for " + other.tag);
		}
	}

	private IEnumerator PortalSimulation(Vector3 start, Vector3 end, float speed, bool flag)
	{
		Vector3 currentScale;
		while(true)
		{
			portalFrame += Time.deltaTime;
			currentScale = Vector3.Lerp(start, end, portalFrame / speed);
			
			// Set the scale for the portal parts
			foreach(GameObject obj in portalParts)
			{
				obj.transform.localScale = currentScale;
			}
			
			// Check if we have finished
			if(currentScale.Equals(end))
			{
				break;
			}
			else
			{
				yield return null;
			}
		}

		// Flag its open state
		isOpened = flag;
		
		// Scale back rotation speeds
		spiralSpeed /= 10.0f;
		spiralCoreSpeed /= 10.0f;
		runeSpeed /= 10;
	}
}
