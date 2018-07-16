using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerReplay : MonoBehaviour
{
    private float startTime;
    private int lastIndex;
    private Rigidbody2D rb;
    private Vector2 previousTargetPosition;
    private bool replayEnded;

    public List<TimePosition> TimePositions { get; set; }

    
    // Use this for initialization
    void Start ()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.startTime = Time.timeSinceLevelLoad;
        this.transform.position = this.TimePositions[0].Position;
        this.previousTargetPosition = this.TimePositions[0].Position;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        this.rb.position = previousTargetPosition;
        if (this.replayEnded)
        {
            this.rb.isKinematic = true;
        }

        var targetPosition = this.DetermineCurrentPosition();
        this.rb.MovePosition(targetPosition);
        this.previousTargetPosition = targetPosition;
	}

    private Vector2 CalculateVelocity(Vector2 position, Vector2 targetPosition)
    {
        return (targetPosition - position) * Time.fixedDeltaTime; 
    }

    private Vector2 DetermineCurrentPosition()
    {
        Vector2 position = this.transform.position;

        if (this.lastIndex >= this.TimePositions.Count - 1)
        {
            this.replayEnded = true;
            return position;
        }

        var currentAge = Time.timeSinceLevelLoad - this.startTime;

        // Find the previous and next position based on current time
        for (var i = lastIndex; i < this.TimePositions.Count; i++)
        {
            if (this.TimePositions[i].Seconds < currentAge)
            {
                lastIndex = i;
                continue;
            }

            var lastTimePosition = this.TimePositions[lastIndex];
            var nextTimePosition = this.TimePositions[i];

            if (lastIndex == 0 && i == 0)
            {
                lastTimePosition = new TimePosition(this.startTime, this.transform.position);
            }

            var currentTicks = currentAge - lastTimePosition.Seconds;
            var nextTicks = nextTimePosition.Seconds - lastTimePosition.Seconds;

            var fraction = currentTicks / nextTicks;

            var xDelta = (nextTimePosition.Position.x - lastTimePosition.Position.x) * fraction;
            var xPos = lastTimePosition.Position.x + xDelta;

            var yDelta = (nextTimePosition.Position.y - lastTimePosition.Position.y) * fraction;
            var yPos = lastTimePosition.Position.y + yDelta;

            position = new Vector2(xPos, yPos);

            break;
        }

        // Find the percentage between the previous and next position
        // scale the x and y values between the previous and next position as new position

        return position;
    }
}
