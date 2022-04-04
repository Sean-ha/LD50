using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAdjustButton : MonoBehaviour
{
	[SerializeField] private float incrementAmount;

	private void OnMouseDown()
	{
		SoundManager.instance.AdjustVolume(incrementAmount);

		SoundManager.instance.PlayOneShot(SoundManager.Sound.TypeSound);
	}
}
