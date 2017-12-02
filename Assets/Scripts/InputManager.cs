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
}
