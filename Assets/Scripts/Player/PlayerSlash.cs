using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
	private PlayerController source;

	private float damage;

	public void SetupSlash(PlayerController player, float damage)
	{
		source = player;
		this.damage = damage;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("EnemySolid"))
		{
			collision.GetComponent<HealthSystem>()?.TakeDamage(damage);
			ObjectCreator.instance.CreateHitParticles(collision.transform.position);
			ObjectCreator.instance.CreateExpandingExplosion(collision.transform.position, Color.white, 2f, duration: 0.25f);

			SoundManager.instance.PlayOneShot(SoundManager.Sound.PlayerAttackHits, createNewSource: true);
		}
	}
}
