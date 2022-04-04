using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthWrapper : MonoBehaviour
{
	public static PlayerHealthWrapper instance;

	private HealthSystem myHealthSystem;

	private void Awake()
	{
		instance = this;
		myHealthSystem = GetComponent<HealthSystem>();
	}

	private void Start()
	{
		StartCoroutine(BleedingOut());
	}

	public void GainHealth(float amount)
	{
		myHealthSystem.GainHealth(amount);
	}

	private IEnumerator BleedingOut()
	{
		while (true)
		{
			float ticksPerSecond = 5f;
			yield return new WaitForSeconds(1f / ticksPerSecond);
			myHealthSystem.TakeDamage(GameStats.healthLostPerSecond / ticksPerSecond);
		}
	}
}
