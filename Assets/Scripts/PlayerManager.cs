using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private SceneManager sceneManager;
    private float startTime;
    private List<TimePosition> timePositionList;

    public float StartDelay { get; set; }
    public Vector2 Velocity { get; private set; }

    // Use this for initialization
    void Start ()
	{
        this.sceneManager = GameObject.FindGameObjectWithTag("Level").GetComponent<SceneManager>();
        this.startTime = Time.timeSinceLevelLoad;
        this.timePositionList = new List<TimePosition>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (InputManager.GetButton4Down())
        {
            Debug.Log("Button 4 is down");
            this.sceneManager.AddPlayerRecord(new PlayerRecord(this.StartDelay, this.timePositionList));
            this.sceneManager.ResetTime();
            Destroy(this.gameObject);
        }
	}

    private void FixedUpdate()
    {
        this.AddTimePosition();
    }

    private void AddTimePosition()
    {
        var timePosition = new TimePosition(Time.timeSinceLevelLoad - this.startTime, this.transform.position.AsVector2());

        var count = this.timePositionList.Count;

        if (count > 2
            && timePosition.Position == this.timePositionList[count - 1].Position
            && timePosition.Position == this.timePositionList[count - 2].Position)
        {
            this.timePositionList.Remove(this.timePositionList[count - 1]);
            Debug.Log("No movement since last update, removing previous last position.");
        }

        this.timePositionList.Add(timePosition);
        Debug.Log(string.Format("Time position list contains {0} elements", this.timePositionList.Count));
    }
}
