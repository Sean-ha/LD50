using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	[SerializeField] private float spinsRate;

	private float currDangle;

	private void Update()
	{
		currDangle += spinsRate * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0, 0, currDangle);
	}
}
