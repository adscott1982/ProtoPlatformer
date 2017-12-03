using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Animator animator;

    public bool IsRagdollActivated { get; set; }

	// Use this for initialization
	void Start ()
	{
	    this.animator = this.GetComponent<Animator>();
        this.IsRagdollActivated = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (InputManager.GetButton4Down())
        {
            //this.animator.SetBool("IsRagdollActivated", true);
            this.animator.enabled = false;
            this.IsRagdollActivated = true;

            this.GetComponent<Rigidbody2D>().simulated = false;
        }
	}
}
