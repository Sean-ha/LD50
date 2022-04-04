using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraPanner : MonoBehaviour
{
	[SerializeField] private float panSpeed;

	private PixelPerfectCamera ppc;

	private void Awake()
	{
		ppc = GetComponent<PixelPerfectCamera>();
	}

	private void Update()
	{
		Vector3 pos = transform.position;
		pos.x += panSpeed * Time.deltaTime;

		transform.position = pos;
	}

	private void LateUpdate()
	{
		transform.position = ppc.RoundToPixel(transform.position);
	}
}
