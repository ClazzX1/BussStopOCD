using UnityEngine;
using System.Collections;

public enum GameState
{
    UNKNOWN,
    SPLASH,
    MAIN_MENU,
    IN_GAME,
    GAME_OVER,
    TUTORIAL
}

public class GameStateManager : MonoBehaviour 
{
    private static GameStateManager instance = null;

    public GameState getState() { return state; }
    public void setState(GameState newState) { state = newState; }
    private GameState state = GameState.UNKNOWN;

    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("GameStateManager");
                obj.hideFlags = HideFlags.HideAndDontSave;
                instance = obj.AddComponent<GameStateManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

	void Start () 
    {	
	}

	void Update () 
    {
	}

    void OnGUI()
    {
        if (Application.isEditor)
        {
            Rect rect = new Rect(10.0f, 10.0f, 500.0f, 500.0f);
            string text = state.ToString();
            GUI.Label(rect, text);
        }
    }
}
