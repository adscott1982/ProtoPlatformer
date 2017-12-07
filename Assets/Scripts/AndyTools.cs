using UnityEngine;

public static class AndyTools
{
    public const float FloatEqualityTolerance = 0.000001f;

    public static Vector3 AddVector2(this Vector3 vector3, Vector2 vector2)
    {
        return new Vector3(vector3.x + vector2.x, vector3.y + vector2.y);
    }

    public static float DirectionDegrees(this Vector2 vector)
    {
        return (Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg);
    }

    public static float RelativeDirectionDegrees(this Vector2 vector, Vector2 relativeVector)
    {
        // Calculate the angle of the relative vector
        var relativeVectorAngle = relativeVector.DirectionDegrees();

        // Rotate the incoming vector by the angle difference
        var rotatedVector = vector.Rotate(-relativeVectorAngle);

        // return the angle
        return (Mathf.Atan2(rotatedVector.y, rotatedVector.x) * Mathf.Rad2Deg);
    }

    public static Vector2 Rotate(this Vector2 vector, float degrees)
    {
        var sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        var cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        var tx = vector.x;
        var ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }

    public static bool IsInRange(this float value, float firstLimit, float secondLimit)
    {
        if (value >= firstLimit && value <= secondLimit)
        {
            return true;
        }

        return value >= secondLimit && value <= firstLimit;
    }

    public static float GetLerpedRotationDelta(float currentRotation, float targetRotation, float interpolationValue, float maxRotationSpeed)
    {
        var lerpedRotation = Mathf.LerpAngle(currentRotation, targetRotation, interpolationValue);
        var rotationDelta = lerpedRotation - currentRotation;

        if (Mathf.Abs(rotationDelta) > maxRotationSpeed)
        {
            rotationDelta = Mathf.Sign(rotationDelta) * maxRotationSpeed;
        }

        return rotationDelta;
    }

    public static Quaternion AsEulerZ(this float zRotation)
    {
        return Quaternion.Euler(new Vector3(0, 0, zRotation));
    }

    public static Vector2 RadianToVector2(this float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(this float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    public static Bounds OrthographicBounds(this Camera camera)
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public static Rect OrthographicRectInWorldSpace(this Camera camera)
    {
        var height = camera.orthographicSize * 2;
        var width = height * Screen.width / Screen.height;
        var x = camera.transform.position.x - (width / 2);
        var y = camera.transform.position.y - (height / 2);

        return new Rect(x, y, width, height);
    }

    public static Vector2 AsVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }
}
