using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour {
	public void LoadLoadScene()
    {
        SceneManager.LoadScene("LoadScene");
    }
}
