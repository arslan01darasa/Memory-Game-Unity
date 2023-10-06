using StackTheBlockArslan;
using UnityEngine;

public class SoundManager : PnCSoundManger
{
	public static SoundManager m_instance;

	private bool isSoundOn = true;

	public static SoundManager Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = Object.FindObjectOfType<SoundManager>();
			}
			return m_instance;
		}
	}

	private void Start()
	{
		isSoundOn = PlayerDataHandler<PlayerData>.Instance.soundSetting;
		applySoundSetting();
	}

	public void toggleSoundOnOff()
	{
		playSound(AudioClips.UI);
		isSoundOn = !isSoundOn;
		applySoundSetting();
		PlayerDataHandler<PlayerData>.Instance.soundSetting = isSoundOn;
		PlayerDataHandler<PlayerData>.Instance.SaveData();
	}

	private void applySoundSetting()
	{
		AudioListener.volume = (isSoundOn ? 1 : 0);
		UIManager.Instance.ToggleSoundSprite(isSoundOn);
	}
}
