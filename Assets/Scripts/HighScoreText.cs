using UnityEngine;
using System.Collections;

public class HighScoreText : MonoBehaviour {

	void Start () {

		transform.parent = GameObject.Find ("HighScoreList").transform;

	}
}
