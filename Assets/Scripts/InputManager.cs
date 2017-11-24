using UnityEngine;

public static class InputManager
{
    public static float ThumbstickDeadzone = 0.12f;
    public static float TriggerDeadzone = 0.12f;

    public static Vector2 GetLeftThumbstick()
    {
        var vector = new Vector2(Input.GetAxis("LThumbstickX"), Input.GetAxis("LThumbstickY"));

        if (vector.magnitude < ThumbstickDeadzone)
        {
            vector = Vector2.zero;
        }

        return vector;
    }

    public static Vector2 GetRightThumbstick()
    {
        var vector = new Vector2(Input.GetAxis("RThumbstickX"), Input.GetAxis("RThumbstickY"));

        if (vector.magnitude < ThumbstickDeadzone)
        {
            vector = Vector2.zero;
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

    public static bool GetButtonADown()
    {
        return Input.GetButtonDown("ButtonA");
    }

    public static bool GetButtonA()
    {
        return Input.GetButton("ButtonA");
    }
}
