using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	[SerializeField] private float parallaxMagnitude;

	[SerializeField] private Transform myLeft;
	[SerializeField] private Transform myMiddle;
	[SerializeField] private Transform myRight;

	private Camera cam;

	private float startPos;
	private float currPos;
	private int length = 60;

	private Transform l, m, r;

	private void Start()
	{
		cam = Camera.main;
		l = myLeft;
		m = myMiddle;
		r = myRight;
	}

	private void Update()
	{
		float temp = cam.transform.position.x * (1 - parallaxMagnitude);

		float dist = cam.transform.position.x * parallaxMagnitude;

		transform.position = new Vector2(startPos + dist, transform.position.y);

		if (temp > currPos + length)
		{
			l.position = new Vector2(r.position.x + length, myLeft.position.y);
			
			// Adjust left right and mid so they are still correct
			var tmp = r;
			r = l;
			l = m;
			m = tmp;

			currPos += length;
		}
			
		else if (temp < currPos - length)
		{
			r.position = new Vector2(l.position.x - length, myRight.position.y);

			// Adjust left right and mid so they are still correct
			var tmp = m;
			m = l;
			l = r;
			r = tmp;

			currPos -= length;
		}
	}
}
