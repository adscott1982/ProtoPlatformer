using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private List<List<TimePosition>> records;
    private List<GameObject> playerReplayObjects;
    // Use this for initialization
    void Start ()
    {
        this.records = new List<List<TimePosition>>();
        this.playerReplayObjects = new List<GameObject>();
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

        foreach(var record in this.records)
        {
            var go = Instantiate(Resources.Load("PlayerReplay")) as GameObject;
            go.GetComponent<PlayerReplay>().TimePositions = record;
            this.playerReplayObjects.Add(go);
        }

        var playerObject = Instantiate(Resources.Load("Player")) as GameObject;
    }
}
