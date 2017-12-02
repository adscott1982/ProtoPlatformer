using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Extensions;
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

    private bool isGrounded;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    private bool jumpWasPressed;
    
    private bool isDecelerating;
    private bool isWallSliding;

    private Vector2 inputAxes;

    private List<Vector2> contactNormals;
    private Rigidbody2D rb;
    private Animator animator;
    

    public Vector2 Velocity { get { return this.rb.velocity; } }
    public Vector2 InputAxes { get { return this.inputAxes; } }

    public bool IsBackContactPointTriggered { get; set; }

    public bool IsFrontContactPointTriggered { get; set; }

    public bool IsBottomContactPointTriggered { get; set; }

    public bool IsTopContactPointTriggered { get; set; }

    public bool IsFacingLeft { get; private set; }

    // Use this for initialization
    private void Start()
	{
        this.animator = this.GetComponent<Animator>();
        this.contactNormals = new List<Vector2>();
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
        var firstNormal = other.contacts.First().normal;
        this.contactNormals.Add(firstNormal);
    }

    private void UpdateInput()
    {
        if (InputManager.GetButtonADown())
        {
            this.jumpWasPressed = true;
        }

        this.inputAxes = InputManager.GetLeftThumbstick();
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

        this.animator.SetFloat("xSpeed", Math.Abs(this.Velocity.x));
        this.animator.SetFloat("vSpeed", this.Velocity.y);
        this.animator.SetBool("OnGround", this.isGrounded);
        this.animator.SetBool("FacingLeft", this.IsFacingLeft);
        this.animator.SetBool("IsDecelerating", this.isDecelerating);
        this.animator.SetBool("IsWallSliding", this.isWallSliding);
    }

    private void CheckCollisions()
    {
        this.isGrounded = this.contactNormals.Any(n => n == Vector2.up);
        this.isTouchingLeftWall = this.contactNormals.Any(n => n == Vector2.right);
        this.isTouchingRightWall = this.contactNormals.Any(n => n == Vector2.left);
        this.contactNormals.Clear();
    }

    private void UpdateMovement()
    {
        this.rb.ClampXMaxSpeed(this.MaxSpeed);

        this.isWallSliding = false;

        // If on a wall and not on the ground, and the left thumbstick is pressed in a lateral direction, clamp vertical falling speed to wall slide speed
        if (!this.isGrounded && (this.isTouchingLeftWall || this.isTouchingRightWall) && !this.inputAxes.x.IsApproxZero())
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
