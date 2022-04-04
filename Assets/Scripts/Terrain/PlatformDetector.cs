using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
	private int platformLayer;

	private HashSet<Platform> availablePlatforms = new HashSet<Platform>();

	private void Awake()
	{
		platformLayer = LayerMask.NameToLayer("Platform");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == platformLayer)
		{
			availablePlatforms.Add(collision.GetComponent<Platform>());
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == platformLayer)
		{
			availablePlatforms.Remove(collision.GetComponent<Platform>());
		}
	}

	public Vector2 GetSpawnPoint()
	{
		Platform element = availablePlatforms.ElementAt(Random.Range(0, availablePlatforms.Count));

		return element.GetRandomPositionOnPlatform();
	}
}
