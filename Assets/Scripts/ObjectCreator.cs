using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
	public static ObjectCreator instance;

	[SerializeField] private GameObject jumpParticles;
	[SerializeField] private GameObject doubleJumpParticles;
	[SerializeField] private GameObject landParticles;
	[SerializeField] private GameObject runParticles;
	[SerializeField] private GameObject hitParticles;
	[SerializeField] private GameObject expandingExplosion;
	[SerializeField] private GameObject bloodParticles;
	[SerializeField] private GameObject playerBloodParticles;
	[SerializeField] private GameObject enemySpawnParticles;


	private void Awake()
	{
		instance = this;
	}

	public GameObject CreateJumpParticles(Vector2 position)
	{
		GameObject created = Instantiate(jumpParticles, position, Quaternion.identity);

		return created;
	}

	public GameObject CreateDoubleJumpParticles(Vector2 position)
	{
		GameObject created = Instantiate(doubleJumpParticles, position, Quaternion.identity);

		return created;
	}

	public GameObject CreateLandParticles(Vector2 position)
	{
		GameObject created = Instantiate(landParticles, position, Quaternion.identity);

		return created;
	}

	public GameObject CreateRunParticles(Vector2 position, Vector2 scale)
	{
		GameObject created = Instantiate(runParticles, position, Quaternion.identity);
		created.transform.localScale = scale;

		return created;
	}

	public GameObject CreateHitParticles(Vector2 position)
	{
		GameObject created = Instantiate(hitParticles, position, Quaternion.identity);

		return created;
	}

	public GameObject CreateExpandingExplosion(Vector2 position, Color color, float radius, float duration = 0.15f)
	{
		GameObject created = Instantiate(expandingExplosion, position, Quaternion.identity);
		created.GetComponent<ExpandingExplosion>().SetExplosion(color, radius, explosionDuration: duration);

		return created;
	}

	public GameObject CreateBloodParticles(Vector2 position)
	{
		GameObject created = Instantiate(bloodParticles, position, Quaternion.identity);

		return created;
	}

	public GameObject CreatePlayerBloodParticles(Vector2 position)
	{
		GameObject created = Instantiate(playerBloodParticles, position, Quaternion.identity);

		return created;
	}

	public GameObject CreateEnemySpawnParticles(Vector2 position)
	{
		GameObject created = Instantiate(enemySpawnParticles, position, Quaternion.identity);

		return created;
	}
}
