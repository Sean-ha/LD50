using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
	[SerializeField] private Transform toFollow;

	private Vector2 offset;

	private void Start()
	{
		offset = transform.position;
	}

	private void Update()
	{
		transform.position = (Vector2)toFollow.position + offset;
	}
}
