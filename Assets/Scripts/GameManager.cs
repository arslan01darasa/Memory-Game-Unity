using StackTheBlockArslan;
using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : LazySingleton<GameManager>
{
	[HideInInspector]
	public GameMode gameMode;

	[SerializeField]
	private string playStoreURL;

	[SerializeField]
	private string appStoreURL;

	public Action GameInitialized;

	public Action GameStarted;

	public Action Scored;

	public Action GameContinued;

	public Action GameOver;

	public UnityEvent OnGameIntialized;

	public UnityEvent OnGameStarted;

	public UnityEvent OnGameOver;

	private void Awake()
	{
		Application.targetFrameRate = 300;
		gameMode = GetComponent<GameMode>();
		PlayerDataHandler<PlayerData>.Create();
		GameInitialized = (Action)Delegate.Combine(GameInitialized, (Action)delegate
		{
			OnGameIntialized.Invoke();
		});
		GameStarted = (Action)Delegate.Combine(GameStarted, (Action)delegate
		{
			OnGameStarted.Invoke();
		});
		GameOver = (Action)Delegate.Combine(GameOver, (Action)delegate
		{
			OnGameOver.Invoke();
		});
	}

	private void OnDestroy()
	{
		GameInitialized = (Action)Delegate.Remove(GameInitialized, (Action)delegate
		{
			OnGameIntialized.Invoke();
		});
		GameStarted = (Action)Delegate.Remove(GameStarted, (Action)delegate
		{
			OnGameStarted.Invoke();
		});
		GameOver = (Action)Delegate.Remove(GameOver, (Action)delegate
		{
			OnGameOver.Invoke();
		});
	}

	private void Start()
	{
		InitGame();
		if (PlayerDataHandler<PlayerData>.Instance.adsRemoved)
		{
			UIManager.Instance.removeAdBtn.SetActive(value: false);
		}
	}

	public void InitGame()
	{
		if (GameInitialized != null)
		{
			GameInitialized();
		}
		gameMode.enabled = true;
	}

	public void StartGame()
	{
		if (GameStarted != null)
		{
			GameStarted();
		}
	}

	public void EndGame()
	{
		if (GameOver != null)
		{
			GameOver();
        }
	}

	public void gameover()
	{

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

	public void RateGame()
	{
		Application.OpenURL(playStoreURL);
	}

	public void RemoveAds()
	{
		PlayerDataHandler<PlayerData>.Instance.adsRemoved = true;
		PlayerDataHandler<PlayerData>.Instance.SaveData();
		LazySingleton<AdsManager>.Instance.isAdRemoved = true;
		UIManager.Instance.removeAdBtn.SetActive(value: false);
	}

	public void IncreasePlayerCash()
	{
		float num = 100f;
		PlayerDataHandler<PlayerData>.Instance.cash += num;
		PlayerDataHandler<PlayerData>.Instance.SaveData();
		UnityEngine.Debug.Log("player cash increased by " + num);
	}

	public void DecreasePlayerCash()
	{
		float num = 100f;
		PlayerDataHandler<PlayerData>.Instance.cash -= num;
		PlayerDataHandler<PlayerData>.Instance.SaveData();
		UnityEngine.Debug.Log("player cash decreased by " + num);
	}

	public void ClearPlayerData()
	{
		PlayerDataHandler<PlayerData>.Clear();
		UnityEngine.Debug.Log("player data cleared");
	}
}
