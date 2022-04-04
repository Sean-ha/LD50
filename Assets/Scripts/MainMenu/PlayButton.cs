using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
	[SerializeField] private SpriteRenderer blackScreen;

	[SerializeField] private Transform line;

	[SerializeField] private ParticleSystem playParticles;

	private bool clickable = true;

	private Tween tween;

	private void OnMouseDown()
	{
		if (!clickable)
			return;

		Transform t = ObjectCreator.instance.CreateHitParticles(transform.position).transform;
		t.parent = Camera.main.transform;
		t = ObjectCreator.instance.CreateExpandingExplosion(transform.position, Color.white, 4f, 0.2f).transform;
		t.parent = Camera.main.transform;

		playParticles.Play();

		SoundManager.instance.PlayOneShot(SoundManager.Sound.PlayerAttackHits);

		blackScreen.DOColor(Color.black, 0.75f).OnComplete(() =>
		{
			SceneManager.LoadScene("SampleScene");
		});

		clickable = false;
	}

	private void OnMouseEnter()
	{
		SoundManager.instance.PlayOneShot(SoundManager.Sound.Hover);

		tween.Kill();
		tween = line.DOScaleX(9f, 0.1f).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			tween = line.DOScaleX(8f, 0.1f).SetEase(Ease.InOutQuad);
		});
	}

	private void OnMouseExit()
	{
		tween.Kill();
		tween = line.DOScaleX(0, 0.1f).SetEase(Ease.InOutQuad);
	}
}
