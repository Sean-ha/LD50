using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerWrapper : MonoBehaviour
{
	[SerializeField] PlayerController pc;

	public void FinishAttack()
	{
		pc.OnAttackFinish();
	}

	public void CreateSlash()
	{
		pc.CreateSlash();
	}

	public void CreateRunParticles()
	{
		pc.CreateRunParticles();
	}

	public void PlayFootstepSound()
	{
		SoundManager.instance.PlayOneShot(SoundManager.Sound.Step);
	}
}
