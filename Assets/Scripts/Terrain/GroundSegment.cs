using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSegment : MonoBehaviour
{
	public bool leftExists { get; set; }
	public bool rightExists { get; set; }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((!leftExists || !rightExists) && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			MapGenerator.instance.CreateGroundSegment(transform.parent, this);
			print("Player detected, generate next ground");
		}
	}
}
