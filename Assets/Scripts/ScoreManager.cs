using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {


	static ScoreManager instance = null;

	public Text highScoreText;

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

			highScoreText.text = i + 1 + ". " + ScoreList.Instance.data.scoreList[i].score.ToString ();
			Instantiate (highScoreText, new Vector2 (415, 420 - (40 * (i + 1))), Quaternion.identity);
		}
	}
}
