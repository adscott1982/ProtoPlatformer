using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private List<PlayerRecord> pendingRecords;
    private List<PlayerRecord> activeRecords;
    private List<GameObject> playerReplayObjects;
    private PlayerStart playerStart;
    private float restartTime;

    // Use this for initialization
    void Start ()
    {
        this.restartTime = Time.timeSinceLevelLoad;
        this.pendingRecords = new List<PlayerRecord>();
        this.activeRecords = new List<PlayerRecord>();

        this.playerReplayObjects = new List<GameObject>();
        this.playerStart = this.GetComponentInChildren<PlayerStart>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.CheckPendingRecords();
	}

   

    internal void AddPlayerRecord(PlayerRecord record)
    {
        this.activeRecords.Add(record);
    }

    internal void ResetTime()
    {
        foreach(var playerReplayObject in this.playerReplayObjects)
        {
            Destroy(playerReplayObject);
        }

        this.restartTime = Time.timeSinceLevelLoad;

        foreach(var record in this.activeRecords)
        {
            this.pendingRecords.Add(record);
        }

        this.activeRecords.Clear();
    }

    private void CheckPendingRecords()
    {
        var timeSinceRestart = Time.timeSinceLevelLoad - this.restartTime;
        var readyToActivateRecords = this.pendingRecords.Where(r => r.StartDelay < timeSinceRestart);

        foreach (var record in readyToActivateRecords)
        {
            this.pendingRecords.Remove(record);
            this.activeRecords.Add(record);
            this.ActivateRecord(record);

            if (this.pendingRecords.Count == 0)
            {
                // No more replays to add - start new player
                this.playerStart.StartTime = this.restartTime;
                this.playerStart.PlayerStarted = false;
            }
        } 
    }

    private void ActivateRecord(PlayerRecord record)
    {
        var go = Instantiate(Resources.Load("PlayerReplay")) as GameObject;
        go.GetComponent<PlayerReplay>().TimePositions = record.TimePositions;
        this.playerReplayObjects.Add(go);
    }
}
