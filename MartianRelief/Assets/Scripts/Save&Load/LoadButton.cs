using UnityEngine;
using System.Collections;

public class LoadButton : MonoBehaviour {
	//main menu + gra
	public void LoadLoadScene(string name)
    {
        NewGameOrLoad.LoadName = name;
        Application.LoadLevel("GameScene");
    }
}
