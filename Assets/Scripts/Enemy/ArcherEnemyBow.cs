using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemyBow : BehavingEnemy
{
	[SerializeField] private GameObject arrowObject;
	[SerializeField] private float shootCooldown;
	[SerializeField] private float arrowSpeed;
	[SerializeField] private float arrowDamage;

	[SerializeField] private Animator myAnimator;
	[SerializeField] private ParticleSystem readyParticles;
	[SerializeField] private ParticleSystem shootParticles;

	private Coroutine behaviorCR;

	private HashSet<GameObject> myProjectiles = new HashSet<GameObject>();

	private void Start()
	{
		behaviorCR = StartCoroutine(ShootingBehavior());
	}

	private IEnumerator ShootingBehavior()
	{
		while (true)
		{
			yield return new WaitForSeconds(shootCooldown);

			readyParticles.Play();
			ObjectCreator.instance.CreateExpandingExplosion(transform.position, Color.red, 2f, 0.3f);
			SoundManager.instance.PlayPositionedOneShot(SoundManager.Sound.ReverseCymbal, transform.position);
			myAnimator.Play("ArcherEnemyBowReady");

			yield return new WaitForSeconds(1f);

			shootParticles.Play();
			ObjectCreator.instance.CreateExpandingExplosion(transform.position, Color.red, 2f, 0.15f);
			SoundManager.instance.PlayPositionedOneShot(SoundManager.Sound.ArrowImpact, transform.position);
			ShootProjectile();
			myAnimator.Play("ArcherEnemyBowIdle");
		}
	}

	private void ShootProjectile()
	{
		GameObject projObj = Instantiate(arrowObject, transform.position, Quaternion.identity);
		EnemyProjectile proj = projObj.GetComponent<EnemyProjectile>();
		proj.OnDestroyProjectile((EnemyProjectile destroyedProj) => myProjectiles.Remove(destroyedProj.gameObject));
		proj.SetupProjectile(transform.rotation.eulerAngles.z, arrowDamage, arrowSpeed);
		myProjectiles.Add(projObj);
	}

	public override void KillBehavior()
	{
		if (behaviorCR != null)
			StopCoroutine(behaviorCR);

		foreach (GameObject o in myProjectiles)
		{
			Destroy(o);
		}

		myProjectiles.Clear();
	}
}
