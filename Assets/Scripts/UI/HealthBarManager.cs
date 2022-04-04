using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
	private SpriteRenderer barRenderer;

	private float maxWidth;

	private void Awake()
	{
		barRenderer = GetComponent<SpriteRenderer>();
		maxWidth = barRenderer.size.x;
	}

	public void UpdateHealthBar(float currHealth, float maxHealth)
	{
		float ratio = currHealth / maxHealth;

		barRenderer.size = new Vector2(ratio * maxWidth, barRenderer.size.y);
	}
}
