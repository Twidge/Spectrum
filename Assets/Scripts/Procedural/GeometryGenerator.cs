using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum Colours { Red, Yellow, Green, Turquoise, Blue };

public class GeometryGenerator : MonoBehaviour
{
	public Transform player;
	public GameObject path;
	public GameObject goal;
	public GameObject platform;
	public GameObject conditionalPlatform;
	public GameObject lift;
	public GameObject light;
	public List<GameObject> platformArray;
	GameObject nextPlatform;
	
	// Global random numbers to keep track of different states
	
	private int globalRN = -1;
	private int globalLiftRN = -1;
	
	public static int platformsInstantiated = 0;
	
	public Material conditionalRed, conditionalGreen, conditionalBlue, conditionalYellow, conditionalTurquoise;
	
	[Range(0.1f,0.9f)]
	public float platformSizeDegeneracy; // Determines the decrease in platform size when moving upwards
	
	Vector3 currentPlatformSize;
	
	public static bool geometryGenReady = false;

	void Start ()
	{	
		platformArray = new List<GameObject>();
		currentPlatformSize = platform.transform.localScale;
		geometryGenReady = true;
	}
	
	void Update ()
	{		
		if(geometryGenReady)
		{
			platformsInstantiated = 0;
			geometryGenReady = false;
			
			FitGeometryToNodeArray();
		}
	}
	
	// Main function, organises the fitting of geometry to the random walk
	
	void FitGeometryToNodeArray()
	{
		foreach(Node x in WalkGenerator.nodeArray)
		{
			platformsInstantiated++;
		
			// If moving upwards between platforms, reduce platform size
		
			if(x.lastDirection.y > 0f)
			{
				currentPlatformSize.x *= platformSizeDegeneracy;
				currentPlatformSize.z *= platformSizeDegeneracy;
			}
			
			// If moving downwards between platforms, increase platform size (up to the original)
			
			else if(x.lastDirection.y < 0f && x.position.y < 0f)
			{
				currentPlatformSize = platform.transform.localScale;
			}
			
			// Create platform and set size
			
			if(x.lastDirection.y > 0f)
			{
				nextPlatform = Instantiate (conditionalPlatform, x.position, Quaternion.identity) as GameObject;
				nextPlatform.GetComponent<ConditionalPlatform>().player = player;
				nextPlatform.GetComponent<ConditionalPlatform>().wall = WalkGenerator.topWall.transform;
				
				if(platformsInstantiated != WalkGenerator.nodeArray.Count)
				{
					Instantiate (light, new Vector3 (x.position.x, x.position.y + (2 * currentPlatformSize.y), x.position.z), Quaternion.identity);
				}
				
				globalRN = Random.Range (Mathf.Max (globalLiftRN - 1, (int)Colours.Red), Mathf.Min (globalLiftRN + 2, (int)Colours.Blue + 1));
				
				switch(globalRN)
				{
					case (int)Colours.Red :
					{
						nextPlatform.GetComponent<Renderer>().material = conditionalRed;
						break;
					}
						
					case (int)Colours.Yellow :
					{
						nextPlatform.GetComponent<Renderer>().material = conditionalYellow;
						break;
					}
						
					case (int)Colours.Green :
					{
						nextPlatform.GetComponent<Renderer>().material = conditionalGreen;
						break;
					}
						
					case (int)Colours.Turquoise :
					{
						nextPlatform.GetComponent<Renderer>().material = conditionalTurquoise;
						break;
					}
						
					case (int)Colours.Blue :
					{
						nextPlatform.GetComponent<Renderer>().material = conditionalBlue;
						break;
					}
						
					default :
					{
						break;
					}
				}
			}
		
			else
			{
				nextPlatform = Instantiate (platform, x.position, Quaternion.identity) as GameObject;
				
				if(platformsInstantiated != WalkGenerator.nodeArray.Count)
				{
					Instantiate (light, new Vector3 (x.position.x, x.position.y + (2 * currentPlatformSize.y), x.position.z), Quaternion.identity);
				}
			}
			
			nextPlatform.transform.localScale = currentPlatformSize;
			
			platformArray.Add (nextPlatform);
			
			// If next node exists and is in a horizontal direction to x, put a bridge between the platforms
			
			if(x.nextDirection.x != 0f || x.nextDirection.z != 0f)
			{
				CreatePathFromNode(x);
			}
			
			// If next node exists and is above x, put a lift of random colour on the current platform at a random corner
			
			else if(x.nextDirection.y > 0f)
			{
				CreateLiftAtNode(x);
			}
			
			// If next direction does not exist (ie. last node) then place a goal on the platform
			
			else if(x.nextDirection.x == 0f && x.nextDirection.y == 0f && x.nextDirection.z == 0f)
			{
				CreateGoalAtNode(x);
			}
			
			else if(x.nextDirection.y < 0f)
			{
				// Do nothing yet!
			}
			
			else
			{
				Debug.Log ("Error: no valid case for x.nextDirection");
			}
			
			globalRN = -1;
		}
	}
	
