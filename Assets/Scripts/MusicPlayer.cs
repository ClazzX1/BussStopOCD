using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour 
{
	private static MusicPlayer instance = null;

	public AudioClip musicClip;

	public static MusicPlayer Instance 
	{
		get { return instance; }
	}

	void Awake() 
	{
		if (instance != null)
		{
			if (musicClip == null)
				instance.stopMusic();
			else if (instance.musicClip == null || !instance.musicClip.name.Equals(musicClip.name))
				instance.playMusic(musicClip);

			Destroy(this.gameObject);
			return;
		}
		else
		{
			if (instance != this)
			{
				instance = this;
				instance.playMusic(musicClip);
			}
		}
		DontDestroyOnLoad(this.gameObject); 
	}
	 
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
