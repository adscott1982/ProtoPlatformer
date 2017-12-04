using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollBone : MonoBehaviour
{
    private PlayerManager playerManager;
    private Rigidbody2D parentRb;
    private Collider2D coll;
    private Rigidbody2D rb;
    private Joint2D joint;

    private bool hasTransitionedToRagdoll = false;

    // Use this for initialization
    void Start()
	{
	    this.playerManager = this.GetComponentInParent<PlayerManager>();
	    this.coll = this.GetComponent<Collider2D>();
	    this.rb = this.GetComponent<Rigidbody2D>();
	    this.joint = this.GetComponent<Joint2D>();

        //this.coll.enabled = false;
        //if (this.joint != null) this.joint.enabled = false;
	    this.rb.simulated = false;
	}
	
	// Update is called once per frame
	void Update()
    {
        if (this.playerManager.IsRagdollActivated &&!this.hasTransitionedToRagdoll)
        {
            this.hasTransitionedToRagdoll = true;
            this.rb.simulated = true;
            this.rb.velocity = this.playerManager.Velocity;
            //this.coll.enabled = true;
            //if (this.joint != null) this.joint.enabled = false;
        }
	}

    void FixedUpdate()
    {
        //if (this.isActivatedThisFrame)
        //{
            
        //    this.isActivatedThisFrame = false;
        //}
    }
}
