using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
	private void OnEnable()
	{
		DOVirtual.DelayedCall(0.1f, () => gameObject.SetActive(false));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
		{
			// If platform that collided with you is bigger, then destroy self
			if (collision.transform.localScale.x >= transform.parent.localScale.x)
				Destroy(transform.parent.gameObject);
		}
	}
}
