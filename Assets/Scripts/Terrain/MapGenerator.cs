using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public static MapGenerator instance;

	[SerializeField] private GameObject groundObject;
	[SerializeField] private GameObject platformObject;

	// The lowest and highest y-pos a platform could possible be at
	private int minYHeight = 0;
	private int maxYHeight = 30;

	private void Awake()
	{
		instance = this;
	}

	// Source = current player's ground segment. Generate the new ground to the left/right of it
	public void CreateGroundSegment(Transform source, GroundSegment segment)
	{
		// Generate right or left side (or both)
		if (segment.right == null)
		{
			Vector2 nextGroundPos = (Vector2)source.position + new Vector2(source.localScale.x - 1, 0);
			GameObject rightGround = Instantiate(groundObject, nextGroundPos, Quaternion.identity);
			GroundSegment rightSegment = rightGround.transform.GetChild(0).GetComponent<GroundSegment>();
			rightSegment.left = segment;

			GeneratePlatforms(rightGround.transform);

			segment.right = rightSegment;
		}
		if (segment.left == null)
		{
			Vector2 nextGroundPos = (Vector2)source.position - new Vector2(source.localScale.x - 1, 0);
			GameObject leftGround = Instantiate(groundObject, nextGroundPos, Quaternion.identity);
			GroundSegment leftSegment = leftGround.transform.GetChild(0).GetComponent<GroundSegment>();
			leftSegment.right = segment;

			GeneratePlatforms(leftGround.transform);

			segment.left = leftSegment;
		}
	}

	public void GeneratePlatforms(Transform source)
	{
		float leftXPos = source.position.x - source.localScale.x / 2f;

		// Generate left aligned platforms with varying heights (and x offsets)
		for (int i = 0; i < 5; i++)
		{
			float xPos = leftXPos + Random.Range(0, 11);
			// [0-5]; [6-11]; [12-17]; [18-23]; [24-29]
			float yPos = Random.Range(minYHeight + (6 * i), minYHeight + 5 + (6 * i));

			CreatePlatformRecursive(source, new Vector2(xPos, yPos), source.position.x + source.localScale.x / 2);
		}
	}

	private void CreatePlatformRecursive(Transform parent, Vector2 start, float endpointX)
	{
		if (start.x >= endpointX)
			return;

		Transform left = CreatePlatform(start);
		left.parent = parent;

		int gap = Random.Range(5, 11);

		int heightDiff = Random.Range(-4, 5);
		Vector2 nextPos = new Vector2(start.x + left.localScale.x + gap, start.y + heightDiff);
		
		nextPos.y = Mathf.Clamp(minYHeight, (int)nextPos.y, maxYHeight);
		CreatePlatformRecursive(parent, nextPos, endpointX);
	}

	private Transform CreatePlatform(Vector2 position)
	{
		Transform platformlatform = Instantiate(platformObject, position, Quaternion.identity).transform;

		float platformScale = Random.Range(7, 26);
		platformlatform.localScale = new Vector2(platformScale, platformlatform.localScale.y);

		return platformlatform;
	}
}
