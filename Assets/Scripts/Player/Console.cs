﻿using Orbit.Commands;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A console to display Unity's debug logs in-game.
/// </summary>
class Console : MonoBehaviour
{
	struct Log
	{
		public string message;
		public string stackTrace;
		public LogType type;
	}

	#region Inspector Settings

	/// <summary>
	/// Whether to only keep a certain number of logs.
	///
	/// Setting this can be helpful if memory usage is a concern.
	/// </summary>
	public bool restrictLogCount = false;

	/// <summary>
	/// Number of logs to keep before removing old ones.
	/// </summary>
	public int maxLogs = 1000;

	#endregion

	readonly List<Log> logs = new List<Log>();
	public string input;
	Vector2 scrollPosition;
	bool visible;
	bool follow;

	CommandManager commandManager = new CommandManager();

	// Visual elements:

	static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
	{
		{ LogType.Assert, Color.white },
		{ LogType.Error, Color.red },
		{ LogType.Exception, Color.red },
		{ LogType.Log, Color.white },
		{ LogType.Warning, Color.yellow },
	};

	const string windowTitle = "Console";
	const int margin = 20;
	static readonly GUIContent followLabel = new GUIContent("Auto-scroll", "Auto-scroll to the latest messages.");

	readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);
	Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), (Screen.height / 2) - (margin * 2));

	void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	void Start()
	{
		GameInput.Bind(Controls.CONSOLE, KeyCode.BackQuote);
		GameInput.Bind(Controls.SUBMIT, KeyCode.Return);
		GameInput.Bind(Controls.SHIP_X_AXIS_POS, KeyCode.W);
	}

	void Update()
	{
		if (GameInput.GetKeyDown(Controls.CONSOLE))
		{
			visible = !visible;
		}
		if (follow)
		{
			scrollPosition.y = Mathf.Infinity;
		}
	}

	void OnGUI()
	{
		if (!visible)
		{
			return;
		}

		if (visible)
		{
			// Manually handle the keyDown event, as the TextField eats up the event
			if (Event.current.isKey)
			{
				if (Event.current.keyCode == KeyCode.Return)
				{
					ProcessCommand(input);
					input = "";
				}
			}
		}

		windowRect = GUILayout.Window(1, windowRect, DrawConsoleWindow, windowTitle);
	}

	/// <summary>
	/// Displays a window that lists the recorded logs.
	/// </summary>
	/// <param name="windowID">Window ID.</param>
	void DrawConsoleWindow(int windowID)
	{
		DrawLogsList();
		DrawInput();
		DrawToolbar();

		// Allow the window to be dragged by its title bar.
		GUI.DragWindow(titleBarRect);
	}

	/// <summary>
	/// Displays a scrollable list of logs.
	/// </summary>
	void DrawLogsList()
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		// Iterate through the recorded logs.
		for (var i = 0; i < logs.Count; i++)
		{
			var log = logs[i];


			GUI.contentColor = logTypeColors[log.type];
			GUILayout.Label(log.message);
		}

		GUILayout.EndScrollView();

		// Ensure GUI colour is reset before drawing other components.
		GUI.contentColor = Color.white;
	}

	/// <summary>
	/// Displays options for filtering and changing the logs list.
	/// </summary>
	void DrawInput()
	{
		GUILayout.BeginHorizontal();

		input = GUILayout.TextField(input);

		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// Displays options for filtering and changing the logs list.
	/// </summary>
	void DrawToolbar()
	{
		GUILayout.BeginHorizontal();

		follow = GUILayout.Toggle(follow, followLabel, GUILayout.ExpandWidth(false));

		GUILayout.EndHorizontal();
	}



	/// <summary>
	/// Records a log from the log callback.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="stackTrace">Trace of where the message came from.</param>
	/// <param name="type">Type of message (error, exception, warning, assert).</param>
	void HandleLog(string message, string stackTrace, LogType type)
	{
		logs.Add(new Log
		{
			message = message,
			stackTrace = stackTrace,
			type = type,
		});

		TrimExcessLogs();
	}

	/// <summary>
	/// Removes old logs that exceed the maximum number allowed.
	/// </summary>
	void TrimExcessLogs()
	{
		if (!restrictLogCount)
		{
			return;
		}

		var amountToRemove = Mathf.Max(logs.Count - maxLogs, 0);

		if (amountToRemove == 0)
		{
			return;
		}

		logs.RemoveRange(0, amountToRemove);
	}

	void ProcessCommand(string str)
	{
		Debug.Log(str);
		string command = "";
		string[] parameters;

		if (string.IsNullOrEmpty(str))
		{
			return;
		}

		parameters = str.Split(' ');
		command = parameters[0];
		parameters = parameters.Skip(1).ToArray();

		CommandResult result = commandManager.Execute(command, parameters);

		if (result == CommandResult.NotFound)
		{
			Debug.LogFormat("command '{0}' is undefined", command);
		}
	}
}