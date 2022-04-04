using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
	[SerializeField] private SpriteRenderer blackScreen;

	private bool clickable = true;

	private void OnMouseDown()
	{
		if (!clickable)
			return;

		Transform t = ObjectCreator.instance.CreateHitParticles(transform.position).transform;
		t.parent = Camera.main.transform;
		t = ObjectCreator.instance.CreateExpandingExplosion(transform.position, Color.white, 4f, 0.2f).transform;
		t.parent = Camera.main.transform;

		SoundManager.instance.PlayOneShot(SoundManager.Sound.PlayerAttackHits);

		blackScreen.DOColor(Color.black, 0.75f).OnComplete(() =>
		{
			SceneManager.LoadScene("SampleScene");
		});

		clickable = false;
	}
}
