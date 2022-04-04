using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuitButton : MonoBehaviour
{
	[SerializeField] private Transform line;

	private Tween tween;

	private void OnMouseDown()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void OnMouseEnter()
	{
		SoundManager.instance.PlayOneShot(SoundManager.Sound.Hover, randomizePitch: false);

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
