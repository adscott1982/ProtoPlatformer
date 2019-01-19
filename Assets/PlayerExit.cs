using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerExit : MonoBehaviour
{
    private bool playerCollided;
    private AsyncOperation sceneLoader;

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
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (this.sceneLoader == null)
        {
            this.sceneLoader = SceneManager.LoadSceneAsync(currentSceneIndex + 1);
        }
        
        if (!this.sceneLoader.isDone)
        {
            return;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentSceneIndex + 1));

        if (currentSceneIndex > 0)
        {
            SceneManager.UnloadSceneAsync(currentSceneIndex - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && !collider.isTrigger)
        {
            this.playerCollided = true;
        }
    }
}
