using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour, IClickable
{
	[SerializeField] private SpriteRenderer blackScreen;

	private bool clickable = true;

	public void OnClick()
	{
		if (!clickable)
			return;

		ObjectCreator.instance.CreateExpandingExplosionSortingLayer(transform.position, Color.white, 4f, "DeathEffects", 99, 0.3f);
		GameObject hitfx = ObjectCreator.instance.CreateHitParticles(transform.position);
		var rend = hitfx.GetComponent<ParticleSystemRenderer>();
		rend.sortingLayerName = "DeathEffects";
		rend.sortingOrder = 0;

		SoundManager.instance.PlayOneShot(SoundManager.Sound.PlayerAttackHits);

		blackScreen.DOColor(Color.black, 0.75f).OnComplete(() =>
		{
			SceneManager.LoadScene("MainMenu");
		});

		clickable = false;
	}
}
