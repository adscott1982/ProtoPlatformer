using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SceneManager sceneManager;
    private float startTime;
    private List<TimePosition> timePositionList;

    public Vector2 Velocity { get; private set; }

    // Use this for initialization
    void Start ()
	{
        this.sceneManager = GameObject.FindGameObjectWithTag("Level").GetComponent<SceneManager>();
        this.startTime = Time.timeSinceLevelLoad;
        this.timePositionList = new List<TimePosition>();
	    this.playerMovement = this.GetComponent<PlayerMovement>();

        this.AddTimePosition();
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.AddTimePosition();
        if (InputManager.GetButton4Down())
        {
            Debug.Log("Button 4 is down");
            this.sceneManager.AddReplay(this.timePositionList);
            this.sceneManager.ResetTime();
            Destroy(this.gameObject);
        }
	}

    private void AddTimePosition()
    {
        var timePosition = new TimePosition(Time.timeSinceLevelLoad - this.startTime, this.transform.position.AsVector2());
        this.timePositionList.Add(timePosition);
        Debug.Log(string.Format("Time position list contains {0} elements", this.timePositionList.Count));
    }
}
