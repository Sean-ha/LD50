using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
	[SerializeField] private float maxHealth;

	// Params: currHealth, maxHealth
	[SerializeField] private UnityEvent<float, float> onTakeDamage;
	[SerializeField] private UnityEvent<float, float> onGainHealth;

	[SerializeField] private UnityEvent onDeath;
	

	[SerializeField] private bool destroyOnDeath;

	private float currHealth;


	private void Start()
	{
		currHealth = maxHealth;
	}

	public void TakeDamage(float amount)
	{
		currHealth -= amount;

		if (currHealth < 0)
			currHealth = 0;

		onTakeDamage.Invoke(currHealth, maxHealth);

		if (currHealth <= 0)
		{
			onDeath.Invoke();

			if (destroyOnDeath)
				Destroy(gameObject);
		}
	}

	public void GainHealth(float amount)
	{
		currHealth += amount;

		if (currHealth > maxHealth)
			currHealth = maxHealth;

		onGainHealth.Invoke(currHealth, maxHealth);
	}

	// TODO: Move these elsewhere probably
	public void RestorePlayerHealth()
	{
		PlayerHealthWrapper.instance.GainHealth(GameStats.healthRestorePerKill);
	}

	public void RestorePlayerHealthToFull()
	{
		PlayerHealthWrapper.instance.GainHealth(999);
	}

	public void CreateBloodParticles()
	{
		ObjectCreator.instance.CreateBloodParticles(transform.position);
	}

	public void UnparentObject(Transform trans)
	{
		trans.parent = null;
	}
}
