using StackTheBlockArslan;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : PnCUIManger
{
	[Header("Home screen")]
	public Image soundImage;

	public Sprite soundOnSprite;

	public Sprite soundOffSprite;

	public GameObject removeAdBtn;

	public Button ARBtn;

	public Sprite AROnSprite;

	public Sprite AROffSprite;

	[Header("HUD")]
	public Text scoretext_HUD;

	public Text cashtext_HUD;

	[Header("Shop")]
	public Text cashText;

	public Text buyPopUpText;

	[Header("Game over")]
	public Text scoreText_Gameover;

	public Text highScoreText_GameOver;

	public Text cashText_GameOver;

	public static UIManager Instance;

	protected override void AwakeInit()
	{
		Instance = this;
		GameManager instance = LazySingleton<GameManager>.Instance;
		instance.GameInitialized = (Action)Delegate.Combine(instance.GameInitialized, new Action(OpenHomeScreen));
		GameManager instance2 = LazySingleton<GameManager>.Instance;
		instance2.GameStarted = (Action)Delegate.Combine(instance2.GameStarted, new Action(OpenHUDScreen));
		GameManager instance3 = LazySingleton<GameManager>.Instance;
		instance3.GameContinued = (Action)Delegate.Combine(instance3.GameContinued, new Action(OpenGameContinueScreen));
		GameManager instance4 = LazySingleton<GameManager>.Instance;
		instance4.GameOver = (Action)Delegate.Combine(instance4.GameOver, new Action(OpenGameOverScreen));
	}

	private void OnDestroy()
	{
		GameManager instance = LazySingleton<GameManager>.Instance;
		instance.GameInitialized = (Action)Delegate.Remove(instance.GameInitialized, new Action(OpenHomeScreen));
		GameManager instance2 = LazySingleton<GameManager>.Instance;
		instance2.GameStarted = (Action)Delegate.Remove(instance2.GameStarted, new Action(OpenHUDScreen));
		GameManager instance3 = LazySingleton<GameManager>.Instance;
		instance3.GameContinued = (Action)Delegate.Remove(instance3.GameContinued, new Action(OpenGameContinueScreen));
		GameManager instance4 = LazySingleton<GameManager>.Instance;
		instance4.GameOver = (Action)Delegate.Remove(instance4.GameOver, new Action(OpenGameOverScreen));
	}

	private void OpenHomeScreen()
	{
		OpenScreen(UIScreensList.HomeScreen);
	}

	private void OpenHUDScreen()
	{
		SoundManager.Instance.playSound(AudioClips.UI);
		OpenScreen(UIScreensList.HUD);
	}

	private void OpenGameContinueScreen()
	{
		Instance.OpenScreen(UIScreensList.ContinueGame);
	}

	private void OpenGameOverScreen()
	{
		OpenScreen(UIScreensList.GameOver);
	}

	public void UpdateHudData(int score, float cash)
	{
		scoretext_HUD.text = score.ToString();
		cashtext_HUD.text = cash.ToString();
	}

	public void UpdateGameOverData(int score, int highScore, float cash)
	{
		scoreText_Gameover.text = score.ToString();
		highScoreText_GameOver.text = highScore.ToString();
		cashText_GameOver.text = cash.ToString();
	}

	public void ToggleOpenCostPopUp(bool status, string text = "buy this skin?")
	{
		if (status)
		{
			buyPopUpText.text = text;
			OpenModal(UIScreensList.BuyPopUp);
		}
		else
		{
			CloseModal();
		}
	}

	public void ToggleSoundSprite(bool isSoundOn)
	{
		soundImage.sprite = (isSoundOn ? soundOnSprite : soundOffSprite);
	}

	public void ToggleARModeSprite(bool isARModeOn)
	{
		ARBtn.image.sprite = (isARModeOn ? AROnSprite : AROffSprite);
	}
}
