using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour 
{
	public AudioClip musicClip;

	void Start () 
	{
	}
	
	void Update () 
	{
	}

	public void playMusic (AudioClip audioClip)
	{
		musicClip = audioClip;

		AudioSource musicSource = GetComponent<AudioSource>();
		musicSource.Stop ();
		musicSource.clip = musicClip;
		if (musicClip != null)
			musicSource.Play ();
	}

	public void stopMusic()
	{
		musicClip = null;
		AudioSource musicSource = GetComponent<AudioSource>();
		musicSource.Stop ();
	}
}
