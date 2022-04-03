using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSegment : MonoBehaviour
{
	public GroundSegment left { get; set; }
	public GroundSegment right { get; set; }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((left == null || right == null) && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			MapGenerator.instance.CreateGroundSegment(transform.parent, this);
			print("Player detected, generate next ground");
		}
	}

	public void DestroySegment()
	{
		if (left != null)
		{
			left.right = null;
		}
		if (right != null)
		{
			right.left = null;
		}

		Destroy(transform.parent.gameObject);
	}
}
