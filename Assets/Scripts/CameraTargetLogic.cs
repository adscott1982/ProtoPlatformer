using System;
using UnityEngine;

public class CameraTargetLogic : MonoBehaviour
{
    public GameObject PlayerGameObject;
    public float VerticalLookDistance = 15f;

    private PlayerMovement playerMovement;

	// Use this for initialization
	private void Start ()
	{
	    this.playerMovement = this.PlayerGameObject.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	private void Update ()
	{
	    var targetX = this.playerMovement.Velocity.x;
	    var targetY = this.playerMovement.Velocity.y;
        var targetPos = new Vector2(targetX, targetY);

        // If the player is looking up or down, look the camera up or down
	    if (this.playerMovement.Velocity.magnitude < Tools.FloatEqualityTolerance)
	    {
	        if (Math.Abs(this.playerMovement.InputAxes.y) > 0)
	        {
	            targetPos.y += Math.Sign(this.playerMovement.InputAxes.y) * VerticalLookDistance;
	        }
	    }

        this.transform.localPosition = Vector2.Lerp(this.transform.localPosition, targetPos, Time.deltaTime);
        //this.transform.localPosition = targetPos;

    }
}
