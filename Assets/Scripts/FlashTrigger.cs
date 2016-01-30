using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashTrigger : MonoBehaviour 
{
	private Image image;

	private float flashAnim = 0.0f;

	void Start () 
	{
		image = GetComponent<Image>();
	}
	
	void Update () 
	{
		if (flashAnim > 0.0f) 
		{
			flashAnim = Mathf.Max(0.0f, flashAnim - Time.deltaTime);
			Color color = image.color;
			color.a = Mathf.Lerp (0.0f, 1.0f, flashAnim);
			image.color = color;
		}
	}

	public void TriggerFlash()
	{
		flashAnim = 1.0f;
	}
}
