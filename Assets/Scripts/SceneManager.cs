using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private List<List<TimePosition>> records;
    private List<GameObject> playerReplayObjects;
    private PlayerStart playerStart;
    private float restartTime;

    // Use this for initialization
    void Start ()
    {
        this.records = new List<List<TimePosition>>();
        this.playerReplayObjects = new List<GameObject>();
        this.playerStart = this.GetComponentInChildren<PlayerStart>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    internal void AddReplay(List<TimePosition> timePositionList)
    {
        this.records.Add(timePositionList);
    }

    internal void ResetTime()
    {
        foreach(var playerReplayObject in this.playerReplayObjects)
        {
            Destroy(playerReplayObject);
        }

        this.restartTime = Time.timeSinceLevelLoad;
        foreach(var record in this.records)
        {
            var go = Instantiate(Resources.Load("PlayerReplay")) as GameObject;
            go.GetComponent<PlayerReplay>().TimePositions = record;
            this.playerReplayObjects.Add(go);
        }

        this.playerStart.PlayerStarted = false;
    }
}
