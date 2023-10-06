using System.Collections.Generic;
using UnityEngine;

namespace StackTheBlockArslan
{
	public class PnCSoundManger : MonoBehaviour
	{
		public List<Audio> audios;

		private AudioSource audioSource;

		private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			foreach (Audio audio in audios)
			{
				soundDictionary.Add(audio.audioName, audio.clip);
			}
		}

		public void playSound(AudioClips clipName)
		{
			PlaySound(clipName.ToString());
		}

		public void PlaySound(string clipName)
		{
			AudioClip value = null;
			if (soundDictionary.TryGetValue(clipName, out value))
			{
				audioSource.clip = value;
				audioSource.Play();
			}
			else
			{
				UnityEngine.Debug.LogError("audio clip does not exist");
			}
		}

		public void playSound(int index)
		{
			audioSource.clip = audios[index].clip;
			audioSource.Play();
		}
	}
}
