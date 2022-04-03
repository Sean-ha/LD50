using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private TextMeshPro debugText;
	[SerializeField] public Transform feetPos;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float jumpForce;
	[SerializeField] private float doubleJumpForce;
	[SerializeField] private float jumpLowForce;

	[SerializeField] private LayerMask groundLayers;

	[SerializeField] private Transform frontTransform;
	[SerializeField] private GameObject playerSlashObject;

	[SerializeField] private float attackCooldown;
	private float currAttackCooldown;
	[SerializeField] private float attackDamage;


	private Rigidbody2D rb;
	private PlayerAnimator myAnimator;
	private Collider2D myCol;

	private int maxJumps = 2;
	private int currJumps = 0;
	// Is player locked in jump state? (They are locked for a few frames after pressing jump)
	private bool jumpLock;
	private Coroutine jumpLockCR;

	private float horizontal;

	private bool isGrounded;
	// Platform you are currently standing on, or null if not on a platform
	private Collider2D onPlatform;

	private bool isAttacking;

	private Tween velocityOffsetTween;
	private Vector2 velocityOffset = Vector2.zero;

	private float jumpAssist;

	public enum PlayerState
	{
		Idling,
		Running,
		Jumping,
		DoubleJumping,
		Falling,
		Attacking
	}
	private PlayerState currState;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<PlayerAnimator>();
		myCol = GetComponent<Collider2D>();
	}

	private void Start()
	{
		Application.targetFrameRate = 60;
	}

	private void Update()
	{
		horizontal = Input.GetAxisRaw("Horizontal");
		isGrounded = IsGrounded();

		currAttackCooldown -= Time.deltaTime;
		jumpAssist -= Time.deltaTime;

		if (!isAttacking)
		{
			if (horizontal < 0)
				transform.localScale = new Vector3(-1, 1, 1);
			else if (horizontal > 0)
				transform.localScale = new Vector3(1, 1, 1);
		}

		if (currState == PlayerState.Idling || currState == PlayerState.Running)
			currJumps = 0;


		CheckState();


		// Release jump
		if (currState == PlayerState.Jumping || currState == PlayerState.DoubleJumping)
		{
			if (Input.GetKeyUp(KeyCode.Space))
			{
				if (rb.velocity.y > jumpLowForce)
				{
					rb.velocity = new Vector2(rb.velocity.x, jumpLowForce);
				}
			}
		}

		// Drop through platform
		if (onPlatform != null && isGrounded && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
		{
			Physics2D.IgnoreCollision(onPlatform, myCol);
			StartCoroutine(ReenableCollision(onPlatform));
		}

		myAnimator.SetAnimation(this, currState);

		DisplayDebugText();
	}

	private void CheckState()
	{
		if (!jumpLock)
		{
			// Determine state
			if (isGrounded)
			{
				if ((currState != PlayerState.Jumping && currState != PlayerState.DoubleJumping))
				{
					// Grounded and standing still / moving
					if (Mathf.Approximately(rb.velocity.x, 0))
						currState = PlayerState.Idling;
					else
						currState = PlayerState.Running;
				}

				// Grounded and press space
				if (Input.GetKeyDown(KeyCode.Space) && currJumps < maxJumps)
				{
					DoJump();
				}
			}
			else
			{
				if (rb.velocity.y < 0)
				{
					// Just ran off a platform, set jump assist time
					if (currState == PlayerState.Running)
					{
						jumpAssist = 0.1f;
					}

					currState = PlayerState.Falling;
				}

				if (Input.GetKeyDown(KeyCode.Space) && currJumps < maxJumps)
				{
					// Has jump assist, do a jump instead of double jump
					if (jumpAssist > 0)
					{
						DoJump();
					}
					// No jump assist, do double jump
					else
					{
						currState = PlayerState.DoubleJumping;
						rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
						currJumps = 2;
						SetJumpLock();

						ObjectCreator.instance.CreateDoubleJumpParticles(feetPos.position);
						SoundManager.instance.PlayOneShot(SoundManager.Sound.DoubleJump);
					}
				}
			}

			CheckAttackInput();
		}
		// Can attack even during jumpLock
		else
		{
			CheckAttackInput();
		}
		

		// Attack state overrides all other states
		if (isAttacking)
			currState = PlayerState.Attacking;
	}

	private void DoJump()
	{
		currState = PlayerState.Jumping;
		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		currJumps += 1;
		SetJumpLock();

		ObjectCreator.instance.CreateJumpParticles(feetPos.position);
		SoundManager.instance.PlayOneShot(SoundManager.Sound.Jump);
	}

	private void CheckAttackInput()
	{
		if (Input.GetKeyDown(KeyCode.J) && currAttackCooldown <= 0 && !isAttacking)
		{
			currState = PlayerState.Attacking;
			isAttacking = true;
			// AddVelocityOffset(new Vector2(3f, 0f) * transform.localScale.x);

			currAttackCooldown = attackCooldown;
			myAnimator.SetAttackAnimation();
		}
	}

	private void DisplayDebugText()
	{
		string debugStr = "grounded: " + isGrounded + "\ncurrJumps: " + currJumps + "\nState: " + currState.ToString();
		debugText.text = debugStr;
	}

	private void FixedUpdate()
	{
		Vector2 velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

		rb.velocity = velocity;
	}

	private bool IsGrounded()
	{
		Collider2D overlap = Physics2D.OverlapBox(feetPos.position, new Vector2(0.45f, 0.02f), 0f, groundLayers);
		

		if (overlap != null)
		{
			if (overlap.gameObject.layer == LayerMask.NameToLayer("Platform"))
				onPlatform = overlap;
			else
				onPlatform = null;

			return true;
		}
		else
		{
			onPlatform = null;

			return false;
		}
	}

	private void SetJumpLock()
	{
		if (jumpLockCR != null)
			StopCoroutine(jumpLockCR);

		jumpLockCR = StartCoroutine(SetJumpLockCR());
	}

	private IEnumerator SetJumpLockCR()
	{
		jumpLock = true;

		yield return null;
		yield return null;

		jumpLock = false;
	}

	private void AddVelocityOffset(Vector2 offset)
	{
		velocityOffsetTween.Kill();

		velocityOffset += offset;

		velocityOffsetTween = DOTween.To(() => velocityOffset, (Vector2 val) => velocityOffset = val, Vector2.zero, 0.5f);
	}

	public void OnAttackFinish()
	{
		isAttacking = false;
		myAnimator.ReleaseAttackAnimation();
	}

	public void CreateSlash()
	{
		Transform slash = Instantiate(playerSlashObject, frontTransform.position, Quaternion.identity).transform;
		if (transform.localScale.x == -1)
			slash.localScale = new Vector3(-1, 1, 1);
		slash.parent = transform;
		slash.GetComponent<PlayerSlash>().SetupSlash(this, attackDamage);

		SoundManager.instance.PlayOneShot(SoundManager.Sound.PlayerSwing);
	}

	public void CreateRunParticles()
	{
		ObjectCreator.instance.CreateRunParticles(feetPos.position, transform.localScale);
	}

	private IEnumerator ReenableCollision(Collider2D coll)
	{
		for (int i = 0; i < 17; i++)
			yield return null;

		Physics2D.IgnoreCollision(coll, myCol, false);
	}
}
