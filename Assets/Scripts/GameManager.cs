using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private bool isPaused;

	private void Start()
	{
		StartCoroutine(ManageHealthLossStat());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
			{
				Time.timeScale = 1;
				isPaused = false;
			}
			else
			{
				Time.timeScale = 0;
				isPaused = true;
			}
		}
	}

	private IEnumerator ManageHealthLossStat()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);

			GameStats.healthLostPerSecond += 0.1f;

			// This will occur after 200 seconds
			if (GameStats.healthLostPerSecond >= 6f)
				yield break;
		}
	}
}
