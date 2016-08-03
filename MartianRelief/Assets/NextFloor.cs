using UnityEngine;
using System.Collections;

public class NextFloor : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            WorldGenerator.floor++;
            Random.seed++;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}
