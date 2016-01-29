using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AppearWithGameState : MonoBehaviour 
{
    public enum AppearStyle
    {
        FADE,
        SLIDE_FROM_BOTTOM
    }
    public enum InterpolationStyle
    {
        LERP,
        SMOOTH,
        SPRING
    }

    public GameState gameState;
    public float inSpeed = 0.5f;
    public float outSpeed = 0.5f;
    public AppearStyle style = AppearStyle.FADE;
    public InterpolationStyle interpolation = InterpolationStyle.SMOOTH;

    private float anim = 0.0f;
    private Image image = null;
    private Text text = null;
    private float imageTargetAlpha = 1.0f;
    private float textTargetAlpha = 1.0f;
    private Vector3 startPosition;

	void Start () 
    {
        image = GetComponent<Image>();
        text = GetComponent<Text>();
        imageTargetAlpha = image ? image.color.a : 1.0f;
        textTargetAlpha = text ? text.color.a : 1.0f;
        anim = 0.0f; // (IsStateActive() ? 0.0f : 1.0f);
        startPosition = transform.position;
        Update();
	}

	void Update () 
    {
        anim += (IsStateActive() ? inSpeed : -outSpeed) * Time.deltaTime;
        anim = Mathf.Clamp01(anim);

        float smoothAnim = anim;
        if (interpolation == InterpolationStyle.SPRING)
        {
            smoothAnim = 0.0f;
            if (anim == 0.0f || anim == 1.0f)
            {
                smoothAnim = anim;
            }
            else
            {
                float s = 0.3f * 0.25f;
                smoothAnim = Mathf.Pow(2.0f, -12.0f * anim) * Mathf.Sin((anim - s) * (Mathf.PI * 2.0f) / 0.3f) + 1.0f;
            }
        }
        else if (interpolation == InterpolationStyle.SMOOTH)
        {
            Mathf.SmoothStep(0.0f, 1.0f, anim);
        }

        if (style == AppearStyle.FADE)
        {
            if (image)
                image.color = new Color(image.color.r, image.color.g, image.color.b, imageTargetAlpha * smoothAnim);
            if (text)
                text.color = new Color(text.color.r, text.color.g, text.color.b, textTargetAlpha * smoothAnim);
        }
        else if (style == AppearStyle.SLIDE_FROM_BOTTOM)
        {
            transform.position = startPosition + new Vector3(0.0f, -300.0f * (1.0f - smoothAnim), 0.0f);
        }
	}

    private bool IsStateActive()
    {
        return GameStateManager.Instance.getState() == gameState;
    }
}
