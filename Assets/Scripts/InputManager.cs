using UnityEngine;

public static class InputManager
{
    public static float ThumbstickDeadzone = 0.5f;
    public static float TriggerDeadzone = 0.12f;

    public static Vector2 GetMainAxes()
    {
        var vector = new Vector2(Input.GetAxis("MainXAxis"), Input.GetAxis("MainYAxis"));

        if (Mathf.Abs(vector.x) < ThumbstickDeadzone)
        {
            vector.x = 0f;
        }

        if (Mathf.Abs(vector.y) < ThumbstickDeadzone)
        {
            vector.y = 0f;
        }

        return vector;
    }

    public static Vector2 GetRightThumbstick()
    {
        var vector = new Vector2(Input.GetAxis("RThumbstickX"), Input.GetAxis("RThumbstickY"));

        if (Mathf.Abs(vector.x) < ThumbstickDeadzone)
        {
            vector.x = 0f;
        }

        if (Mathf.Abs(vector.y) < ThumbstickDeadzone)
        {
            vector.y = 0f;
        }

        return vector;
    }

    public static float GetLeftTrigger()
    {
        var leftTriggerValue = Input.GetAxis("LeftTrigger");

        if (leftTriggerValue < TriggerDeadzone)
        {
            leftTriggerValue = 0f;
        }

        return leftTriggerValue;
    }

    public static bool GetButton1Down()
    {
        return Input.GetButtonDown("Button1");
    }

    public static bool GetButton1()
    {
        return Input.GetButton("Button1");
    }

    public static bool GetButton2Down()
    {
        return Input.GetButtonDown("Button2");
    }

    public static bool GetButton2()
    {
        return Input.GetButton("Button2");
    }

    public static bool GetButton3Down()
    {
        return Input.GetButtonDown("Button3");
    }

    public static bool GetButton3()
    {
        return Input.GetButton("Button3");
    }

    public static bool GetButton4Down()
    {
        return Input.GetButtonDown("Button4");
    }

    public static bool GetButton4()
    {
        return Input.GetButton("Button4");
    }

    public static bool GetExitButtonDown()
    {
        return Input.GetButtonDown("ExitButton");
    }

    public static bool GetExitButton()
    {
        return Input.GetButton("ExitButton");
    }
}
