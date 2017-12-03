using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollBone : MonoBehaviour
{
    private PlayerManager playerManager;
    private Collider2D coll;
    private Rigidbody2D rb;
    private Joint2D joint;

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
        if (this.playerManager.IsRagdollActivated)
        {
            this.rb.simulated = true;
            //this.coll.enabled = true;
            //if (this.joint != null) this.joint.enabled = false;
        }
	}
}
