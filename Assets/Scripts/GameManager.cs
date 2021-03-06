﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum InGameState
{
	IDLE,
	BUSS_COMING,
	BUSS_LEAVING,
	OTHERS_TURN,
	PLAYER_TURN,
	PATTERN_FAIL,
	PATTERN_COMPLETE,
	AMBULANCE_COMING,
	AMBULANCE_LEAVING,
	END_BUSS_COMING,
	END_BUSS_LEAVING
}

public class GameManager : MonoBehaviour
{
	public InGameState state = InGameState.IDLE;
	
	public Transform playerSpawnPoint;
    public Transform spawnPoint;

	public Buss buss;
	public Buss ambulanceBuss;
	public Animator beatIndicator;
	public SpriteRenderer beatIndicatorSprite;
	public FlashTrigger redFlash;
	public Text highscoreText;
	public List<AudioClip> musicClips;
	public MusicPlayer musicPlayer;

	public SkeletonDataAsset playerSkeleton;
	public List<SkeletonDataAsset> otherSkeletons;

    private int round = 1;
	private int score = 0;
	private List<Character> characters = new List<Character>();
	private Character player = null;

	private MovePattern movePattern;
	private BeatManager beatManager;

	void Start ()
    {
		movePattern = GetComponent<MovePattern>();
		beatManager = GetComponent<BeatManager>();

		MovePattern.OnPatternFrameChange += OnPatternFrameChange;
		MovePattern.OnPatternPlayerTurnStart += OnPatternPlayerTurnStart;
		MovePattern.OnPatternComplete += OnPatternComplete;
		MovePattern.OnPatternFailed += OnPatternFailed;
		MovePattern.OnCorrectMove += OnCorrectMove;
		BeatManager.OnBeat += OnBeat;

		ChangeSpeed(1.0f);
		UpdateScore(0);
		round = 0;
        StageStart();
    }

	void OnDestroy()
	{
		MovePattern.OnPatternFrameChange -= OnPatternFrameChange;
		MovePattern.OnPatternPlayerTurnStart -= OnPatternPlayerTurnStart;
		MovePattern.OnPatternComplete -= OnPatternComplete;
		MovePattern.OnPatternFailed -= OnPatternFailed;
		MovePattern.OnCorrectMove -= OnCorrectMove;
		BeatManager.OnBeat -= OnBeat;
	}

	void Update ()
    {
		if (state == InGameState.BUSS_COMING) 
		{
			if (buss.isArrived)
				BussArrived ();
		}
		else if (state == InGameState.BUSS_LEAVING) {
			if (buss.isGoneAway)
				BussGoneAway ();
		} 
		else if (state == InGameState.PATTERN_FAIL) 
		{
			--HeartCounter.totalHearts;
			if (HeartCounter.totalHearts <= 0) {
				state = InGameState.AMBULANCE_COMING;
				ambulanceBuss.StartBussComing ();
			}
			else
			{
			}
		}
		else if (state == InGameState.PATTERN_COMPLETE) 
		{
			state = InGameState.END_BUSS_COMING;
			buss.StartBussComing();
		}
		else if (state == InGameState.AMBULANCE_COMING) 
		{
			if (ambulanceBuss.isArrived)
				AmbulanceArrived();
		}
		else if (state == InGameState.AMBULANCE_LEAVING) 
		{
			if (ambulanceBuss.isGoneAway) 
			{
				AmbulanceGoneAway ();
				state = InGameState.IDLE;
			}
		}
		else if (state == InGameState.END_BUSS_COMING) 
		{
			if (buss.isArrived) 
			{
				KillCharacters();
				state = InGameState.END_BUSS_LEAVING;
			}
		}
		else if (state == InGameState.END_BUSS_LEAVING) 
		{
			if (buss.isGoneAway) 
			{
				StageStart ();
			}
		} 

		if (beatIndicatorSprite) 
		{
			Color color = Color.red;
			if (state != InGameState.OTHERS_TURN &&	state != InGameState.PLAYER_TURN)
				color.a = 0.0f;
			beatIndicatorSprite.color = color;
		}

		if (player)
		{
			int moveIndex = -1;

			if (Input.GetButtonDown("Move1"))
				moveIndex = 0;
			else if (Input.GetButtonDown("Move2"))
				moveIndex = 1;
			else if (Input.GetButtonDown("Move3"))
				moveIndex = 2;
			else if (Input.GetButtonDown("Move4"))
				moveIndex = 3;
			
			if (moveIndex != -1) 
			{
				player.DoMoveAnimation(moveIndex);
				movePattern.PlayerDidMove(moveIndex);
			}
		}
	}

