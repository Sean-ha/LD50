using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private PlatformDetector detector;

	[SerializeField] private GameObject archerEnemy;

	[SerializeField] private float timeToSpawnNextCluster;
	[SerializeField] private int spawnsPerCluster;

	private HashSet<BehavingEnemy> enemies = new HashSet<BehavingEnemy>();

	private Coroutine spawningCR;
	private Coroutine difficultyCR1, difficultyCR2;


	private void Start()
	{
		spawningCR = StartCoroutine(EnemySpawning());
		difficultyCR1 = StartCoroutine(RampUpDifficulty1());
		difficultyCR2 = StartCoroutine(RampUpDifficulty2());
	}

	private IEnumerator EnemySpawning()
	{
		while (true)
		{
			yield return new WaitForSeconds(timeToSpawnNextCluster);


			for (int i = 0; i < spawnsPerCluster; i++)
			{
				Vector2 spawnPos = detector.GetSpawnPoint();
				ObjectCreator.instance.CreateEnemySpawnParticles(spawnPos);
				ObjectCreator.instance.CreateExpandingExplosion(spawnPos, Color.black, 2f, 0.2f);
				SoundManager.instance.PlayPositionedOneShot(SoundManager.Sound.EnemySpawn, spawnPos);
				GameObject enemy = Instantiate(archerEnemy, spawnPos, Quaternion.identity);

				var behav = enemy.GetComponentInChildren<BehavingEnemy>();

				behav.myHealthSystem.onDeath.AddListener(() => enemies.Remove(behav));

				enemies.Add(behav);

				yield return new WaitForSeconds(0.15f);
			}
		}
	}

	// Shortens spawn timer
	private IEnumerator RampUpDifficulty1()
	{
		while (true)
		{
			yield return new WaitForSeconds(10f);

			// Will reach shortest time in 270 secs
			timeToSpawnNextCluster = Mathf.Max(timeToSpawnNextCluster - 0.1f, 0.8f);
		}
	}

	// Increases number of spawns per cluster
	private IEnumerator RampUpDifficulty2()
	{
		while (true)
		{
			yield return new WaitForSeconds(30f);

			spawnsPerCluster += 1;
		}
	}

	public void StopSpawning()
	{
		if (spawningCR != null)
			StopCoroutine(spawningCR);

		if (difficultyCR1 != null)
			StopCoroutine(difficultyCR1);

		if (difficultyCR2 != null)
			StopCoroutine(difficultyCR2);
	}

	public void KillAllBehaviors()
	{
		foreach (BehavingEnemy e in enemies)
		{
			e.KillBehavior();
		}
	}
}
