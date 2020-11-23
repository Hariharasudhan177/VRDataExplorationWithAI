using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRGrabMonkeyPatch : MonoBehaviour
{
    void Awake()
    {
        SetButtonName(InputHelpers.Button.GripPressed, "GripButton");
        SetButtonName(InputHelpers.Button.TriggerPressed, "TriggerButton");

        var newGripName = GetButtonName(InputHelpers.Button.GripPressed);

        Debug.LogError("Monkey Patched Binary Grip Button Name: " + newGripName);

        var newTriggerName = GetButtonName(InputHelpers.Button.TriggerPressed);

        Debug.LogError("Monkey Patched Binary Trigger Button Name: " + newTriggerName);

    }

    public static void SetButtonName(InputHelpers.Button button, string name)
    {
        object buttonInfo = GetButtonInfoObject(button);
        var nameField = buttonInfo.GetType().GetField("name");
        nameField.SetValue(buttonInfo, name);

        SetButtonInfoObject(button, buttonInfo);
    }

    public static string GetButtonName(InputHelpers.Button button)
    {
        object buttonInfo = GetButtonInfoObject(button);
        var nameField = buttonInfo.GetType().GetField("name");
        var nameValue = nameField.GetValue(buttonInfo);
        return (string)nameValue;
    }

    private static object GetButtonInfoObject(InputHelpers.Button button)
    {
        var buttonListField = typeof(InputHelpers).GetField("s_ButtonData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        var buttonList = (System.Array)buttonListField.GetValue(null);

        var buttonInfo = buttonList.GetValue((int)button);
        return buttonInfo;
    }

    private static void SetButtonInfoObject(InputHelpers.Button button, object buttonInfo)
    {
        var buttonListField = typeof(InputHelpers).GetField("s_ButtonData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        var buttonList = (System.Array)buttonListField.GetValue(null);

        buttonList.SetValue(buttonInfo, (int)button);

    }

}