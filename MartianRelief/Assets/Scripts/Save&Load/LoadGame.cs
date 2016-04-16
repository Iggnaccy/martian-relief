using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {
    public void Load(string name)
    {
        NewGameOrLoad.LoadName = name;
        SceneManager.LoadScene("GameScene");
    }
}
