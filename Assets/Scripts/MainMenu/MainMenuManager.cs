using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private SpriteRenderer blackScreen2;
	
	[SerializeField] private GameObject titleSR;
	[SerializeField] private SpriteRenderer swordSR;
	[SerializeField] private Sprite swordWhiteBar;
	[SerializeField] private Sprite swordWhiteSword;
	[SerializeField] private Sprite swordSprite;
	[SerializeField] private ParticleSystem swordSpawnParticles;

	[SerializeField] private GameObject playButton;
	[SerializeField] private GameObject quitButton;
	[SerializeField] private GameObject highScoreText;
	[SerializeField] private GameObject sfxAdjuster;

	private void Start()
	{
		StartCoroutine(StartMainMenu());
	}

	private IEnumerator StartMainMenu()
	{
		titleSR.SetActive(false);
		swordSR.gameObject.SetActive(false);
		playButton.SetActive(false);
		quitButton.SetActive(false);
		highScoreText.SetActive(false);
		sfxAdjuster.SetActive(false);

		blackScreen2.color = Color.black;

		blackScreen2.DOColor(new Color(0, 0, 0, 0), 0.65f);

		yield return new WaitForSeconds(1f);

		float yPos = titleSR.transform.localPosition.y;
		titleSR.transform.localPosition = new Vector3(titleSR.transform.localPosition.x, 16f, titleSR.transform.localPosition.z);
		titleSR.SetActive(true);

		titleSR.transform.DOLocalMoveY(yPos, 0.8f).SetEase(Ease.InOutQuad);

		yield return new WaitForSeconds(1.25f);

		SoundManager.instance.PlayOneShot(SoundManager.Sound.SwordUnsheath, createNewSource: true, randomizePitch: false);
		swordSR.sprite = swordWhiteBar;
		swordSR.gameObject.SetActive(true);
		CameraShake.instance.ShakeCamera(0.65f, 1.5f);

		yield return new WaitForSeconds(0.05f);

		swordSpawnParticles.Play();
		swordSR.sprite = swordWhiteSword;

		yield return new WaitForSeconds(0.075f);

		swordSR.sprite = swordSprite;

		yield return new WaitForSeconds(0.75f);

		playButton.SetActive(true);
		SoundManager.instance.PlayOneShot(SoundManager.Sound.TypeSound);

		yield return new WaitForSeconds(0.5f);

		quitButton.SetActive(true);
		SoundManager.instance.PlayOneShot(SoundManager.Sound.TypeSound);

		yield return new WaitForSeconds(0.5f);

		highScoreText.SetActive(true);
		SoundManager.instance.PlayOneShot(SoundManager.Sound.TypeSound);

		yield return new WaitForSeconds(0.5f);

		sfxAdjuster.SetActive(true);
		SoundManager.instance.PlayOneShot(SoundManager.Sound.TypeSound);
	}
}
