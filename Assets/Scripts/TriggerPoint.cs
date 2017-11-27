using System;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    public TriggerPositions ContactPosition;
    public List<string> CollisionTags;

    private PlayerMovement parent;
    private Action<bool> UpdateParent;
    private bool isColliding;

    void Start()
    {
        this.parent = this.GetComponentInParent<PlayerMovement>();

        switch (this.ContactPosition)
        {
            case TriggerPositions.Bottom:
                this.UpdateParent = b => this.parent.IsBottomContactPointTriggered = b;
                break;
            case TriggerPositions.Left:
                this.UpdateParent = b => this.parent.IsLeftContactPointTriggered = b;
                break;
            case TriggerPositions.Right:
                this.UpdateParent = b => this.parent.IsRightContactPointTriggered = b;
                break;
            case TriggerPositions.Top:
                this.UpdateParent = b => this.parent.IsTopContactPointTriggered = b;
                break;
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
	{
	    this.UpdateParent(this.isColliding);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.CollisionTags.Contains(other.tag))
        {
            this.isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (this.CollisionTags.Contains(other.tag))
        {
            this.isColliding = false;
        }
    }
}
