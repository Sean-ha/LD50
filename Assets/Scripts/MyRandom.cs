using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRandom : MonoBehaviour
{
   // Returns true or false with a 50% chance for either outcome
   public static bool FlipCoin()
   {
      int rand = Random.Range(0, 2);
      return rand == 0;
   }

	// Given a probability p, there is a p chance of returning true, and 1-p chance of returning false
	public static bool RollProbability(float probability)
	{
		if (probability == 0)
			return false;

		float roll = Random.Range(0f, 1f);

		if (roll >= probability)
		{
			return false;
		}

		return true;
	}
}
