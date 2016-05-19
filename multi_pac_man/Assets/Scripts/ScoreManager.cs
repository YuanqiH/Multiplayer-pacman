using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour {

	public static int score;
	public static string localTag;
	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		text.text = "" + 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (score < 0)
			score = 0;
		if (localTag == text.tag)//if the tag trigged is the same of the score
			text.text = "" + score;
	}

	public static void addPoints(int pointToAdd, string tag)
	{
		localTag = tag;
		score += pointToAdd;
	}

	public static void resetPoint()
	{
		score = 0;
	}
}
