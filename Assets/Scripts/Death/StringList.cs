using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StringList : ScriptableObject
{
	[TextArea(3, 8)]
	public List<string> strList;
}
