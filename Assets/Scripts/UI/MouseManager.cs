using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("MyUI"));

			if (hit != null)
			{
				IClickable clickable = hit.transform.GetComponent<IClickable>();
				if (clickable != null)
				{
					clickable.OnClick();
				}
			}
		}
	}
}
