﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform RagdollCameraTarget;
    public List<Transform> RenderObjects;

    private Animator animator;
    private CameraFollow2D cameraFollower;
    private PlayerMovement playerMovement;

    public bool IsRagdollActivated { get; set; }
    public Vector2 Velocity { get; private set; }

	// Use this for initialization
	void Start ()
	{
	    this.animator = this.GetComponent<Animator>();
	    this.playerMovement = this.GetComponent<PlayerMovement>();
	    this.cameraFollower = Camera.main.GetComponent<CameraFollow2D>();
        this.IsRagdollActivated = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (InputManager.GetButton4Down())
        //{
        //    //this.animator.SetBool("IsRagdollActivated", true);
        //    this.animator.enabled = false;
        //    this.IsRagdollActivated = true;
        //    this.Velocity = this.GetComponent<Rigidbody2D>().velocity;
        //    this.GetComponent<Rigidbody2D>().simulated = false;

        //    this.cameraFollower.Target = this.RagdollCameraTarget;
        //}

        //if (!this.IsRagdollActivated)
        //{
        //    // Rotate the render objects based on the PlayerMovement specified RenderAngle
        //    foreach (var renderObject in this.RenderObjects)
        //    {
        //        renderObject.localRotation = Quaternion.AngleAxis(this.playerMovement.RenderRotation, Vector3.forward);
        //    }
        //}
	}
}
