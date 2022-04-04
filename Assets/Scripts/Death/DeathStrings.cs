using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DeathStrings : ScriptableObject
{
	[TextArea(3, 8)]
	public List<string> deathStrings;
}
