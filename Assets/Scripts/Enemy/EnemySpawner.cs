using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private PlatformDetector detector;

	[SerializeField] private GameObject archerEnemy;

	[SerializeField] private float timeToSpawnNext;

	private HashSet<BehavingEnemy> enemies = new HashSet<BehavingEnemy>();

	private Coroutine spawningCR;

	private void Start()
	{
		spawningCR = StartCoroutine(EnemySpawning());
	}

	private IEnumerator EnemySpawning()
	{
		while (true)
		{
			yield return new WaitForSeconds(timeToSpawnNext);

			Vector2 spawnPos = detector.GetSpawnPoint();
			ObjectCreator.instance.CreateEnemySpawnParticles(spawnPos);
			ObjectCreator.instance.CreateExpandingExplosion(spawnPos, Color.black, 2f, 0.2f);
			SoundManager.instance.PlayPositionedOneShot(SoundManager.Sound.EnemySpawn, spawnPos);
			GameObject enemy = Instantiate(archerEnemy, spawnPos, Quaternion.identity);

			var behav = enemy.GetComponentInChildren<BehavingEnemy>();

			behav.myHealthSystem.onDeath.AddListener(() => enemies.Remove(behav));

			enemies.Add(behav);
		}
	}

	public void StopSpawning()
	{
		if (spawningCR != null)
			StopCoroutine(spawningCR);
	}

	public void KillAllBehaviors()
	{
		foreach (BehavingEnemy e in enemies)
		{
			e.KillBehavior();
		}
	}
}