	// Creates a teleporter on a platform (and an exit teleporter on the next platform)
	
	void CreateTeleporterAtNode(Node x, Node y)
	{
		GameObject newPortEntrance, newPortExit;
		
		float nr = UnityEngine.Random.Range (0f, 1f);
		
		float xPos = nextPlatform.transform.position.x;
		float zPos = nextPlatform.transform.position.z;		
	}
	
	// Creates a lift on a platform
	
	void CreateLiftAtNode(Node x)
	{
		GameObject newLift;
		
		float nr = UnityEngine.Random.Range (0f, 1f);
		
		float xPos = nextPlatform.transform.position.x;
		float zPos = nextPlatform.transform.position.z;
		
		if(nr >= 0f && nr < 0.25f)
		{
			xPos += ((nextPlatform.transform.localScale.x / 2) -(currentPlatformSize.x / platform.transform.localScale.x));
			zPos += ((nextPlatform.transform.localScale.z / 2) - (currentPlatformSize.z / platform.transform.localScale.z));
		}
		
		else if(nr >= 0.25f && nr < 0.5f)
		{
			xPos += ((nextPlatform.transform.localScale.x / 2) - (currentPlatformSize.x / platform.transform.localScale.x));
			zPos -= ((nextPlatform.transform.localScale.z / 2) - (currentPlatformSize.z / platform.transform.localScale.z));
		}
		
		else if(nr >= 0.5f && nr < 0.75f)
		{
			xPos -= ((nextPlatform.transform.localScale.x / 2) - (currentPlatformSize.x / platform.transform.localScale.x));
			zPos -= ((nextPlatform.transform.localScale.z / 2) - (currentPlatformSize.z / platform.transform.localScale.z));
		}
		
		else
		{
			xPos -= ((nextPlatform.transform.localScale.x / 2) - (currentPlatformSize.x / platform.transform.localScale.x));
			zPos += ((nextPlatform.transform.localScale.z / 2) - (currentPlatformSize.z / platform.transform.localScale.z));
		}
		
		newLift = Instantiate (lift, new Vector3(xPos, nextPlatform.transform.position.y + nextPlatform.transform.localScale.y / 2, zPos), Quaternion.identity) as GameObject;
		
		newLift.GetComponent<LiftPad>().player = player;
		newLift.GetComponent<LiftPad>().wall = WalkGenerator.topWall.transform;
		newLift.transform.localScale = new Vector3 (newLift.transform.localScale.x * currentPlatformSize.x / platform.transform.localScale.x,
													newLift.transform.localScale.y,
													newLift.transform.localScale.z * currentPlatformSize.z / platform.transform.localScale.z);
		
		globalLiftRN = UnityEngine.Random.Range ((int)Colours.Red, (int)Colours.Blue + 1);
		
		if(globalLiftRN == 0)
		{
			newLift.GetComponent<Renderer>().material = conditionalRed;
		}
		
		else if(globalLiftRN == 1)
		{
			newLift.GetComponent<Renderer>().material = conditionalYellow;
		}
		
		else if(globalLiftRN == 2)
		{
			newLift.GetComponent<Renderer>().material = conditionalGreen;
		}
		
		else if(globalLiftRN == 3)
		{
			newLift.GetComponent<Renderer>().material = conditionalTurquoise;
		}
		
		else
		{
			newLift.GetComponent<Renderer>().material = conditionalBlue;
		}
		
		float foo = newLift.transform.FindChild("Lift Shaft").transform.localScale.x;
		float bar = newLift.transform.FindChild("Lift Shaft").transform.localScale.z;
		
		// Slight adjustment of the y-scaling to allow for edge cases where platform can't be reached
		
		newLift.transform.FindChild("Lift Shaft").transform.localScale = new Vector3(foo, (x.nextDirection.y / newLift.transform.localScale.y) * 1.1f, bar);
	}
	
