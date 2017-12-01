using System;
using System.Collections.Generic;
using System.Linq;
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
    private Collider2D collider;
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
        this.animator = GetComponent<Animator>();
        this.contactNormals = new List<Vector2>();
	    this.collider = this.GetComponent<Collider2D>();
	    this.rb = this.GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        this.UpdateInput();
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

    // Update is called once per frame
	private void FixedUpdate()
	{
	    this.CheckCollisions();
	    this.UpdateMovement();
        this.ResetButtonPresses();
	    this.UpdateAnimator();
	}

    private void UpdateAnimator()
    {
        if (this.Velocity.x > AndyTools.FloatEqualityTolerance)
        {
            if (this.IsFacingLeft)
            {
                this.IsFacingLeft = false;
                var currentScale = this.transform.localScale;
                currentScale.x = -currentScale.x;
                this.transform.localScale = currentScale;
            }
        }
        else if (this.Velocity.x < -AndyTools.FloatEqualityTolerance)
        {
            if (!this.IsFacingLeft)
            {
                this.IsFacingLeft = true;
                var currentScale = this.transform.localScale;
                currentScale.x = -currentScale.x;
                this.transform.localScale = currentScale;
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

        //Debug.Log("Contact normals: " + this.contactNormals.Count);
        //Debug.Log("IsGrounded: " + this.isGrounded);
        //Debug.Log("IsTouchingLeftWall: " + this.isTouchingLeftWall);
        //Debug.Log("IsTouchingRightWall: " + this.isTouchingRightWall);

        this.contactNormals.Clear();
    }

    private void UpdateMovement()
    {
        this.ClampMaxLateralSpeed();

        this.isWallSliding = false;

        // If on a wall and not on the ground, and the left thumbstick is pressed in a direction, clamp vertical falling speed to wall slide speed
        if (!this.isGrounded && (this.isTouchingLeftWall || this.isTouchingRightWall) && Math.Abs(this.inputAxes.x) > AndyTools.FloatEqualityTolerance)
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
        if (Math.Abs(this.inputAxes.x) < AndyTools.FloatEqualityTolerance && Math.Abs(this.rb.velocity.x) > AndyTools.FloatEqualityTolerance)
        {
            this.DecelerateLateralSpeed();
        }
        else
        {
            this.ApplyLateralForce(this.inputAxes.x);
        }
    }

    private void ClampMaxLateralSpeed()
    {
        // If not currently at max speed, return
        if (!(Math.Abs(this.rb.velocity.x) > this.MaxSpeed))
        {
            return;
        }

        var velocity = this.rb.velocity;
        velocity.x = velocity.x > 0 ? this.MaxSpeed : -this.MaxSpeed;
        this.rb.velocity = velocity;
    }

    private void DecelerateLateralSpeed()
    {
        this.isDecelerating = true;
        var velocity = this.rb.velocity;
        var deceleration = this.isGrounded ? this.IdleDecelerationGrounded * Time.fixedDeltaTime :
            this.IdleDecelerationAir * Time.fixedDeltaTime;

        if (Mathf.Abs(velocity.x) - deceleration < 0)
        {
            velocity.x = 0f;
        }
        else
        {
            velocity.x = velocity.x > 0 ? velocity.x - deceleration : velocity.x + deceleration;
        }

        this.rb.velocity = velocity;
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
        var velocity = this.rb.velocity;
        velocity.y += this.WallSlideDeceleration;
        velocity.y = velocity.y > -this.WallSlideSpeed ? -this.WallSlideSpeed : velocity.y;
        this.rb.velocity = velocity;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        var firstNormal = other.contacts.First().normal;
        this.contactNormals.Add(firstNormal);
    }

    private void StopFalling()
    {
        if (!(this.rb.velocity.y < 0)) return;

        // If falling stop vertical velocity
        var velocity = this.rb.velocity;
        velocity.y = 0;
        this.rb.velocity = velocity;
    }
    private void StopHorizontalSpeed()
    {
        if (Math.Abs(this.rb.velocity.x) < AndyTools.FloatEqualityTolerance)
        {
            return;
        }

        // If falling stop vertical velocity
        var velocity = this.rb.velocity;
        velocity.x = 0;
        this.rb.velocity = velocity;
    }
}
