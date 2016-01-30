using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoreManager : MonoBehaviour {

	public static bool levelComplete;
	public string highscorePos;
	public static int score;
	public static int temp;

	public Text scoreText;

	void Start () 
	{
		score = 0;
		OnGameComplete ();
	}

	public static void OnGameComplete () {
		levelComplete = true;
		//score = 10000; //values from your scoring logic
		for (int i = 1; i <= 5; i++) //for top 5 highscores
		{
			if (PlayerPrefs.GetInt ("highscorePos" + i) < score)   //if cuurent score is in top 5
			{
				temp = PlayerPrefs.GetInt ("highscorePos" + i);    //store the old highscore in temp varible to shift it down 
				PlayerPrefs.SetInt ("highscorePos" + i, score);    //store the currentscore to highscores
				if (i < 5)                                         //do this for shifting scores down
				{
					int j = i + 1;
					score = PlayerPrefs.GetInt ("highscorePos" + j);
					PlayerPrefs.SetInt ("highscorePos" + j ,temp);    
				}
			}
		}
	}

	void ScoreBoard () {
		if (levelComplete)
		{
			for (int y = 1; y <= 5; y++)
			{
				scoreText.text = y + ". " + PlayerPrefs.GetInt("highscorePos" + y);
				Instantiate (scoreText, new Vector2 (415, 420 - (40 * y)), Quaternion.identity);
			}
			levelComplete = false;
		}
	}
}
