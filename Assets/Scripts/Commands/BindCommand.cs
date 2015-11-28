using UnityEngine;

namespace Assets.Scripts.Commands
{
	public partial class CommandManager
	{
		public CommandResult Bind(char key)
		{
			Debug.Log("BINDING KEY: " + key);
			return CommandResult.Success;
		}
	}
}
