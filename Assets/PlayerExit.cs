using UnityEngine;

public class PlayerExit : MonoBehaviour
{
    private bool playerCollided;
    private Rigidbody2D playerRb;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (!this.playerCollided)
        {
            return;
        }

        // Player has collided
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            this.playerCollided = true;
        }
    }
}
