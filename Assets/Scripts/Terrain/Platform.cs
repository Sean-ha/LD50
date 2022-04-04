using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
	public Vector2 GetRandomPositionOnPlatform()
	{
		float left = transform.position.x;
		float right = transform.position.x + transform.lossyScale.x;

		float rand = Random.Range(left, right);

		return new Vector2(rand, transform.position.y);
	}
}
