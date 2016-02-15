using UnityEngine;
using System.Collections;

public class AttackAbility : Ability
{
	[Header("Spell Specific")]
	public GameObject oppositionTarget;
	public float FOV = 45;
	public float attackRange = 3;
	public float attackDamage = 10;
	public float slashDuration = 2;
	public GameObject attackPrefab;
	private float slashFrame = 0.0f;
	private GameObject attackVisual;

	private Animator animator;

	void Start()
	{
		// Get access to the opposing player
		if(gameObject.tag.Equals("Player1"))
		{
			oppositionTarget = GameObject.FindGameObjectWithTag("Player2");
		}
		else if(gameObject.tag.Equals("Player2"))
		{
			oppositionTarget = GameObject.FindGameObjectWithTag("Player1");
		}

		// Set no object created yet
		attackVisual = (GameObject)Instantiate(attackPrefab);
		attackVisual.name = "Frost - " + tag;
		attackVisual.SetActive(false);

		animator = GetComponent<Animator> ();
	}

	void Update()
	{
		// Debugging
		Vector3 LeftRay = Quaternion.AngleAxis(-FOV/2, Vector3.up) * transform.GetComponent<PlayerController>().directionVector3D;
		Vector3 RightRay = Quaternion.AngleAxis(FOV/2, Vector3.up) * transform.GetComponent<PlayerController>().directionVector3D;
		Debug.DrawRay(transform.position, LeftRay * attackRange);
		Debug.DrawRay(transform.position, RightRay * attackRange);
	}

	public override void CastAbility ()
	{
		// Show the visual
		/*attackVisual.SetActive(true);
		attackVisual.transform.position = transform.position + (GetComponent<PlayerController>().directionVector3D * attackRange);
		attackVisual.transform.rotation = transform.rotation;
		Vector3 scale = attackVisual.transform.localScale;
		scale.x *= -1;
		attackVisual.transform.localScale = scale;

		// Reset duration
		slashFrame = 0.0f;

		// Show the slash
		StartCoroutine(ShowSlash());*/

		animator.SetTrigger ("attack");

		// Attack forward
		SlashForward();

		StartCooldown();
	}

	private void SlashForward()
	{
		// Calculate a direction vector between us and the opposition
		Vector3 direction = oppositionTarget.transform.position - transform.position;
		
		// Calculate the angle between the guard and animal
		float angle = Vector3.Angle(direction, transform.GetComponent<PlayerController>().directionVector3D);
		
		// Check if we are within the field of view
		if(angle < FOV * 0.5f)
		{
			RaycastHit hit;
			
			// Create a ray between the our position and the target position
			if(Physics.Raycast(transform.position, direction.normalized, out hit, attackRange))
			{
				// Check if we hit the player
				if(hit.collider.gameObject.tag == oppositionTarget.tag)
				{
					Debug.Log ("Found player");
					// Attack player
					oppositionTarget.GetComponent<PlayerController>().Kill ();
				}
			}
		}
	}

	private IEnumerator ShowSlash()
	{
		// No point progressing if we dont have a visual
		if(!attackVisual)
		{
			yield return new WaitForEndOfFrame();
		}

		// Wait until cooldown has finished then destroy object
		while(true)
		{
			slashFrame += Time.deltaTime;
			if(slashFrame >= slashDuration)
			{
				break;
			}

			yield return null;
		}

		attackVisual.SetActive(false);

		yield return new WaitForEndOfFrame();
	}
}

