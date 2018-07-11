using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float GroundAccelerationForce = 100f;
    public float AirAccelerationForce = 50f;
    public float MaxSpeed = 4f;
    public float IdleDecelerationGrounded = 1f;
    public float IdleDecelerationAir = 1f;
    public float JumpForce = 5f;
    public float WallSlideSpeed = 2f;
    public float WallSlideDeceleration = 1f;
    public float MaxWalkingAngle = 50;
    public Vector2 Angle;

    private bool isGrounded;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    private bool jumpWasPressed;
    
    private bool isDecelerating;
    private bool isWallSliding;

    private Vector2 inputAxes;

    private List<ContactPoint2D> contactPoints;
    private Rigidbody2D rb;
    //private Animator animator;

    public Vector2 Velocity { get { return this.rb.velocity; } }
    public Vector2 InputAxes { get { return this.inputAxes; } }

    public bool IsBackContactPointTriggered { get; set; }

    public bool IsFrontContactPointTriggered { get; set; }

    public bool IsBottomContactPointTriggered { get; set; }

    public bool IsTopContactPointTriggered { get; set; }

    public bool IsFacingLeft { get; private set; }

    public float RenderRotation { get; private set; }

    // Use this for initialization
    private void Start()
	{
        //this.animator = this.GetComponent<Animator>();
        this.contactPoints = new List<ContactPoint2D>();
	    this.rb = this.GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        this.UpdateInput();
    }

    // Update is called once per frame
	private void FixedUpdate()
	{
	    this.CheckCollisions();
	    this.UpdateMovement();
        this.ResetButtonPresses();
        this.UpdateAnimator();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        var firstContact = other.contacts.First();
        this.contactPoints.Add(firstContact);
    }

    private void UpdateInput()
    {
        if (InputManager.GetButton1Down())
        {
            this.jumpWasPressed = true;
        }

        this.inputAxes = InputManager.GetMainAxes();

        if (InputManager.GetExitButtonDown())
        {
            // Exit game - should be moved to game manager object
            Debug.Log("Exit pressed, quitting game.");
            Application.Quit();
        }
    }

    private void ResetButtonPresses()
    {
        this.jumpWasPressed = false;
    }

    private void UpdateAnimator()
    {
        if (this.Velocity.x > AndyTools.FloatEqualityTolerance)
        {
            if (this.IsFacingLeft)
            {
                this.IsFacingLeft = false;
                this.transform.FlipXScale();
            }
        }
        else if (this.Velocity.x < -AndyTools.FloatEqualityTolerance)
        {
            if (!this.IsFacingLeft)
            {
                this.IsFacingLeft = true;
                this.transform.FlipXScale();
            }
        }

        //this.animator.SetFloat("xSpeed", Math.Abs(this.Velocity.x));
        //this.animator.SetFloat("vSpeed", this.Velocity.y);
        //this.animator.SetBool("OnGround", this.isGrounded);
        //this.animator.SetBool("FacingLeft", this.IsFacingLeft);
        //this.animator.SetBool("IsDecelerating", this.isDecelerating);
        //this.animator.SetBool("IsWallSliding", this.isWallSliding);
    }

    private void CheckCollisions()
    {
        foreach (var contactPoint in this.contactPoints)
        {
            var start = contactPoint.point;
            var end = contactPoint.point + (contactPoint.normal * 2);
            Debug.DrawLine(start, end, Color.red);

            var angle = contactPoint.normal.DirectionDegrees() - 90;
            Debug.Log("Contact angle = " + angle);
        }

        this.isTouchingLeftWall = this.contactPoints.Any(p => p.normal == Vector2.right);
        this.isTouchingRightWall = this.contactPoints.Any(p => p.normal == Vector2.left);

        if (this.contactPoints.Count == 0)
        {
            if (!this.IsBottomContactPointTriggered)
            {
                this.RenderRotation = 0f;
                this.isGrounded = false;
            }

            this.isTouchingLeftWall = false;
            this.isTouchingRightWall = false;

            this.contactPoints.Clear();

            return;
        }

        var allGroundAngles = this.contactPoints
            .Select(p => p.normal.RelativeDirectionDegrees(Vector2.up))
            .Where(a => a.IsInRange(-this.MaxWalkingAngle, this.MaxWalkingAngle)).ToList();

        if (allGroundAngles.Any())
        {
            var averageGroundAngle = allGroundAngles.Average();

            if (averageGroundAngle.IsInRange(-50, 50))
            {
                this.isGrounded = true;
                this.RenderRotation = this.IsFacingLeft ? -averageGroundAngle : averageGroundAngle;
            }
        }
        else if (!this.IsBottomContactPointTriggered)
        {
            this.isGrounded = false;
        }

        this.contactPoints.Clear();

        //var right = Vector2.right.DirectionDegrees();
        //var rightUp = new Vector2(1, 1).DirectionDegrees();
        //var up = Vector2.up.DirectionDegrees();
        //var leftUp = new Vector2(-1, 1).normalized.DirectionDegrees();
        //var left = Vector2.left.DirectionDegrees();
        //var leftDown = new Vector2(-1, -1).normalized.DirectionDegrees();
        //var down = Vector2.down.DirectionDegrees();
        //var rightDown = new Vector2(1, -1).normalized.DirectionDegrees();

        //Debug.Log(string.Format("Normal Right: {0} RightUp: {1} Up: {2} UpLeft: {3} Left: {4} LeftDown: {5} Down: {6} DownRight: {7}", right, rightUp, up, leftUp, left, leftDown, down, rightDown));

        //right = Vector2.right.RelativeDirectionDegrees(this.Angle);
        //rightUp = new Vector2(1, 1).normalized.RelativeDirectionDegrees(this.Angle);
        //up = Vector2.up.RelativeDirectionDegrees(this.Angle);
        //leftUp = new Vector2(-1, 1).normalized.RelativeDirectionDegrees(this.Angle);
        //left = Vector2.left.RelativeDirectionDegrees(this.Angle);
        //leftDown = new Vector2(-1, -1).normalized.RelativeDirectionDegrees(this.Angle);
        //down = Vector2.down.RelativeDirectionDegrees(this.Angle);
        //rightDown = new Vector2(1, -1).normalized.RelativeDirectionDegrees(this.Angle);

        //Debug.Log(string.Format("Adjusted Right: {0} RightUp: {1} Up: {2} UpLeft: {3} Left: {4} LeftDown: {5} Down: {6} DownRight: {7}", right, rightUp, up, leftUp, left, leftDown, down, rightDown));

        //foreach (var contactPoint in this.contactPoints)
        //{
        //    var start = contactPoint.point;
        //    var end = contactPoint.point + (contactPoint.normal * 2);
        //    Debug.DrawLine(start, end, Color.red);

        //    var angle = contactPoint.normal.DirectionDegrees() - 90;
        //    Debug.Log("Contact angle = " + angle);
        //}
    }

    private void UpdateMovement()
    {
        this.rb.ClampXMaxSpeed(this.MaxSpeed);

        this.isWallSliding = false;

        // If on a wall and not on the ground, and the left thumbstick is pressed in a lateral direction, clamp vertical falling speed to wall slide speed
        if (!this.isGrounded 
            && ((this.isTouchingLeftWall && this.inputAxes.x < 0)
            || (this.isTouchingRightWall && this.inputAxes.x > 0)))
        {
            if (this.rb.velocity.y < 0)
            {
                this.isWallSliding = true;

                // If wall fall speed is greater than the defined speed
                if (this.rb.velocity.y < -this.WallSlideSpeed)
                {
                    this.ControlWallSlideVerticalSpeed();
                }
            }
        }

        if (this.jumpWasPressed)
        {
            if (this.IsBottomContactPointTriggered) // Jump normally if on the ground
            {
                this.DoGroundJump();
            }
            else if (this.IsFrontContactPointTriggered) // Else if on a wall, wall jump
            {
                this.DoWallJump();
            }
        }

        // If the player has released the lateral movement, and the player is moving laterally, decelerate
        if (this.inputAxes.x.IsApproxZero() && !this.rb.velocity.x.IsApproxZero())
        {
            this.DecelerateLateralSpeed();
        }
        else
        {
            this.ApplyLateralForce(this.inputAxes.x);
        }
    }

    private void DecelerateLateralSpeed()
    {
        this.isDecelerating = true;
        this.rb.DecelerateX(this.isGrounded ? this.IdleDecelerationGrounded : this.IdleDecelerationAir);
    }

    private void ApplyLateralForce(float fraction)
    {
        var lateralForce = this.isGrounded ? this.GroundAccelerationForce : this.AirAccelerationForce;
        var lateralForceVector = new Vector2(fraction * lateralForce, 0);

        // Determine if the applied lateral force will decelerate the player
        var currentXSpeedSign = Math.Sign(this.rb.velocity.x);
        var appliedXForceSign = Math.Sign(lateralForceVector.x);

        if (currentXSpeedSign == 0 || currentXSpeedSign == appliedXForceSign)
        {
            this.isDecelerating = false;
        }
        else
        {
            this.isDecelerating = true;
        }

        this.rb.AddForce(lateralForceVector);
    }

    private void DoGroundJump()
    {
        this.StopFalling();
        this.rb.AddForce(Vector2.up * this.JumpForce);
    }

    private void DoWallJump()
    {
        this.StopFalling();
        this.StopHorizontalSpeed();
        var jumpDirection = this.IsFacingLeft ? new Vector2(1f, 1.2f) : new Vector2(-1f, 1.2f);
        jumpDirection = jumpDirection.normalized;
        this.rb.AddForce(jumpDirection * this.JumpForce);
    }

    private void ControlWallSlideVerticalSpeed()
    {
        if (!(this.rb.velocity.y < -this.WallSlideSpeed))
        {
            return;
        }

        // If wall fall speed is greater than the max slide speed clamp the vertical velocity
        this.rb.ClampYMaxSpeed(this.WallSlideSpeed);
    }

    private void StopFalling()
    {
        if (!(this.rb.velocity.y < 0)) return;

        // If falling stop vertical velocity
        this.rb.ClampYMaxSpeed(0f);
    }

    private void StopHorizontalSpeed()
    {
        this.rb.ClampXMaxSpeed(0f);
    }
}
