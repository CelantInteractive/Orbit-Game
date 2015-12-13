using UnityEngine;
using System;
using System.Collections.Generic;

public enum Controls
{
	UNASSIGNED = 0,
	CONSOLE = 1,
	SUBMIT = 2,
	SHIP_X_AXIS = 3
};

public class GameInput : MonoBehaviour
{

	public static Dictionary<KeyCode, Controls> KeyMappings = new Dictionary<KeyCode, Controls>();

	public static Dictionary<Controls, bool> KeyStates = new Dictionary<Controls, bool>();
	public static Dictionary<Controls, int> KeyStateDown = new Dictionary<Controls, int>();
	public static Dictionary<Controls, int> KeyStateUp = new Dictionary<Controls, int>();


	public static Controls BindNext = Controls.UNASSIGNED;

	void Start()
	{
		foreach(Controls control in Enum.GetValues(typeof(Controls)))
		{
			KeyStates.Add(control, false);
		}
	}

	void OnGUI()
	{
		Event curEvent = Event.current;

		switch (curEvent.type)
		{
			case (EventType.KeyDown):
				{
					Controls control = GetMappingForKey(curEvent.keyCode);
					if (control != Controls.UNASSIGNED)
					{
						if(KeyStates[control] == false)
						{
							KeyStateDown.Add(control, Time.frameCount);
						}
						KeyStates[control] = true;
						//Debug.Log(string.Format("Key for {0} is down", control));
					}
				}
				break;
			case (EventType.KeyUp):
				{
					Controls control = GetMappingForKey(curEvent.keyCode);
					if (control != Controls.UNASSIGNED)
					{
						if (KeyStates[control] == true)
						{
							KeyStateUp.Add(control, Time.frameCount);
						}
						KeyStates[control] = false;
						//Debug.Log(string.Format("Key for {0} is up", control));
					}
				}
				break;
		}
	}

	void FixedUpdate()
	{
		var curFrame = Time.frameCount;
		var keysDown = new List<Controls>(KeyStateDown.Keys);

		foreach (Controls entry in keysDown)
		{
			int value = KeyStateDown[entry];
			if (value <= curFrame)
			{
				KeyStateDown.Remove(entry);
			}
		}

		var keysUp = new List<Controls>(KeyStateUp.Keys);

		foreach (Controls entry in keysUp)
		{
			int value = KeyStateUp[entry];
			if (value <= curFrame)
			{
				KeyStateUp.Remove(entry);
			}
		}
	}

	public static bool GetKey(Controls control)
	{
		if (KeyStates.ContainsKey(control))
		{
			if (KeyStates[control])
			{
				return true; // Key is being pressed
			}
			else
			{
				return false; // Key is not being pressed
			}
		}
		else
		{
			return false; // No key assigned to control
		}
	}

	public static bool GetKeyDown(Controls control)
	{
		if(KeyStateDown.ContainsKey(control))
		{
			return true;
		} else
		{
			return false;
		}
	}

	public static bool GetKeyUp(Controls control)
	{
		if (KeyStateUp.ContainsKey(control))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static KeyCode GetBinding(Controls control)
	{
		KeyCode key = KeyCode.None;

		foreach(KeyValuePair<KeyCode, Controls> entry in KeyMappings)
		{
			if (entry.Value.Equals(control) & !entry.Value.Equals(Controls.UNASSIGNED))
			{
				key = entry.Key;
			}
		}
		return key;
	}

	public static void Bind(Controls control, KeyCode key)
	{
		var keys = new List<KeyCode>(KeyMappings.Keys);

		if (KeyMappings.ContainsKey(key))
		{
			foreach (KeyCode entry in keys)
			{
				Controls value = KeyMappings[entry];
				if (value == control)
				{
					Debug.Log(string.Format("Duplicate controls, unbinding '{0}' from key {1}", value, entry));
					KeyMappings[entry] = Controls.UNASSIGNED;
					return;
				}
			}

			KeyMappings[key] = control;
			Debug.Log(string.Format("Succesfully bound '{0}' to key {1}", control, key));
		} else
		{
			foreach (KeyCode entry in keys)
			{
				Controls value = KeyMappings[entry];
				if (value == control)
				{
					Debug.Log(string.Format("Duplicate controls, unbinding '{0}' from key '{1}'", value, entry));
					KeyMappings[entry] = Controls.UNASSIGNED;
					return;
				}
			}

			KeyMappings.Add(key, control);
			Debug.Log(string.Format("Succesfully bound '{0}' to key {1}", control, key));
		}
	}

	public static void Unbind(KeyCode key)
	{
		var keys = new List<KeyCode>(KeyMappings.Keys);

		if (KeyMappings.ContainsKey(key))
		{
			Controls control = KeyMappings[key];
			KeyStates[control] = false;
			KeyMappings[key] = Controls.UNASSIGNED;
			return;
		}

		Debug.Log("No control was bound to specified key");
	}

	public static void UnbindAll()
	{
		KeyMappings.Clear();
		KeyStates.Clear();

		Debug.Log("All bindings have been reset");
	}

	private static Controls GetMappingForKey(KeyCode key)
	{
		if (KeyMappings.ContainsKey(key))
		{
			return KeyMappings[key];
		}
		else
		{
			return Controls.UNASSIGNED;
		}
	}
}
