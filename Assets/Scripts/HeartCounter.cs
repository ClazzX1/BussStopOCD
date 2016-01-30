using UnityEngine;
using System.Collections;

public class HeartCounter : MonoBehaviour 
{
	static public int totalHearts = 3;

	public int heartIndex = 0;

	private SpriteRenderer sprite;

	void Start() 
	{
		sprite = GetComponent<SpriteRenderer>();
	}
	
	void Update() 
	{
		if (totalHearts <= heartIndex)
			sprite.enabled = false;
		else
			sprite.enabled = true;
	}
}
