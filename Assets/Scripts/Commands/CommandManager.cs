using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Commands
{
	public enum CommandResult
	{
		Success,
		NotFound,
		Failure
	}

	public class CommandManager
	{
		public CommandResult Execute(string command, params object[] parameters)
		{
			MethodInfo info = typeof(CommandManager).GetMethod(command);

			if (info == null)
			{
				return CommandResult.NotFound;
			}

			ParameterInfo[] paramInfo = info.GetParameters();
			if (parameters.Length < paramInfo.Count(p => !p.IsOptional))
			{
				Debug.LogError($"Not enough parameters supplied. Expected {paramInfo.Length} but received {parameters.Length}.");
				return CommandResult.Failure;
			}

			object[] newParams = new object[paramInfo.Length];
			for (int i = 0; i < paramInfo.Length; i++)
			{
				Type t = paramInfo[i].ParameterType;
				if (parameters[i].GetType() != t)
				{
					newParams[i] = Convert.ChangeType(parameters[i], t);
				}
				else
				{
					newParams[i] = parameters[i];
				}
			}

			return (CommandResult)info.Invoke(this, newParams.Length > 0 ? newParams : null);
		}

		public CommandResult Bind(char key)
		{
			Debug.Log("BINDING KEY: " + key);
			return CommandResult.Success;
		}
	}
}
