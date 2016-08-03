using UnityEngine;
using System.Collections;

public class NextFloor : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
			other.transform.position = new Vector3(0, 0, 0);
            WorldGenerator.floor++;
            Static.randomSeed++;
            //UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
			Application.LoadLevel(Application.loadedLevel);
		}
    }
}
