using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
	private float tmpResetColorAnim = 0.0f;

	void Start () 
	{
	}

	void Update () 
	{
		if (tmpResetColorAnim > 0.0f) 
		{
			tmpResetColorAnim -= Time.deltaTime * 1.5f;
			if (tmpResetColorAnim <= 0.0f) 
			{
				SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
				sprite.color = Color.white;
			}
		}
	}

	public void DoMoveAnimation(int moveIndex)
	{
		// TODO: replace this temp code to start spine animations instead
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		if (moveIndex == 0)
			sprite.color = Color.red;
		if (moveIndex == 1)
			sprite.color = Color.blue;
		if (moveIndex == 2)
			sprite.color = Color.green;
		if (moveIndex == 3)
			sprite.color = Color.magenta;
		tmpResetColorAnim = 1.0f;
	}
}
