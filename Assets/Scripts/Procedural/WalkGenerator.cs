using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Node
{
	public Vector3 position;
	public Vector3 lastDirection;
	public Vector3 nextDirection;
	public Node previousNode;
	public Node nextNode;
	
	public Node(Vector3 pos, Vector3 ld)
	{
		position = pos;
		lastDirection = ld;
	}
	
	public Node(Vector3 pos, Vector3 ld, Node node)
	{
		position = pos;
		lastDirection = ld;
		previousNode = node;
	}
}

public class WalkGenerator : MonoBehaviour
{	
	// Boundary Walls empty object

	public GameObject boundaryWalls;
	
	public static Transform topWall, bottomWall, negativeXWall, positiveXWall, negativeZWall, positiveZWall;

	public float minY, maxY;
	public float minX, maxX;
	public float minZ, maxZ;
	
	// Controls how close to the boundary walls the nodes are allowed to get
	
	public float borderBuffer;
	
	// minSep controls how close paths between nodes are allowed to be
	// (to stop the path looping somewhere, or being close enough to looping that the player may be able to circumvent game mechanics)
	
	public float minSep;
	
	public int maxChecks;
	public int maxNodes;
	public static bool shouldContinueWithNodes;
	
	public GameObject waypoint;
	public bool waypointsVisible;  // for debugging/testing purposes
	
	[Range(0,1)]
	public float XLikelihood, ZLikelihood, YLikelihood;
	
	Node startNode;
	public static List<Node> nodeArray;

