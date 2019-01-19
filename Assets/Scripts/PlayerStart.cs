using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public bool PlayerStarted
    {
        get { return this.playerStarted; }
        set
        {
            this.playerStarted = value;
            if(value == false) this.timeLastBlocked = Time.timeSinceLevelLoad;
        }
    }

    public float StartTime = float.NaN;

    private float timeLastBlocked;
    private GameObject playerObject;
    private bool isWaiting;
    private bool playerStarted;

    // Use this for initialization
    void Start ()
    {
        this.PlayerStarted = false;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (this.playerStarted || (Time.timeSinceLevelLoad - this.timeLastBlocked) < 0.5)
        {
            return;
        }

        this.playerObject = Instantiate(Resources.Load("Player"), this.transform.position, Quaternion.identity) as GameObject;
        this.playerObject.GetComponent<PlayerManager>().StartDelay = float.IsNaN(this.StartTime) ? 0f : Time.timeSinceLevelLoad - this.StartTime;
        this.PlayerStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Platform")
        {
            this.timeLastBlocked = Time.timeSinceLevelLoad;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Platform")
        {
            //this.isBlocked = true;
            this.timeLastBlocked = Time.timeSinceLevelLoad;
        }
    }
}
