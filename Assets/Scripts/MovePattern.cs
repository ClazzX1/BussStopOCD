using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PatternState
{
	IDLE,
	OTHERS_DO_MOVES,
	PLAYER_TURN_WAIT_INPUT,
	PLAYER_TURN,
	FINISHED
}

public class MovePattern : MonoBehaviour 
{
	public delegate void PatternFrameAction(int frameIndex);
	public static event PatternFrameAction OnPatternFrameChange;

	public delegate void PatternAction();
	public static event PatternAction OnPatternPlayerTurnStart;
	public static event PatternAction OnPatternComplete;
	public static event PatternAction OnPatternFailed;
	public static event PatternAction OnCorrectMove;

	public PatternState patternState = PatternState.IDLE;

	private List<int> moveList = new List<int>();
	public int currentFrame;
	public int playerMoveFrame = 0;
	private float nextMoveTimer;

	private float startTime = 0.0f;
	private float lengthTime = 0.0f;

	private BeatManager beatManager;

	void Start () 
	{
		beatManager = GetComponent<BeatManager>(); 

		BeatManager.OnBeat += OnBeat;
		BeatManager.OnAfterBeat += OnAfterBeat;
	}

	void OnDestroy()
	{
		BeatManager.OnBeat -= OnBeat;
		BeatManager.OnAfterBeat -= OnAfterBeat;
	}

	void Update() 
	{
	}

	public void StartPattern(int moveCount, float patternLengthTime)
	{
		lengthTime = patternLengthTime;
		GenerateNewPattern(moveCount);
		patternState = PatternState.OTHERS_DO_MOVES;
		RestartPattern();
	}

	public void StopPattern()
	{
		patternState = PatternState.FINISHED;
	}

	public void PlayerDidMove(int moveIndex)
	{
		if (patternState == PatternState.PLAYER_TURN_WAIT_INPUT) 
		{
			bool isFail = false;
			currentFrame = -1;
			playerMoveFrame = 0;

			if (!beatManager.IsInTheBeat())
				isFail = true;			
			if (moveIndex != moveList[playerMoveFrame])
				isFail = true;

			++playerMoveFrame;

			if (isFail)
				PatternFailed();
			else
			{
				if (OnCorrectMove != null)
					OnCorrectMove ();
			}

			patternState = PatternState.PLAYER_TURN;
		}
		else if (patternState == PatternState.PLAYER_TURN) 
		{
			bool isFail = false;

			if (!beatManager.IsInTheBeat())
				isFail = true;
			if (moveIndex != moveList[playerMoveFrame])
				isFail = true;

			++playerMoveFrame;

			if (isFail)
				PatternFailed();
			else
			{
				if (OnCorrectMove != null)
					OnCorrectMove ();
				if (playerMoveFrame >= moveList.Count)
				{
					PatternComplete ();
				}
			}
		}
	}

	private void RestartPattern()
	{
		startTime = Time.fixedTime;
		currentFrame = -1;
		playerMoveFrame = 0;
	}

	private void StartPlayerTurn()
	{
		if (OnPatternPlayerTurnStart != null)
			OnPatternPlayerTurnStart();
		
		patternState = PatternState.PLAYER_TURN_WAIT_INPUT;
		RestartPattern();
	}

	private void EndPattern()
	{
		if (patternState == PatternState.OTHERS_DO_MOVES)
			StartPlayerTurn();
		else if (patternState == PatternState.PLAYER_TURN)
			PatternComplete();
	}

	private void PatternComplete()
	{
		patternState = PatternState.FINISHED;

		if (OnPatternComplete != null)
			OnPatternComplete();
	}

	private int GetCorrectFrameForPlayer()
	{
		float time = Time.fixedTime - startTime;
		int frame = (int)((float)moveList.Count / lengthTime * time);
		return frame;
	}

	private void GenerateNewPattern(int length)
	{
		moveList.Clear();
		for (int i = 0; i < length; ++i) 
		{
			int move = Random.Range(0, 4);
			moveList.Add(move);
		}
	}

	private void PatternFailed()
	{
		if (OnPatternFailed != null)
			OnPatternFailed();

		// patternState = PatternState.FINISHED;
	}

	private void OnBeat()
	{
		if (patternState == PatternState.OTHERS_DO_MOVES) 
		{
			++currentFrame;
			if (currentFrame >= moveList.Count)
				EndPattern();
			else if (OnPatternFrameChange != null)
				OnPatternFrameChange( moveList[currentFrame] );	
		}			
	}

	private void OnAfterBeat()
	{
		if (patternState == PatternState.PLAYER_TURN) 
		{
			++currentFrame;
			if (currentFrame > moveList.Count) 
			{
				PatternComplete ();
			}
			else 
			{
				if (currentFrame >= playerMoveFrame)
					PatternFailed ();
			}
		}
	}
}
