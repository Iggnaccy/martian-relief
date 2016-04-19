using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour {
	//scena load
    public void Load(string name)
    {
        NewGameOrLoad.LoadName = name;
        Application.LoadLevel("GameScene");
    }
}
