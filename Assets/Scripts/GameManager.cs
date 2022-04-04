using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[SerializeField] private SpriteRenderer fadeInBlack;
	[SerializeField] private TextTyper startTextTyper;

	[SerializeField] private SpriteRenderer solidBG;
	[SerializeField] private EnemySpawner spawner;
	[SerializeField] private PlayerHealthWrapper healthWrapper;
	[SerializeField] private TextTyper deathText;
	[SerializeField] private TextMeshPro summaryText;
	[SerializeField] private GameObject continueButton;

	[SerializeField] private GameObject playerCape1;
	[SerializeField] private GameObject playerCape2;

	public int enemiesSlain { get; set; } = 0;
	public int secondsSurvived { get; set; } = 0;


	private PlayerController player;

	private Coroutine healthLossCR;
	private Coroutine timeSurvivedCR;

	private void Awake()
	{
		instance = this;

		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		player = PlayerController.instance;

		fadeInBlack.color = Color.black;

		// Disable game systems
		PlayerController.instance.gameObject.SetActive(false);

		DOVirtual.DelayedCall(0.5f, () =>
		{
			startTextTyper.gameObject.SetActive(true);
			startTextTyper.DisplayString(() =>
			{
				StartCoroutine(FadeIn());
			});
		}, ignoreTimeScale: false);
	}

	private IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(0.75f);

		PlayerController.instance.gameObject.SetActive(true);
		fadeInBlack.DOColor(new Color(0, 0, 0, 0), 0.75f);

		yield return new WaitForSeconds(1f);

		startTextTyper.GetComponent<TextMeshPro>().DOColor(new Color(0, 0, 0, 0), 0.75f);

		yield return new WaitForSeconds(1f);

		StartGame();
	}

	// Actually begins the game systems
	private void StartGame()
	{
		healthLossCR = StartCoroutine(ManageHealthLossStat());
		timeSurvivedCR = StartCoroutine(TimeSurvived());
		spawner.StartSpawning();
		healthWrapper.StartBleeding();
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

	private IEnumerator TimeSurvived()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);

			secondsSurvived += 1;
		}
	}

	public void StartDeathSequence()
	{
		Time.timeScale = 0.5f;

		DOVirtual.DelayedCall(1f, () => Time.timeScale = 1f, ignoreTimeScale: true);

		StopCoroutine(healthLossCR);
		StopCoroutine(timeSurvivedCR);
		healthWrapper.StopBleedingOut();

		// Disable player inputs
		PlayerController.instance.DisableSelfOnDeath();
		playerCape1.SetActive(false);
		playerCape2.SetActive(false);

		spawner.StopSpawning();
		spawner.KillAllBehaviors();

		ObjectCreator.instance.CreateExpandingExplosionSortingLayer(player.transform.position, Color.red, 4f, "DeathEffects", 99, 0.3f);
		GameObject blood = ObjectCreator.instance.CreatePlayerBloodParticles(player.transform.position);
		var rend = blood.GetComponent<ParticleSystemRenderer>();
		rend.sortingLayerName = "DeathEffects";
		rend.sortingOrder = 0;
		var bloodCol = blood.GetComponent<ParticleSystem>().collision;
		bloodCol.enabled = false;

		solidBG.DOColor(Color.white, 0.1f).SetEase(Ease.InQuad).SetUpdate(true);

		DOVirtual.DelayedCall(1.15f, () =>
		{
			solidBG.DOColor(Color.black, 1.5f).OnComplete(() =>
			{
				DOVirtual.DelayedCall(1f, () => 
				{
					deathText.gameObject.SetActive(true);
					deathText.DisplayString(() =>
					{
						DOVirtual.DelayedCall(0.5f, () => StartCoroutine(ShowSummary()));
					});
				});
			});
		}, ignoreTimeScale: false);

		
	}

	private IEnumerator ShowSummary()
	{
		summaryText.gameObject.SetActive(true);
		summaryText.text = "";

		string text = "summary";
		UpdateSummaryText(text);

		yield return new WaitForSeconds(0.35f);

		text += "\nenemies slain: " + enemiesSlain.ToString();
		UpdateSummaryText(text);

		yield return new WaitForSeconds(0.35f);

		TimeSpan time = TimeSpan.FromSeconds(secondsSurvived);
		text += "\ntime survived: " + time.ToString(@"mm\:ss");
		UpdateSummaryText(text);

		yield return new WaitForSeconds(0.35f);

		int oldHighScore = PlayerPrefs.GetInt("HighScore", 0);

		int score = 100 * enemiesSlain + 25 * secondsSurvived;
		text += "\n\nscore: " + score;

		if (score > oldHighScore)
		{
			text += " (new high score)";
			PlayerPrefs.SetInt("HighScore", score);
		}

		UpdateSummaryText(text);

		yield return new WaitForSeconds(0.35f);

		continueButton.SetActive(true);
		SoundManager.instance.PlayOneShot(SoundManager.Sound.TypeSound);
	}

	private void UpdateSummaryText(string text)
	{
		summaryText.text = text;
		SoundManager.instance.PlayOneShot(SoundManager.Sound.TypeSound);
	}
}
