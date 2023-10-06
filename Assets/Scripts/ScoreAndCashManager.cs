using StackTheBlockArslan;
using System;
using UnityEngine;

public class ScoreAndCashManager : LazySingleton<ScoreAndCashManager>
{
	[HideInInspector]
	public int currentScore;

	private float currentCash;

	[Tooltip("Score increases by this value")]
	[SerializeField]
	private int scoreUnit;

	[Tooltip("Cash updates after this score")]
	[SerializeField]
	private int cashUpdateFrequency;

	private void Start()
	{
		GameManager instance = LazySingleton<GameManager>.Instance;
		instance.GameInitialized = (Action)Delegate.Combine(instance.GameInitialized, new Action(Init));
		GameManager instance2 = LazySingleton<GameManager>.Instance;
		instance2.GameOver = (Action)Delegate.Combine(instance2.GameOver, new Action(GameOver));
		GameManager instance3 = LazySingleton<GameManager>.Instance;
		instance3.GameStarted = (Action)Delegate.Combine(instance3.GameStarted, new Action(ResetScore));
	}

	public void Init()
	{
		UIManager.Instance.UpdateHudData(0, 0f);
	}

	public void UpdateScore()
	{
		currentScore += scoreUnit;
		if (cashUpdateFrequency != 0 && currentScore % cashUpdateFrequency == 0)
		{
			currentCash += 1f;
		}
		UIManager.Instance.UpdateHudData(currentScore, currentCash);
	}

	private void ResetScore()
	{
		currentScore = 0;
	}

	private void GameOver()
	{
		if (currentScore > PlayerDataHandler<PlayerData>.Instance.highScore || currentCash > 0f)
		{
			PlayerDataHandler<PlayerData>.Instance.highScore = currentScore;
			PlayerDataHandler<PlayerData>.Instance.cash += currentCash;
			Invoke("SaveData", 2f);
		}
		UIManager.Instance.UpdateGameOverData(currentScore, PlayerDataHandler<PlayerData>.Instance.highScore, PlayerDataHandler<PlayerData>.Instance.cash);
	}

	public void SaveData()
	{
		PlayerDataHandler<PlayerData>.Instance.SaveData();
	}
}
