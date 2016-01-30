using UnityEngine;
using System.Collections;

public class Buss : MonoBehaviour 
{
	private Animator animator;

	public bool isArrived = false;
	public bool isGoneAway = false;

	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	void Update () 
	{
	}

	public void StartBussComing()
	{
		isArrived = false;
		isGoneAway = false;
		animator.SetTrigger("BussStart");
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
