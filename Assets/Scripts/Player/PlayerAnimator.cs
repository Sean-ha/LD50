using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimator : MonoBehaviour
{
	[SerializeField] private Animator animator;

	private PlayerController player;

	private PlayerController.PlayerState? currAnim;

	private bool attackLock;

	public void SetAnimation(PlayerController player, PlayerController.PlayerState newAnim)
	{
		this.player = player;
		if (currAnim == newAnim || attackLock)
			return;

		switch (newAnim)
		{
			case PlayerController.PlayerState.Idling:
				if (currAnim == PlayerController.PlayerState.Falling)
					DoLanding(false);
				else
					animator.Play("PlayerIdle");
				break;
			case PlayerController.PlayerState.Jumping:
				animator.Play("PlayerJump");
				break;
			case PlayerController.PlayerState.DoubleJumping:
				animator.Play("PlayerDoubleJump");
				break;
			case PlayerController.PlayerState.Running:
				if (currAnim == PlayerController.PlayerState.Falling)
					DoLanding(true);
				else
					animator.Play("PlayerRun");
				break;
			case PlayerController.PlayerState.Falling:
				animator.Play("PlayerFall");
				break;
			case PlayerController.PlayerState.Dead:
				animator.Play("PlayerDead");
				break;
		}

		currAnim = newAnim;
	}

	private void DoLanding(bool runAfterLand)
	{
		if (runAfterLand)
		{
			animator.Play("PlayerRun");
		}
		else
		{
			animator.Play("PlayerLand");
			animator.SetBool("runAfterLand", runAfterLand);
		}

		ObjectCreator.instance.CreateLandParticles(player.feetPos.position);

		SoundManager.instance.PlayOneShot(SoundManager.Sound.Land);
	}

	public void SetAttackAnimation()
	{
		animator.Play("PlayerAttack");
		attackLock = true;
	}

	public void ReleaseAttackAnimation()
	{
		attackLock = false;
		currAnim = null;
	}
}
