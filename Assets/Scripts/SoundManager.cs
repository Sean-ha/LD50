using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public enum Sound
	{
		Step = 0,
		Jump = 1,
		DoubleJump = 2,
		Land = 3,
		PlayerSwing = 4,
		PlayerAttackHits = 5,
	}

	[System.Serializable]
	public class SoundAndClip
	{
		public Sound sound;
		public AudioClip clip;
	}

	public static SoundManager instance;

	[SerializeField] private List<SoundAndClip> sounds;

	private Dictionary<Sound, AudioClip> soundDict = new Dictionary<Sound, AudioClip>();

	private AudioSource mySource;

	private float sfxVolume = 0.25f;

	private void Awake()
	{
		// Build dictionary from list
		foreach (SoundAndClip sac in sounds)
		{
			soundDict.Add(sac.sound, sac.clip);
		}
		instance = this;

		GameObject soundObj = new GameObject();
		mySource = soundObj.AddComponent<AudioSource>();
	}

	public void PlayOneShot(Sound sound, bool randomizePitch = true, float volumeDelta = 0)
	{
		if (randomizePitch)
		{
			mySource.pitch = Random.Range(0.9f, 1.1f);
		}
		else
		{
			mySource.pitch = 1f;
		}

		mySource.volume = sfxVolume + volumeDelta;

		mySource.PlayOneShot(soundDict[sound]);
	}
}
