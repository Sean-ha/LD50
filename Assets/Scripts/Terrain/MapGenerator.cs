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
		if (!segment.rightExists)
		{
			Vector2 nextGroundPos = (Vector2)source.position + new Vector2(source.localScale.x - 1, 0);
			GameObject rightGround = Instantiate(groundObject, nextGroundPos, Quaternion.identity);
			rightGround.transform.GetChild(0).GetComponent<GroundSegment>().leftExists = true;

			GeneratePlatforms(rightGround.transform);

			segment.rightExists = true;
		}
		if (!segment.leftExists)
		{
			Vector2 nextGroundPos = (Vector2)source.position - new Vector2(source.localScale.x - 1, 0);
			GameObject leftGround = Instantiate(groundObject, nextGroundPos, Quaternion.identity);
			leftGround.transform.GetChild(0).GetComponent<GroundSegment>().rightExists = true;

			GeneratePlatforms(leftGround.transform);

			segment.leftExists = true;
		}
	}

	public void GeneratePlatforms(Transform source)
	{
		float leftXPos = source.position.x - source.localScale.x / 2f;

		// Leftmost platform will be the lowest
		// TODO: Might not clamp to pixel units, might be an issue where player is raised by 1 pixel
		float lowYPos = Random.Range(minYHeight, minYHeight);
		Transform lowPlatform = CreatePlatform(new Vector2(leftXPos, lowYPos));

		// Generate left aligned platforms with varying heights (and x offsets)
		for (int i = 0; i < 4; i++)
		{
			float xPos = leftXPos + Random.Range(0, 11);
			// [4-9]; [10-15]; [16-21]; [22-27]
			float yPos = Random.Range(minYHeight + 4 + (6 * i), minYHeight + 9 + (6 * i));

			CreatePlatformRecursive(new Vector2(xPos, yPos), source.position.x + source.localScale.x / 2);
		}
	}

	private void CreatePlatformRecursive(Vector2 start, float endpointX)
	{
		if (start.x >= endpointX)
			return;

		Transform left = CreatePlatform(start);

		int gap = Random.Range(2, 11);
		int heightDiff = Random.Range(-4, 5);
		Vector2 nextPos = new Vector2(start.x + left.localScale.x + gap, start.y + heightDiff);
		
		nextPos.y = Mathf.Clamp(minYHeight, (int)nextPos.y, maxYHeight);
		CreatePlatformRecursive(nextPos, endpointX);
	}

	private Transform CreatePlatform(Vector2 position)
	{
		Transform platformlatform = Instantiate(platformObject, position, Quaternion.identity).transform;

		float platformScale = Random.Range(7, 26);
		platformlatform.localScale = new Vector2(platformScale, platformlatform.localScale.y);

		return platformlatform;
	}
}
