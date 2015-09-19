using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Controls
{
    UNASSIGNED = 0,
    SHIP_X_AXIS = 1,
};

public class GameInput : MonoBehaviour
{

    public static Dictionary<KeyCode, Controls> KeyMappings = new Dictionary<KeyCode, Controls>();

    public static Dictionary<Controls, bool> KeyStates = new Dictionary<Controls, bool>();

    void Update()
    {
        Event curEvent = Event.current;

        switch (curEvent.type)
        {
            case (EventType.KeyDown):
                {
                    Controls control = GetControlForKey(curEvent.keyCode);
                    if (KeyStates.ContainsKey(control))
                    {
                        KeyStates[control] = true;
                    }
                    else
                    {
                        KeyStates.Add(control, true);
                    }

                    Debug.Log(string.Format("Key for ", control, " is down"));
                }
                break;
            case (EventType.KeyUp):
                {
                    Controls control = GetControlForKey(curEvent.keyCode);
                    if (KeyStates.ContainsKey(control))
                    {
                        KeyStates[control] = false;
                    }
                    else
                    {
                        KeyStates.Add(control, false);
                    }

                    Debug.Log(string.Format("Key for ", control, " is up"));
                }
                break;
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

    public static KeyCode GetBinding(Controls control)
    {
        KeyCode key = KeyCode.None;

        foreach(KeyValuePair<KeyCode, Controls> entry in KeyMappings)
        {
            if (entry.Value.Equals(control))
            {
                key = entry.Key;
            }
        }
        return key;
    }

    public static void BindControl(Controls control, KeyCode key)
    {
        var keys = new List<KeyCode>(KeyMappings.Keys);

        if (KeyMappings.ContainsKey(key))
        {
            foreach (KeyCode entry in keys)
            {
                Controls value = KeyMappings[entry];
                if (value == control)
                {
                    Debug.Log(string.Format("Duplicate controls, unbinding '", value, "' from key '", entry));
                    KeyMappings[entry] = Controls.UNASSIGNED;
                    return;
                }
            }

            KeyMappings[key] = control;
            Debug.Log(string.Format("Succesfully bound '", control, "' to key ", key));
        } else
        {
            foreach (KeyCode entry in keys)
            {
                Controls value = KeyMappings[entry];
                if (value == control)
                {
                    Debug.Log(string.Format("Duplicate controls, unbinding '", value, "' from key '", entry));
                    KeyMappings[entry] = Controls.UNASSIGNED;
                    return;
                }
            }

            KeyMappings.Add(key, control);
            Debug.Log(string.Format("Succesfully bound '", control, "' to key ", key));
        }
    }

    private static Controls GetControlForKey(KeyCode key)
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
