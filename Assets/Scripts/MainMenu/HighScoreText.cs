using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreText : MonoBehaviour
{
	private void Start()
	{
		GetComponent<TextMeshPro>().text = "high score:\n" + PlayerPrefs.GetInt("HighScore", 0).ToString();
	}
}
