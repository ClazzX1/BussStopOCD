using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour 
{
	public List<AudioClip> smokingAudio;
	public List<AudioClip> coughingAudio;
	public List<AudioClip> phoneAudio;
	public List<AudioClip> busAudio;

	private AudioSource audioSource;
	private SkeletonAnimation spine;

	private Vector3 originalPosition;
	private Vector3 originalScale;

	void Start () 
	{
		originalPosition = transform.position;
		originalScale = transform.localScale;

		audioSource = GetComponent<AudioSource>();
		spine = GetComponent<SkeletonAnimation>();
	}

	void Update () 
	{
		Vector3 pos = originalPosition;
		pos.y -= Mathf.Sin(BeatManager.beatTimer * 2.0f * Mathf.PI) * 0.15f;
		transform.position = pos;

		Vector3 scale = originalScale;
		scale.x += Mathf.Sin(BeatManager.beatTimer * 2.0f * Mathf.PI) * 0.02f;
		scale.y -= Mathf.Sin(BeatManager.beatTimer * 2.0f * Mathf.PI) * 0.05f;

		transform.localScale = scale;
	}

	public void DoMoveAnimation(int moveIndex)
	{
		if (moveIndex == 0) 
		{
			spine.state.SetAnimation(0, "SmokeCough", false);
			int index = Random.Range (0, coughingAudio.Count);
			audioSource.PlayOneShot (coughingAudio [index]);
		}
		if (moveIndex == 1) 
		{
			spine.state.SetAnimation(0, "Phone", false);
			int index = Random.Range (0, phoneAudio.Count);
			audioSource.PlayOneShot (phoneAudio [index]);
		}
		if (moveIndex == 2) 
		{
			spine.state.SetAnimation(0, "SmokeCough", false);
			int index = Random.Range (0, smokingAudio.Count);
			audioSource.PlayOneShot (smokingAudio [index]);
		}
		if (moveIndex == 3) 
		{
			spine.state.SetAnimation(0, "BusWave", false);
			int index = Random.Range (0, busAudio.Count);
			audioSource.PlayOneShot (busAudio [index]);
		}
	}
}
