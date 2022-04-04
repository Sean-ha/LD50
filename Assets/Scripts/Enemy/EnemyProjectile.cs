using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyProjectile : MonoBehaviour
{
	private Rigidbody2D rb;

	private float damage;
	private float speed;
	private Vector2 direction;

	private float timeAlive;

	private Tween myTweens;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void SetupProjectile(float dangle, float damage, float speed)
	{
		this.damage = damage;
		this.speed = speed;
		transform.rotation = Quaternion.Euler(0, 0, dangle);

		float rangle = Mathf.Deg2Rad * dangle;

		direction = new Vector2(Mathf.Cos(rangle), Mathf.Sin(rangle)).normalized;


		DoSquashAndStretch();
	}

	private void DoSquashAndStretch()
	{
		transform.localScale = new Vector3(1.5f, 0.5f, 1);
		transform.DOScale(1, 0.25f);
	}

	private void FixedUpdate()
	{
		rb.velocity = direction * speed;

		timeAlive += Time.fixedDeltaTime;
		// Bullet destroy itself after some seconds
		if (timeAlive >= 7)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			collision.GetComponent<HealthSystem>().TakeDamage(damage);

			// Hit effects
			Vector2 pos = collision.ClosestPoint(transform.position);
			ObjectCreator.instance.CreateExpandingExplosion(pos, Color.red, 2f);
			ObjectCreator.instance.CreatePlayerBloodParticles(pos);

			SoundManager.instance.PlayOneShot(SoundManager.Sound.PlayerHurt, createNewSource: true);

			Destroy(gameObject);
		}
	}
}
