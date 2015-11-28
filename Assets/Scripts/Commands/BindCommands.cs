using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Commands
{
	public partial class CommandManager
	{
		private List<char> binds = new List<char>();

		public CommandResult Bind(char key)
		{
			Debug.Log("BINDING KEY: " + key);
			if (!binds.Contains(key))
			{
				binds.Add(key);
				return CommandResult.Success;
			}

			return CommandResult.Failure;
		}

		public CommandResult Unbind(char key)
		{
			Debug.Log("UNBINDING KEY: " + key);
			if (!binds.Contains(key))
			{
				return CommandResult.Failure;
			}

			binds.Remove(key);
			return CommandResult.Success;
		}
	}
}
