using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTyper : MonoBehaviour
{
	[SerializeField] private StringList strings;

	private TextMeshPro tmp;

	private void Awake()
	{
		tmp = GetComponent<TextMeshPro>();
	}

	public void DisplayString(System.Action onComplete)
	{
		// Get random death string
		List<string> strs = strings.strList;

		string chosen = strs[Random.Range(0, strs.Count)];

		StartCoroutine(TypeString(chosen, onComplete));
	}

	private IEnumerator TypeString(string text, System.Action onComplete)
	{
		foreach (char c in text)
		{
			tmp.text += c;
			if (c == ' ')
				continue;

			SoundManager.instance.PlayOneShot(SoundManager.Sound.TypeSound);

			yield return null;
			yield return null;
			yield return null;
			yield return null;
			yield return null;
		}

		onComplete.Invoke();
	}
}
