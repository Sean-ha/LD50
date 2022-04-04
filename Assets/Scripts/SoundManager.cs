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
		ReverseCymbal = 6,
		ArrowImpact = 7,
		PlayerHurt = 8,
		EnemySpawn = 9,
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
	private Transform cam;

	// For very short clips only
	private AudioSource randomPitchSource;

	private float sfxVolume = 0.25f;

	private void Awake()
	{
		instance = this;
		// Build dictionary from list
		foreach (SoundAndClip sac in sounds)
		{
			soundDict.Add(sac.sound, sac.clip);
		}

		GameObject soundObj = new GameObject();
		randomPitchSource = soundObj.AddComponent<AudioSource>();
		cam = Camera.main.transform;
	}

	public void PlayOneShot(Sound sound, bool createNewSource = false, bool randomizePitch = true, float volumeDelta = 0)
	{
		if (createNewSource)
		{
			AudioClip clip = soundDict[sound];

			GameObject newSource = new GameObject();
			AudioSource src = newSource.AddComponent<AudioSource>();
			src.volume = sfxVolume + volumeDelta;

			if (randomizePitch)
				src.pitch = Random.Range(0.9f, 1.1f);

			src.PlayOneShot(clip);

			Destroy(newSource, clip.length);

			return;
		}


		if (randomizePitch)
		{
			randomPitchSource.pitch = Random.Range(0.9f, 1.1f);
		}

		randomPitchSource.volume = sfxVolume + volumeDelta;

		randomPitchSource.PlayOneShot(soundDict[sound]);
	}

	// Play sound at a position. Adjust its volume based on its distance
	public void PlayPositionedOneShot(Sound sound, Vector2 pos, bool randomizePitch = true)
	{
		float dist = Vector2.Distance(pos, cam.position);
		// 0-1 based on distance
		float vol;

		float minDist = 11f;
		float maxDist = 60f;
		if (dist < minDist)
		{
			vol = 1;
		}
		else if (dist > maxDist)
		{
			vol = 0;
		}
		else
		{
			vol = 1 - ((dist - minDist) / (maxDist - minDist));
		}

		vol *= sfxVolume;
		float volumeDelta = vol - sfxVolume;

		PlayOneShot(sound, createNewSource: true, randomizePitch: randomizePitch, volumeDelta: volumeDelta);
	}
}
