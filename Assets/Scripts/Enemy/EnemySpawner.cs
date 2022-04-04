using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private PlatformDetector detector;

	[SerializeField] private GameObject archerEnemy;

	[SerializeField] private float timeToSpawnNext;

	private void Start()
	{
		StartCoroutine(EnemySpawning());
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
			Instantiate(archerEnemy, spawnPos, Quaternion.identity);
		}
	}
}
