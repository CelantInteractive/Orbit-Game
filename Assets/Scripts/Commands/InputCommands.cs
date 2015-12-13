using UnityEngine;
using System;
using System.Collections.Generic;

namespace Orbit.Commands
{
	public partial class CommandManager
	{

		public CommandResult Bind(string control, string key)
		{
			Controls controlValue = (Controls) Enum.Parse(typeof(Controls), control, true);
			KeyCode keyValue = (KeyCode)Enum.Parse(typeof(KeyCode), key, true);

			if (!Enum.IsDefined(typeof(KeyCode), keyValue))
			{
				Debug.LogWarning("Invalid keycode");
				return CommandResult.Failure;
			}
			if (!Enum.IsDefined(typeof(Controls), controlValue))
			{
				Debug.LogWarning("Invalid control");
				return CommandResult.Failure;
			}

			GameInput.Bind(controlValue, keyValue);
			return CommandResult.Success;
		}

		public CommandResult Unbind(string key)
		{
			KeyCode keyValue = (KeyCode)Enum.Parse(typeof(KeyCode), key, true);

			if (!Enum.IsDefined(typeof(KeyCode), keyValue))
			{
				Debug.LogWarning("Invalid keycode");
				return CommandResult.Failure;
			}

			GameInput.Unbind(keyValue);
			return CommandResult.Success;
		}

		public CommandResult Bindings()
		{
			Dictionary<KeyCode, Controls> bindings = GameInput.GetAllBindings();

			foreach (KeyValuePair<KeyCode, Controls> binding in bindings)
			{
				Debug.LogFormat("{0}: {1}", binding.Value, binding.Key);
			}

			return CommandResult.Success;
		}
	}
}