	// Creates a path between platforms
	
	void CreatePathFromNode(Node x)
	{		
		float nr = UnityEngine.Random.Range(0, 2);
		
		GameObject newPath = Instantiate (path, nextPlatform.transform.position + (x.nextDirection / 2), Quaternion.identity) as GameObject;
		Instantiate (light, new Vector3 (newPath.transform.position.x, newPath.transform.position.y + (2 * newPath.transform.localScale.y), newPath.transform.position.z), Quaternion.identity);
		
		// If the most recently created platform is conditional (and not green), swap the path around so whichever of red/blue is nearest to that colour will be closest to the platform
		
		if(globalRN != -1 && globalRN != (int)Colours.Green)
		{
			Debug.Log ("Code executing.");
			
			if((globalRN == (int)Colours.Red || globalRN == (int)Colours.Yellow) && x.nextDirection.x > 0f)
			{
				newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.x) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);	
			}
			
			else if((globalRN == (int)Colours.Turquoise || globalRN == (int)Colours.Blue) && x.nextDirection.x > 0f)
			{
				newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));	
				newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.x) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);
			}
			
			else if((globalRN == (int)Colours.Red || globalRN == (int)Colours.Yellow) && x.nextDirection.x < 0f)
			{
				newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
				newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.x) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);
			}
			
			else if((globalRN == (int)Colours.Turquoise || globalRN == (int)Colours.Blue) && x.nextDirection.x < 0f)
			{
				newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.x) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);	
			}
			
			else if((globalRN == (int)Colours.Red || globalRN == (int)Colours.Yellow) && x.nextDirection.z > 0f)
			{
				newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
				newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.z) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);	
			}
			
			else if((globalRN == (int)Colours.Turquoise || globalRN == (int)Colours.Blue) && x.nextDirection.z > 0f)
			{
				newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
				newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.z) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);
			}
			
			else if((globalRN == (int)Colours.Red || globalRN == (int)Colours.Yellow) && x.nextDirection.z < 0f)
			{
				newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
				newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.z) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);
			}
			
			else if((globalRN == (int)Colours.Turquoise || globalRN == (int)Colours.Blue) && x.nextDirection.z < 0f)
			{
				newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
				newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.z) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);	
			}
		}
		
		else if(nr == 0 && x.nextDirection.x != 0f)
		{
			newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.x) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);
		}
		
		else if(nr == 1 && x.nextDirection.x != 0f)
		{
			newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.x) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);
		}
		
		else if(nr == 0 && x.nextDirection.z != 0f)
		{
			newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
			newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.z) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);
		}
		
		else if(nr == 1 && x.nextDirection.z != 0f)
		{
			newPath.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
			newPath.transform.localScale = new Vector3((Mathf.Abs (x.nextDirection.z) - currentPlatformSize.x - path.transform.Find ("Platform").transform.localScale.x) / newPath.transform.FindChild("Path Indicator").transform.localScale.x, 1f, 1f);
		}
		
		newPath.transform.Find ("Platform").transform.localScale = new Vector3(path.transform.Find ("Platform").localScale.x / newPath.transform.localScale.x, path.transform.Find ("Platform").localScale.y, path.transform.Find ("Platform").localScale.z);
	}
	
	// Creates a goal (should only happen on the final platform)
	
	void CreateGoalAtNode(Node x)
	{
		Instantiate (goal, new Vector3(nextPlatform.transform.position.x, nextPlatform.transform.position.y + (platform.transform.localScale.y / 2f) + (goal.transform.localScale.y / 2f), nextPlatform.transform.position.z), Quaternion.identity);
	}
}
