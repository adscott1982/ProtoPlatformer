using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float LateralMoveSpeed = 2f;
    public float JumpForce = 5f;

    private bool isGrounded;

    private List<Vector2> contactNormals;

    private bool isTouchingWall;

    private Collider2D collider;
    private Rigidbody2D rb;
    private bool jumpWasPressed;

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
        this.isTouchingWall = this.contactNormals.Any(n => n == Vector2.left || n == Vector2.right);

        Debug.Log("Contact normals: " + this.contactNormals.Count);
        Debug.Log("IsGrounded: " + this.isGrounded);
        Debug.Log("IsTouchingWall: " + this.isTouchingWall);

        this.contactNormals.Clear();
    }

    private void UpdateMovement()
    {
        var leftThumbstick = InputManager.GetLeftThumbstick();

        //Debug.Log("Left Thumbstick X - " + leftThumbstick.x);

        if (Math.Abs(leftThumbstick.x) < 0.2f)
        {
            leftThumbstick.x = 0f;
        }

        var moveSpeed = this.isGrounded ? this.LateralMoveSpeed : this.LateralMoveSpeed;
        var velocity = new Vector2(leftThumbstick.x * moveSpeed, this.rb.velocity.y);

        if (this.jumpWasPressed)
        {
            if (this.isGrounded)
            {
                this.rb.AddForce(Vector2.up * this.JumpForce);
            }
            else if (this.isTouchingWall)
            {
                // Flip the X direction of velocity
                velocity.x = -velocity.x;
                this.rb.AddForce(Vector2.up * this.JumpForce / 2);
            }
        }

        this.rb.velocity = velocity;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        var firstNormal = other.contacts.First().normal;
        this.contactNormals.Add(firstNormal);
    }
}
