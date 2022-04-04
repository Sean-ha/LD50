using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
	private Transform player;

	private void Start()
	{
		player = PlayerController.instance.transform;
	}

	private void Update()
	{
		// Get angle between enemy and player location
		Vector2 diff = (Vector2)player.position - (Vector2)transform.position;

		if (diff.x > 0)
			transform.localScale = new Vector3(1, 1, 1);
		else if (diff.x < 0)
			transform.localScale = new Vector3(-1, 1, 1);
	}
}
