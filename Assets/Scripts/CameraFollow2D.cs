using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;

    private void Update()
    {
        if (this.Target == null)
        {
            return;
        }

        Vector3 newPosition = this.Target.position;
        newPosition.z = -10;
        this.transform.position = Vector3.Slerp(this.transform.position, newPosition, this.FollowSpeed * Time.deltaTime);
    }
}
