using System;
using System.Collections;
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

    private bool isGrounded;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    private bool jumpWasPressed;

    private List<Vector2> contactNormals;
    private Collider2D collider;
    private Rigidbody2D rb;

    // Use this for initialization
	private void Start()
	{
        this.contactNormals = new List<Vector2>();
	    this.collider = this.GetComponent<Collider2D>();
	    this.rb = this.GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        this.UpdateButtonPresses();
    }

    private void UpdateButtonPresses()
    {
        if (InputManager.GetButtonADown())
        {
            this.jumpWasPressed = true;
        }
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
    }


    private void CheckCollisions()
    {
        this.isGrounded = this.contactNormals.Any(n => n == Vector2.up);
        this.isTouchingLeftWall = this.contactNormals.Any(n => n == Vector2.right);
        this.isTouchingRightWall = this.contactNormals.Any(n => n == Vector2.left);

        Debug.Log("Contact normals: " + this.contactNormals.Count);
        Debug.Log("IsGrounded: " + this.isGrounded);
        Debug.Log("IsTouchingLeftWall: " + this.isTouchingLeftWall);
        Debug.Log("IsTouchingRightWall: " + this.isTouchingRightWall);

        this.contactNormals.Clear();
    }

    private void UpdateMovement()
    {
        // Clamp max speed
        if (Math.Abs(this.rb.velocity.x) > this.MaxSpeed)
        {
            var velocity = this.rb.velocity;
            velocity.x = velocity.x > 0 ? this.MaxSpeed : -this.MaxSpeed;
            this.rb.velocity = velocity;
        }

        var leftThumbstick = InputManager.GetLeftThumbstick();

        //Debug.Log("Left Thumbstick X - " + leftThumbstick.x);

        // If the player has released the lateral movement, decelerate
        if (Math.Abs(leftThumbstick.x) < Tools.FloatEqualityTolerance && Math.Abs(this.rb.velocity.x) > Tools.FloatEqualityTolerance)
        {
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
        else // Else apply lateral movement
        {
            var lateralForce = this.isGrounded ? this.GroundAccelerationForce : this.AirAccelerationForce;
            var lateralForceVector = new Vector2(leftThumbstick.x * lateralForce, 0);
            this.rb.AddForce(lateralForceVector);
        }

        if (this.jumpWasPressed)
        {
            if (this.isGrounded) // Jump normally if on the ground
            {
                this.rb.AddForce(Vector2.up * this.JumpForce);
            }
            else if (this.isTouchingLeftWall || this.isTouchingRightWall) // Else if on a wall, wall jump
            {
                // If falling stop vertical velocity
                if (this.rb.velocity.y < 0)
                {
                    var velocity = this.rb.velocity;
                    velocity.y = 0;
                    this.rb.velocity = velocity;
                }

                var jumpDirection = this.isTouchingLeftWall ? new Vector2(1f, 1.2f) : new Vector2(-1f, 1.2f);
                jumpDirection = jumpDirection.normalized;
                this.rb.AddForce(jumpDirection * this.JumpForce);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        var firstNormal = other.contacts.First().normal;
        this.contactNormals.Add(firstNormal);
    }
}
