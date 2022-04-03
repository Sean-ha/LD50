using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
	[SerializeField] private float maxHealth;
	[SerializeField] private UnityEvent<float> onTakeDamage;

	private float currHealth;


	private void Start()
	{
		currHealth = maxHealth;
	}

	public void TakeDamage(float amount)
	{
		currHealth -= amount;

		onTakeDamage.Invoke(currHealth);

		print(currHealth);

		if (currHealth <= 0)
		{
			// Die
		}
	}
}