	public void ChangeSpeed(float newSpeed)
	{
		beatManager.speed = newSpeed;
		beatIndicator.speed = newSpeed;

		// TODO: change music speed?

		if (player) 
		{
			Animator playerAnimator = player.gameObject.GetComponent<Animator> ();
			if (playerAnimator)
				playerAnimator.speed = newSpeed * 1.5f;
		}
		foreach (Character character in characters) 
		{
			Animator animator = character.gameObject.GetComponent<Animator> ();
			if (animator)
				animator.speed = newSpeed * 1.5f;
		}
	}

    public void StageStart()
    {
		state = InGameState.BUSS_COMING;
		++round;

		if (round == 1)
			ChangeSpeed(1.3333f);
		if (round == 2)
			ChangeSpeed(1.5f);
		if (round == 3)
			ChangeSpeed(1.6666f);
		if (round == 4)
			ChangeSpeed(1.6666f);

		if (musicClips.Count >= round)
			musicPlayer.playMusic(musicClips[round - 1]);

		if (musicClips.Count >= round)
			musicPlayer.playMusic(musicClips[round - 1]);

		buss.StartBussComing();
    }

	public void SpawnCharacters()
	{
		int characterCount = 1 + round / 2;
		for (int i = 0; i < characterCount; ++i)
		{
			Vector3 position = spawnPoint.position;
			float offsetX = (float)((i + 1) / 2) * 2.2f;
			position.x += (i % 2 == 0 ? -offsetX : offsetX);
			Character character = CreateCharacter(position, false);
			characters.Add(character);
		}
			
		player = CreateCharacter(playerSpawnPoint.position, true);
	}

	public void KillCharacters()
	{
		for (int i = 0; i < characters.Count; ++i)
		{
			Destroy(characters[i].gameObject);
		}
		characters.Clear();

		if (player) 
		{
			Destroy (player.gameObject);
			player = null;
		}
	}

	Character CreateCharacter(Vector3 position, bool isPlayer)
    {
        GameObject newObject = (GameObject)Instantiate(Resources.Load("Character"));
        newObject.transform.parent = transform;
        newObject.transform.position = position;

		Animator animator = newObject.GetComponent<Animator>();
		if (animator)
			animator.speed = beatManager.speed;

		if (isPlayer) 
		{
			SkeletonAnimation skeletonAnimation = newObject.GetComponent<SkeletonAnimation> ();
			skeletonAnimation.skeletonDataAsset = playerSkeleton;
			skeletonAnimation.Reset ();
		}
		else
		{
			int index = Random.Range(0, otherSkeletons.Count);

			SkeletonAnimation skeletonAnimation = newObject.GetComponent<SkeletonAnimation> ();
			skeletonAnimation.skeletonDataAsset = otherSkeletons[index];
			skeletonAnimation.Reset ();
		}

		Character character = newObject.GetComponent<Character>();
		return character;
    }

	private void OnBeat()
	{
		if (beatIndicator)
			beatIndicator.SetTrigger("HeartBeat");
	}

	private void OnPatternFrameChange(int frameIndex)
	{
		if (state == InGameState.OTHERS_TURN) 
		{
			int characterIndex = Random.Range (0, characters.Count);
			characters [characterIndex].DoMoveAnimation(frameIndex);
		}
	}

	private void OnPatternPlayerTurnStart()
	{
		state = InGameState.PLAYER_TURN;
	}

	private void OnPatternComplete()
	{
		state = InGameState.PATTERN_COMPLETE;
	}

	private void OnPatternFailed()
	{
		--HeartCounter.totalHearts;
		redFlash.TriggerFlash();

		if (HeartCounter.totalHearts <= 0) 
		{
			player.Faint ();
			movePattern.StopPattern ();
			state = InGameState.AMBULANCE_COMING;
			ambulanceBuss.StartBussComing ();
		}
	}

	private void BussArrived()
	{
		state = InGameState.BUSS_LEAVING;
		SpawnCharacters();
	}

	private void BussGoneAway()
	{
		state = InGameState.OTHERS_TURN;
		movePattern.StartPattern(2 + round / 2, 2.5f);
	}

	private void AmbulanceArrived()
	{
		state = InGameState.AMBULANCE_LEAVING;
		KillCharacters();
	}

	private void AmbulanceGoneAway()
	{
		ScoreList.Instance.AddScore(score, "");
		UpdateScore(0);

		HeartCounter.totalHearts = 3;
		SceneManager.LoadScene("StartMenu");
	}

	private void OnCorrectMove()
	{
		UpdateScore(score + 1);
	}

	private void UpdateScore(int newScore)
	{
		score = newScore;
		highscoreText.text = score.ToString();
	}
}
