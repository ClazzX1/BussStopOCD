using UnityEngine;
using System.Collections;

public class Buss : MonoBehaviour 
{
	private Animator animator;

	public bool isArrived = false;
	public bool isGoneAway = false;

	AudioSource audioSource;
	public AudioClip vehicleAudio;

	void Start () 
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource> ();
	}

	void Update () 
	{
	}

	public void StartBussComing()
	{
		isArrived = false;
		isGoneAway = false;
		animator.SetTrigger("BussStart");
		audioSource.PlayOneShot (vehicleAudio);
	}

	// called from animation
	public void BussArrived()
	{
		isArrived = true;
	}

	// called from animation
	public void BussGoneAway()
	{
		isGoneAway = true;
	}
}
