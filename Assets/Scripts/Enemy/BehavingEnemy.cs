using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehavingEnemy : MonoBehaviour
{
	public HealthSystem myHealthSystem;

	public abstract void KillBehavior();
}
