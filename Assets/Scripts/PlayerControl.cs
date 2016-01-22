using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public float moveSpeed;
	public float jumpForce;
	public float maxSpeed;
	public float zoomForce;
	public Transform gameCamera;
	
	public Transform lookTarget;
	
	public Transform currentPlatform;
	private bool isJumping = false;
	private Rigidbody body;
	
	private Vector3 currentPlatformPositionLastFrame;
	private Vector3 currentPlatformPosition;
	
	// public Image fade;
	
	private const int M_PROCEDURAL_LEVEL = 7;

	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody>();
		
		// PlayerControl.canMove = false;
		
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.Log ("fadeInReady is: " + MenuFadeInOut.fadeInReady);
		Debug.Log ("fadeInPlaying is: " + MenuFadeInOut.fadeInPlaying);
		Debug.Log ("fadeOutPlaying is: " + MenuFadeInOut.fadeOutPlaying);
			
		float cameraYAngle = gameCamera.eulerAngles.y;
		float actualSpeed = moveSpeed * Time.deltaTime;
		
		// WASD Movement
		
		if(Input.GetKey(KeyCode.W))
		{
			Vector3 normalisedDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * cameraYAngle), 0, Mathf.Cos(Mathf.Deg2Rad * cameraYAngle));
			
			transform.position += actualSpeed * normalisedDirection;
		}
		
		if(Input.GetKey (KeyCode.S))
		{
			Vector3 normalisedDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * cameraYAngle), 0, Mathf.Cos(Mathf.Deg2Rad * cameraYAngle));
			
			transform.position -= actualSpeed * normalisedDirection;
		}
		
		if(Input.GetKey (KeyCode.A))
		{
			Vector3 normalisedLeftDirection = new Vector3(Mathf.Sin (Mathf.Deg2Rad * (cameraYAngle - 90)), 0, Mathf.Cos (Mathf.Deg2Rad * (cameraYAngle - 90)));
			
			transform.position += actualSpeed * normalisedLeftDirection;
		}
		
		if(Input.GetKey (KeyCode.D))
		{
			Vector3 normalisedLeftDirection = new Vector3(Mathf.Sin (Mathf.Deg2Rad * (cameraYAngle - 90)), 0, Mathf.Cos (Mathf.Deg2Rad * (cameraYAngle - 90)));
			
			transform.position -= actualSpeed * normalisedLeftDirection;
		}
		
		// Jump movement
		
		if(Input.GetKey (KeyCode.Space) && !isJumping)
		{
			body.AddForce (new Vector3(0, jumpForce, 0));
			isJumping = true;
		}
		
		// If the current level is not the menu, Escape moves the player to the menu - otherwise it quits the game
		
		if(Input.GetKey (KeyCode.Escape) && Application.loadedLevel != 0)
		{
			Application.LoadLevel (0);
		}
		
		if(Input.GetKey (KeyCode.Escape) && Application.loadedLevel == 0)
		{
			Application.Quit();
		}
		
		// Check if player walked off platform or walked into a lift (and disable jump)
		
		if(body.velocity.y <= -1 || body.velocity.y >= 1)
		{
			isJumping = true;
		}
		
		// Check platform movement
		
		if(!isJumping)
		{
			Vector3 checkVector = currentPlatformPosition - currentPlatformPositionLastFrame;
			
			if(checkVector != Vector3.zero)
			{
				transform.position += checkVector;
			}
		}
		
		if(currentPlatform != null)
		{
			currentPlatformPositionLastFrame = currentPlatformPosition;
			currentPlatformPosition = currentPlatform.position;
			
			// Check if on a zoom path
			
			if(currentPlatform.tag == "ZoomPath" && this.GetComponent<Rigidbody>().velocity.magnitude <=  maxSpeed)
			{
				Vector3 pushDirection = currentPlatform.GetChild(0).transform.position - transform.position;
			
				this.GetComponent<Rigidbody>().AddForce ((zoomForce * pushDirection) / pushDirection.magnitude);
			}
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		currentPlatform = collision.collider.transform;
		currentPlatformPositionLastFrame = currentPlatformPosition = currentPlatform.position;
		
		// If colliding with this surface is supposed to kill the player, reset the level
		
		if(collision.collider.tag == "DeathWall")
		{
			transform.position = new Vector3(0f, 1f, 0f);
		}
		
		// If colliding with procedural goal, create a new procedural level
		
		if(collision.collider.tag == "Procedural Goal")
		{
			Application.LoadLevel (M_PROCEDURAL_LEVEL);
		}
		
		// If colliding with the goal, go to next level
		
		if(collision.collider.tag == "Goal")
		{
			Application.LoadLevel (Application.loadedLevel + 1);	
		}
		
		// If colliding with exit button, quit game
		
		if(collision.collider.tag == "Quit")
		{
			Application.Quit();
		}
		
		// If colliding with teleporter, teleport
		
		if(collision.collider.tag == "Teleport")
		{
			Transform outPoint = collision.collider.transform.Find("OutPoint").transform;
			Transform oppositePort = collision.collider.GetComponent<Teleport>().oppositePort;
			
			this.transform.position = outPoint.position;
			
			lookTarget = oppositePort.Find("Front").transform;
			
			gameCamera.transform.LookAt(lookTarget);
		}
		
		// Resets jump if player has landed on a platform (checks directly underneath centre and directly underneath player extremities (to prevent corner cases where player centre is not
		// directly above land).
		
		Vector3 checkPosition = new Vector3 (transform.position.x, transform.position.y - (transform.localScale.y + 0.05f), transform.position.z);
		
		if(collision.collider.bounds.Contains(checkPosition))
		{
			transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
			isJumping = false;
		}
		
		// NOTE: more than four checks are necessary. Just doing i < 4 and PI / 2 fails when player lands on the exact corner of a platform.
		// Interesting question: is five enough? I think so, but not sure.
		
		for(int i = 0; i < 8; i++)
		{
			Vector3 nextCheckPosition = new Vector3 (transform.position.x + (transform.localScale.x * Mathf.Sin(i * (Mathf.PI / 4))), transform.position.y - (transform.localScale.y + 0.05f), transform.position.z + (transform.localScale.z * Mathf.Cos (i * (Mathf.PI / 4))));
			
			if(collision.collider.bounds.Contains(nextCheckPosition))
			{
				isJumping = false;
			}
		}
	}
	
	void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject == currentPlatform.gameObject)
		{
			currentPlatform = null;
		}
	}
}
