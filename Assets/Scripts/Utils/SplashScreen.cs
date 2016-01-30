using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private enum FadeState
    {
        FADE_IN,
        FADE_OUT
    }

    public static bool isTutorialShown = false;

    public string targetLevel = "mainmenu";
    public float minsShowTimeSec = 1.0f;
    public float showTimeSec = 1.0f;
    public bool isExitWithAnyKey = true;

    public bool isEnableLevelChange = true;
    public bool isEnableFadeIn = true;
    public bool isEnableFadeOut = true;
    public float fadeInSpeed = 0.5f;
    public float fadeOutSpeed = 0.5f;
    public Texture2D fadeOutTexture;
    public int fadeOutDepth = -1000;

    public bool isEnableGameState = false;
    public GameState gameStateAtStart = GameState.UNKNOWN;

    private float startTime = 0.0f;
    private bool isRequestExit = false;
    private bool isAlreadyCallExit = false;
    private float fadeAlpha = 1.0f;
    private FadeState fadingState = FadeState.FADE_IN;

    private bool isRequestCustomLevelChange = false;
    private string customTargetLevel = "mainmenu";

    void Start()
    {
        startTime = Time.time;
        isRequestExit = false;
        isRequestCustomLevelChange = false;
        isAlreadyCallExit = false;
        fadingState = FadeState.FADE_IN;
        fadeAlpha = isEnableFadeIn ? 1.0f : 0.0f;
        if (isEnableGameState)
            GameStateManager.Instance.setState(gameStateAtStart);
    }

    void Update()
    {       
        UpdateLevelChangeCheck();
    }

    void UpdateLevelChangeCheck()
    {
        if (isEnableLevelChange && !isRequestExit && !isRequestCustomLevelChange)
        {
            if (!isEnableFadeIn || fadeAlpha <= 0.0f)
            {
                if (Time.time - startTime > showTimeSec)
                    isRequestExit = true;
                else if (isExitWithAnyKey && Input.anyKeyDown && Time.time - startTime > minsShowTimeSec)
                    isRequestExit = true;
            }
        }

        bool isExit = (isRequestExit || isRequestCustomLevelChange);
        if (isEnableFadeOut)
        {
            if (isExit)
                fadingState = FadeState.FADE_OUT;
            if (fadingState == FadeState.FADE_OUT && fadeAlpha < 1.0f)
                isExit = false;
        }

        if (isExit && !isAlreadyCallExit)
        {
            isAlreadyCallExit = true;
			SceneManager.LoadScene(isRequestCustomLevelChange ? customTargetLevel : targetLevel);
        }
    }

    void OnGUI()
    {
        fadeAlpha += (fadingState == FadeState.FADE_IN ? -fadeInSpeed : fadeOutSpeed) * Time.deltaTime;
        fadeAlpha = Mathf.Clamp01(fadeAlpha);
        if (fadeAlpha > 0.0f)
        {
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeAlpha);
            GUI.depth = fadeOutDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
        }
    }

    public void TriggerLevelChange()
    {
        if (isRequestExit || isRequestCustomLevelChange)
            return;

        isRequestExit = true;
    }

    public void TriggerCustomLevelChange(string customLevelName)
    {
        if (isRequestExit || isRequestCustomLevelChange)
            return;

        isRequestCustomLevelChange = true;
        customTargetLevel = customLevelName;
    }

    public void TriggerMainGameScene()
    {
        if (isRequestExit || isRequestCustomLevelChange)
            return;

        isRequestCustomLevelChange = true;
        if (isTutorialShown)
            customTargetLevel = "MainScene";
        else
            customTargetLevel = "TutorialMenu";
    }

    public void TriggerContinueFromTutorial()
    {
        if (isRequestExit || isRequestCustomLevelChange)
            return;

        isRequestCustomLevelChange = true;
        if (isTutorialShown)
            customTargetLevel = "MainMenu";
        else
            customTargetLevel = "MainScene";

        isTutorialShown = true;
    }
}