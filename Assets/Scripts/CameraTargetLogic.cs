using System;
using UnityEngine;

public class CameraTargetLogic : MonoBehaviour
{
    public GameObject PlayerGameObject;
    public float VerticalLookDistance = 15f;
    public float LateralLeadMultiplier = 2f;

    private PlayerMovement playerMovement;
    private bool previousFacingLeft;

	// Use this for initialization
	private void Start ()
	{
	    this.playerMovement = this.PlayerGameObject.GetComponent<PlayerMovement>();
	    this.previousFacingLeft = this.playerMovement.IsFacingLeft;
	}
	
	// Update is called once per frame
	private void Update ()
	{
        var targetX = this.playerMovement.IsFacingLeft ? -this.playerMovement.Velocity.x : this.playerMovement.Velocity.x;
	    targetX = targetX * this.LateralLeadMultiplier;

        var targetY = this.playerMovement.Velocity.y;
        var targetPos = new Vector2(targetX, targetY);

	    if (this.previousFacingLeft != this.playerMovement.IsFacingLeft)
	    {
	        this.previousFacingLeft = this.playerMovement.IsFacingLeft;

	        var currPos = this.transform.localPosition;
	        currPos.x = -currPos.x;
	        this.transform.localPosition = currPos;
	    }

        // If the player is looking up or down, look the camera up or down
	    if (this.playerMovement.Velocity.magnitude < AndyTools.FloatEqualityTolerance)
	    {
	        if (Math.Abs(this.playerMovement.InputAxes.y) > 0)
	        {
	            targetPos.y += Math.Sign(this.playerMovement.InputAxes.y) * this.VerticalLookDistance;
	        }
	    }

        this.transform.localPosition = Vector2.Lerp(this.transform.localPosition, targetPos, Time.deltaTime);
    }
}
