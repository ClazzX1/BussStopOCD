using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {


	static ScoreManager instance = null;

	public List<Text> highScoreTexts;

	void Awake () {

		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}

	}

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

			highScoreTexts[i].text = i + 1 + ". " + ScoreList.Instance.data.scoreList[i].score.ToString ();
		}
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
