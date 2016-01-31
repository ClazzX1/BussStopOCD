using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour 
{
	private float tmpResetColorAnim = 0.0f;

	public List<AudioClip> smokingAudio;
	public List<AudioClip> coughingAudio;
	public List<AudioClip> phoneAudio;
	public List<AudioClip> busAudio;
	private AudioSource audioSource;

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
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
		{
			sprite.color = Color.red;
			int index = Random.Range (0, coughingAudio.Count);
			audioSource.PlayOneShot (coughingAudio [index]);
		}
		if (moveIndex == 1) 
		{
			sprite.color = Color.blue;
			int index = Random.Range (0, phoneAudio.Count);
			audioSource.PlayOneShot (phoneAudio [index]);
		}
		if (moveIndex == 2) 
		{
			sprite.color = Color.green;
			int index = Random.Range (0, busAudio.Count);
			audioSource.PlayOneShot (busAudio [index]);
		}
		if (moveIndex == 3) 
		{
			sprite.color = Color.magenta;
			int index = Random.Range (0, smokingAudio.Count);
			audioSource.PlayOneShot (smokingAudio [index]);
		}
		tmpResetColorAnim = 1.0f;
	}
}