	void Start ()
	{
		shouldContinueWithNodes = true;
		nodeArray = new List<Node>();
	
		topWall = boundaryWalls.transform.FindChild ("Top Wall").transform;
		bottomWall = boundaryWalls.transform.FindChild ("Bottom Wall").transform;
		negativeXWall = boundaryWalls.transform.FindChild ("Side Wall One").transform;
		positiveXWall = boundaryWalls.transform.FindChild ("Opposite Side Wall One").transform;
		negativeZWall = boundaryWalls.transform.FindChild ("Side Wall Two").transform;
		positiveZWall = boundaryWalls.transform.FindChild ("Opposite Side Wall Two").transform;
	
		// Default values if probabilities do not sum to 1
	
		if(XLikelihood + ZLikelihood + YLikelihood != 1f)
		{
			XLikelihood = ZLikelihood = 0.4f;
			YLikelihood = 0.2f;
		}
		
		// Create startNode, seed random generator with system time for "true" randomness
	
		startNode = new Node(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
		nodeArray.Add(startNode);
		
		Debug.Log("First Node Position: " + startNode.position);
		
		UnityEngine.Random.seed = System.DateTime.Now.Minute * System.DateTime.Now.Second * System.DateTime.Now.Millisecond;
		
		// Generate nodes
		
		while(shouldContinueWithNodes && nodeArray.Count < maxNodes)
		{
			Node newNode = GetNextNode(nodeArray[nodeArray.Count - 1], nodeArray);
			nodeArray.Add(newNode);
			
			Debug.Log ("Next Node Position: " + newNode.position);
		}
		
		// Trigger geometry part of level generator
		
		GeometryGenerator.geometryGenReady = true;
	}
	
	Node GetNextNode(Node lastNode, List<Node> nodesSoFar)
	{
		float foo;
		float bar;
		float nr;
		
		bool isNeg = false;
		
		Vector3 outVector;
		int attemptCounter = 0;
		
		// When the previous node is startNode, just pick a random direction/magnitude
		
		if(lastNode.lastDirection.x == 0f && lastNode.lastDirection.y == 0f && lastNode.lastDirection.z == 0f)
		{
			foo = UnityEngine.Random.Range (0f, 2f);
			nr = UnityEngine.Random.Range(0f, 1f);
			
			if(nr <= 0.5f)
			{
				isNeg = true;
			}
			
			if(foo >= 0f && foo < (XLikelihood * 2f))
			{
				// Pick random length along X axis
				
				bar = UnityEngine.Random.Range(minX, maxX);
				
				if(isNeg)
				{
					bar *= -1;
				}
				
				outVector = bar * Vector3.right;
			}
			
			else if(foo >= (XLikelihood * 2f) && foo < ((XLikelihood + ZLikelihood) * 2f))
			{
				// Pick random length along Z axis
				
				bar = UnityEngine.Random.Range(minZ, maxZ);
				
				if(isNeg)
				{
					bar *= -1f;
				}
				
				outVector = bar * Vector3.forward;
			}
			
			else
			{
				// Pick random length along Y axis
				
				bar = UnityEngine.Random.Range(minY, maxY);
				
				if(isNeg)
				{
					bar *= -1f;
				}
				
				outVector = bar * Vector3.up;
			}
			
			// Debug.Log ("outVector: " + outVector);
		}
		
		// If the previous node has a lastDirection, pick a direction orthogonal to it
		// Also check no near-collisions between paths 
		
		else if(lastNode.lastDirection.x != 0f)
		{
			do
			{
				foo = UnityEngine.Random.Range (XLikelihood * 2f, 2f);
				nr = UnityEngine.Random.Range(0f, 1f);
				
				if(nr <= 0.5f)
				{
					isNeg = true;
				}
				
				if(foo >= XLikelihood * 2f && foo < (XLikelihood + ZLikelihood) * 2f)
				{
					// Pick random length along Z axis
					
					bar = UnityEngine.Random.Range(minZ, maxZ);
					
					if(isNeg)
					{
						bar *= -1f;
					}
					
					outVector = bar * Vector3.forward;
				}
				
				else
				{
					// Pick random length along Y axis
					
					bar = UnityEngine.Random.Range(minY, maxY);
					
					if(isNeg)
					{
						bar *= -1f;
					}
					
					outVector = bar * Vector3.up;
				}
				
				attemptCounter++;
				// Debug.Log ("outVector: " + outVector);
			}
			while(PositionFails(lastNode, new Node(lastNode.position + outVector, outVector, lastNode), nodesSoFar) && attemptCounter <= maxChecks - 1);
		}
		
		else if(lastNode.lastDirection.y != 0f)
		{
			do
			{
				foo = UnityEngine.Random.Range (YLikelihood * 2f, 2f);
				nr = UnityEngine.Random.Range(0f, 1f);
				
				if(nr <= 0.5f)
				{
					isNeg = true;
				}
				
				if(foo >= YLikelihood * 2f && foo < (XLikelihood + YLikelihood) * 2f)
				{
					// Pick random length along X axis
					
					bar = UnityEngine.Random.Range(minX, maxX);
					
					if(isNeg)
					{
						bar *= -1f;
					}
					
					outVector = bar * Vector3.right;
				}
				
				else
				{
					// Pick random length along Z axis
					
					bar = UnityEngine.Random.Range(minZ, maxZ);
					
					if(isNeg)
					{
						bar *= -1f;
					}
					
					outVector = bar * Vector3.forward;
				}
				
				attemptCounter++;
				// Debug.Log ("outVector: " + outVector);
			}
			while(PositionFails(lastNode, new Node(lastNode.position + outVector, outVector, lastNode), nodesSoFar) && attemptCounter <= maxChecks - 1);
		}
		
		else
		{
			do
			{
				foo = UnityEngine.Random.Range (ZLikelihood * 2f, 2f);
				nr = UnityEngine.Random.Range(0f, 1f);
				
				if(nr <= 0.5f)
				{
					isNeg = true;
				}
				
				if(foo >= ZLikelihood * 2f && foo < (XLikelihood + ZLikelihood) * 2f)
				{
					// Pick random length along X axis
					
					bar = UnityEngine.Random.Range(minX, maxX);
					
					if(isNeg)
					{
						bar *= -1f;
					}
					
					outVector = bar * Vector3.right;
				}
				
				else
				{
					// Pick random length along Y axis
					
					bar = UnityEngine.Random.Range(minY, maxY);
					
					if(isNeg)
					{
						bar *= -1f;
					}
					
					outVector = bar * Vector3.up;
				}
				
				attemptCounter++;
				// Debug.Log ("outVector: " + outVector);
			}
			while(PositionFails(lastNode, new Node(lastNode.position + outVector, outVector, lastNode), nodesSoFar) && attemptCounter <= maxChecks - 1);
		}
		
		// Create new node and update previous node's stuff
		
		if(attemptCounter != maxChecks)
		{
			Node nextNode = new Node(lastNode.position + outVector, outVector, lastNode);
			
			lastNode.nextDirection = outVector;
			lastNode.nextNode = nextNode;
			
			// When testing/debugging, show waypoints
			
			if(waypointsVisible)
			{
				GameObject nextPoint = Instantiate<GameObject>(waypoint);
				nextPoint.transform.position = lastNode.position + outVector;
			}
			
			return nextNode;
		}
	
		else
		{
			Debug.Log ("No node found.");
			
			// Stop node search from continuing
			
			shouldContinueWithNodes = false;
			
			return null;
		}
	}
	
	bool PositionFails(Node lastNode, Node nextNode, List<Node> nodesSoFar)
	{
		// Check if the new node is within the map boundaries
		
		if(!IsWithinWalls (nextNode.position))
		{
			return true;
		}
	
		// Check if the new node is too close to any other node
	
		foreach(Node x in nodesSoFar)
		{
			// Debug.Log("Code executing.");
			
			// Temporary fix - in the future I would like this to check whether the two paths between nodes are too close to each other
			
			float minDistance = Vector3.Distance (x.position, nextNode.position);
			
			// Debug.Log ("Min distance: " + minDistance);
			
			if(minDistance <= minSep)
			{
				// Debug.Log ("Min Distance: " + minDistance);
				
				return true;
			} 	
		}
		
		// Debug.Log ("Returning false.");
	
		return false;
	}
	
	bool IsWithinWalls(Vector3 position)
	{
		if(position.x >= negativeXWall.position.x + borderBuffer && position.x <= positiveXWall.position.x - borderBuffer)
		{
			if(position.y >= bottomWall.position.y + borderBuffer && position.y <= topWall.position.y - borderBuffer)
			{
				if(position.z >= negativeZWall.position.z + borderBuffer && position.z <= positiveZWall.position.z - borderBuffer)
				{
					return true;
				}
			}
		}
		
		return false;
	}
}
