using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Orbit.Scripts.Commands
{
	public enum CommandResult
	{
		Success,
		NotFound,
		Failure
	}

	public partial class CommandManager
	{
		public CommandResult Execute(string command, params object[] parameters)
		{
			MethodInfo info = typeof(CommandManager).GetMethods().FirstOrDefault(m => m.Name.ToUpper() == command.ToUpper());

			if (info == null)
			{
				return CommandResult.NotFound;
			}

			ParameterInfo[] paramInfo = info.GetParameters();
			if (parameters.Length < paramInfo.Count(p => !p.IsOptional))
			{
				Debug.LogErrorFormat("Not enough parameters supplied. Expected {0} but received {1}.", paramInfo.Length, parameters.Length);
				return CommandResult.Failure;
			}

			object[] newParams = new object[paramInfo.Length];
			for (int i = 0; i < paramInfo.Length; i++)
			{
				Type t = paramInfo[i].ParameterType;
				if (parameters[i].GetType() != t)
				{
					try
					{
						newParams[i] = Convert.ChangeType(parameters[i], t);
					}
					catch (InvalidCastException)
					{
						Debug.LogErrorFormat("Invalid parameter type provided at index {0}. Expected parameter of type {1} but received type {2}.",
							i, t, parameters[i].GetType());

						return CommandResult.Failure;
					}
					catch (FormatException)
					{
						Debug.LogErrorFormat("Invalid parameter format provided at index {0}. Expected format for parameter of type {1} but received '{2}'.",
							i, t, parameters[i]);

						return CommandResult.Failure;
					}
				}
				else
				{
					newParams[i] = parameters[i];
				}
			}

			return (CommandResult)info.Invoke(this, newParams.Length > 0 ? newParams : null);
		}

		public CommandResult Echo(params string[] parameters)
		{
			Debug.Log(String.Concat(parameters));
			return CommandResult.Success;
		}
	}
}
