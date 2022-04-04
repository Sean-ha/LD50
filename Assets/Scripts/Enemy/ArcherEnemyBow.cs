using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemyBow : MonoBehaviour
{
	[SerializeField] private GameObject arrowObject;
	[SerializeField] private float shootCooldown;
	[SerializeField] private float arrowSpeed;
	[SerializeField] private float arrowDamage;

	[SerializeField] private Animator myAnimator;
	[SerializeField] private ParticleSystem readyParticles;
	[SerializeField] private ParticleSystem shootParticles;

	private void Start()
	{
		StartCoroutine(ShootingBehavior());
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
		GameObject proj = Instantiate(arrowObject, transform.position, Quaternion.identity);
		proj.GetComponent<EnemyProjectile>().SetupProjectile(transform.rotation.eulerAngles.z, arrowDamage, arrowSpeed);
	}
}
