using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AClockworkBerry
{
	public class ScreenLogger : MonoBehaviour
	{
		private class LogMessage
		{
			public string Message;

			public LogType Type;

			public LogMessage(string msg, LogType type)
			{
				Message = msg;
				Type = type;
			}
		}

		public enum LogAnchor
		{
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight
		}

		public static bool IsPersistent = true;

		private static ScreenLogger instance;

		private static bool instantiated = false;

		public bool ShowLog = true;

		public bool ShowInEditor = true;

		[Tooltip("Height of the log area as a percentage of the screen height")]
		[Range(0.3f, 1f)]
		public float Height = 0.5f;

		[Tooltip("Width of the log area as a percentage of the screen width")]
		[Range(0.3f, 1f)]
		public float Width = 0.5f;

		public int Margin = 20;

		public LogAnchor AnchorPosition = LogAnchor.BottomLeft;

		public int FontSize = 14;

		[Range(0f, 1f)]
		public float BackgroundOpacity = 0.5f;

		public Color BackgroundColor = Color.black;

		public bool LogMessages = true;

		public bool LogWarnings = true;

		public bool LogErrors = true;

		public Color MessageColor = Color.white;

		public Color WarningColor = Color.yellow;

		public Color ErrorColor = new Color(1f, 0.5f, 0.5f);

		public bool StackTraceMessages;

		public bool StackTraceWarnings;

		public bool StackTraceErrors = true;

		private static Queue<LogMessage> queue = new Queue<LogMessage>();

		private GUIStyle styleContainer;

		private GUIStyle styleText;

		private int padding = 5;

		private bool destroying;

		private bool styleChanged = true;

		public static ScreenLogger Instance
		{
			get
			{
				if (instantiated)
				{
					return instance;
				}
				instance = (UnityEngine.Object.FindObjectOfType(typeof(ScreenLogger)) as ScreenLogger);
				if (instance == null)
				{
					try
					{
						instance = (UnityEngine.Object.Instantiate(Resources.Load("ScreenLoggerPrefab", typeof(ScreenLogger))) as ScreenLogger);
					}
					catch
					{
						UnityEngine.Debug.Log("Failed to load default Screen Logger prefab...");
						instance = new GameObject("ScreenLogger", typeof(ScreenLogger)).GetComponent<ScreenLogger>();
					}
					if (instance == null)
					{
						UnityEngine.Debug.LogError("Problem during the creation of ScreenLogger");
					}
					else
					{
						instantiated = true;
					}
				}
				else
				{
					instantiated = true;
				}
				return instance;
			}
		}

		public void Awake()
		{
			if (UnityEngine.Object.FindObjectsOfType<ScreenLogger>().Length > 1)
			{
				UnityEngine.Debug.Log("Destroying ScreenLogger, already exists...");
				destroying = true;
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			InitStyles();
			if (IsPersistent)
			{
				Object.DontDestroyOnLoad(this);
			}
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			styleChanged = true;
		}

		private void InitStyles()
		{
			Texture2D texture2D = new Texture2D(1, 1);
			BackgroundColor.a = BackgroundOpacity;
			texture2D.SetPixel(0, 0, BackgroundColor);
			texture2D.Apply();
			styleContainer = new GUIStyle();
			styleContainer.normal.background = texture2D;
			styleContainer.wordWrap = false;
			styleContainer.padding = new RectOffset(padding, padding, padding, padding);
			styleText = new GUIStyle();
			styleText.fontSize = FontSize;
			styleChanged = false;
		}

		private void OnEnable()
		{
			if (ShowInEditor || !Application.isEditor)
			{
				queue = new Queue<LogMessage>();
				Application.logMessageReceived += HandleLog;
			}
		}

		private void OnDisable()
		{
			if (!destroying)
			{
				Application.logMessageReceived -= HandleLog;
			}
		}

		private void Update()
		{
			if (ShowInEditor || !Application.isEditor)
			{
				int num = (int)(((float)(Screen.height - 2 * Margin) * Height - (float)(2 * padding)) / styleText.lineHeight);
				while (queue.Count > num)
				{
					queue.Dequeue();
				}
			}
		}

		private void OnGUI()
		{
			if (ShowLog && (ShowInEditor || !Application.isEditor))
			{
				if (styleChanged)
				{
					InitStyles();
				}
				float width = (float)(Screen.width - 2 * Margin) * Width;
				float height = (float)(Screen.height - 2 * Margin) * Height;
				float x = 1f;
				float y = 1f;
				switch (AnchorPosition)
				{
				case LogAnchor.BottomLeft:
					x = Margin;
					y = (float)Margin + (float)(Screen.height - 2 * Margin) * (1f - Height);
					break;
				case LogAnchor.BottomRight:
					x = (float)Margin + (float)(Screen.width - 2 * Margin) * (1f - Width);
					y = (float)Margin + (float)(Screen.height - 2 * Margin) * (1f - Height);
					break;
				case LogAnchor.TopLeft:
					x = Margin;
					y = Margin;
					break;
				case LogAnchor.TopRight:
					x = (float)Margin + (float)(Screen.width - 2 * Margin) * (1f - Width);
					y = Margin;
					break;
				}
				GUILayout.BeginArea(new Rect(x, y, width, height), styleContainer);
				foreach (LogMessage item in queue)
				{
					switch (item.Type)
					{
					case LogType.Warning:
						styleText.normal.textColor = WarningColor;
						break;
					case LogType.Log:
						styleText.normal.textColor = MessageColor;
						break;
					case LogType.Error:
					case LogType.Assert:
					case LogType.Exception:
						styleText.normal.textColor = ErrorColor;
						break;
					default:
						styleText.normal.textColor = MessageColor;
						break;
					}
					GUILayout.Label(item.Message, styleText);
				}
				GUILayout.EndArea();
			}
		}

		private void HandleLog(string message, string stackTrace, LogType type)
		{
			if ((type == LogType.Assert && !LogErrors) || (type == LogType.Error && !LogErrors) || (type == LogType.Exception && !LogErrors) || (type == LogType.Log && !LogMessages) || (type == LogType.Warning && !LogWarnings))
			{
				return;
			}
			string[] array = message.Split('\n');
			foreach (string msg in array)
			{
				queue.Enqueue(new LogMessage(msg, type));
			}
			if ((type == LogType.Assert && !StackTraceErrors) || (type == LogType.Error && !StackTraceErrors) || (type == LogType.Exception && !StackTraceErrors) || (type == LogType.Log && !StackTraceMessages) || (type == LogType.Warning && !StackTraceWarnings))
			{
				return;
			}
			array = stackTrace.Split('\n');
			foreach (string text in array)
			{
				if (text.Length != 0)
				{
					queue.Enqueue(new LogMessage("  " + text, type));
				}
			}
		}

		public void InspectorGUIUpdated()
		{
			styleChanged = true;
		}
	}
}
