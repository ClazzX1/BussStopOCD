using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {


	static ScoreManager instance = null;

<<<<<<< HEAD
=======
	public static int score = 0;
	int [] highScore = new int[5];
>>>>>>> 4a4e32d44b810b6100bf572be6893eddeea801b0
	public Text highScoreText;

	void Awake () {

		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}

	}

<<<<<<< HEAD
	void Start () 
	{
		UpdateHighScoreText ();
	}

	void Update () 
	{


	}


	void UpdateHighScoreText () 
	{
		for (int i = 0; i < 5; ++i) 
		{
			if (ScoreList.Instance.data.scoreList.Count <= i)
				continue;

			highScoreText.text = i + 1 + ". " + ScoreList.Instance.data.scoreList[i].score.ToString ();
=======
	void Start () {

		highScore [0] = 1000000;
		highScore [1] = 500000;
		highScore [2] = 250000;
		highScore [3] = 50000;
		highScore [4] = 1000;

		UpdateHighScoreText ();

	}

	void Update () {
	
		UpdateHighScore ();

	}

	void UpdateHighScore () {

		if (score > highScore[0]) {
			highScore[0] = score;
		} else if (score < highScore[0] && score > highScore[1]) {
			highScore[1] = score;
		} else if (score < highScore[1] && score > highScore[2]) {
			highScore[2] = score;
		} else if (score < highScore[2] && score > highScore[3]) {
			highScore[3] = score;
		} else if (score < highScore[3] && score > highScore[4]) {
			highScore[4] = score;
		}
	}

	void UpdateHighScoreText () {
		for (int i = 0; i < 5; i++) {
			highScoreText.text = i + 1 + ". " + highScore [i].ToString ();
>>>>>>> 4a4e32d44b810b6100bf572be6893eddeea801b0
			Instantiate (highScoreText, new Vector2 (415, 420 - (40 * (i + 1))), Quaternion.identity);
		}
	}
}
