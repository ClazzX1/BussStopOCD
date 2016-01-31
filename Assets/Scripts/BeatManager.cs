using UnityEngine;
using System.Collections;

public class BeatManager : MonoBehaviour 
{
	public static float beatTimer = 0.0f;

	public delegate void BeatAction();
	public static event BeatAction OnBeat;
	public static event BeatAction OnAfterBeat;

	public float speed = 1.0f;
	public float beatWindowSize = 0.1f;

	void Start () 
	{	
		beatTimer = 0.0f;
	}

	void Update () 
	{
		float oldBeat = beatTimer;
		beatTimer += Time.deltaTime * speed;

		bool isAfterBeat = false;
		if (beatTimer > beatWindowSize && oldBeat <= beatWindowSize)
			isAfterBeat = true;
		if (beatTimer > beatWindowSize + 1.0f && oldBeat <= beatWindowSize + 1.0f)
			isAfterBeat = true;

		if (isAfterBeat)
		{
			if (OnAfterBeat != null)
				OnAfterBeat();
		}

		if (beatTimer > 1.0f) 
		{
			beatTimer -= 1.0f;
			if (OnBeat != null)
				OnBeat();
		}
	}

	public bool IsInTheBeat()
	{
		if (beatTimer < beatWindowSize)
			return true;
		else if (beatTimer > 1.0f - beatWindowSize)
			return true;
		else
			return false;
	}
}
