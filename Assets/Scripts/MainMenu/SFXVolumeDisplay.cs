using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SFXVolumeDisplay : MonoBehaviour
{
	private TextMeshPro tmp;

	private void Awake()
	{
		tmp = GetComponent<TextMeshPro>();
	}

	private void Update()
	{
		string vol = string.Format("{0:0%}", SoundManager.instance.GetVolume());
		tmp.text = "sfx volume:\n" + vol;
	}
}
