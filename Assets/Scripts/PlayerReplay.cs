using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerReplay : MonoBehaviour
{
    private SceneManager sceneManager;
    private float startTime;
    private int lastIndex;

    public List<TimePosition> TimePositions { get; set; }

    
    // Use this for initialization
    void Start ()
    {
        this.sceneManager = GameObject.FindGameObjectWithTag("Level").GetComponent<SceneManager>();
        this.startTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (this.TimePositions == null)
        {
            return;
        }

        try
        {
            this.transform.position = this.DetermineCurrentPosition();
        }
        catch (Exception e)
        {

        }
	}

    private Vector2 DetermineCurrentPosition()
    {
        Vector2 position = this.transform.position;
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

            var currentTicks = currentAge - lastTimePosition.Seconds;
            var nextTicks = nextTimePosition.Seconds - lastTimePosition.Seconds;

            var fraction = currentTicks / nextTicks;

            var xDelta = (nextTimePosition.Position.x - lastTimePosition.Position.x) * fraction;
            var xPos = lastTimePosition.Position.x + xDelta;

            var yDelta = (nextTimePosition.Position.y - lastTimePosition.Position.y) * fraction;
            var yPos = lastTimePosition.Position.y + yDelta;

            position = new Vector2(xPos, yPos);
        }

        // Find the percentage between the previous and next position
        // scale the x and y values between the previous and next position as new position

        return position;
    }
}
