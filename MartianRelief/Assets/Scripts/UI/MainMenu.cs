using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenu : MonoBehaviour {

	public void newGame(){
        NewGameOrLoad.LoadName = null;
		SceneManager.LoadScene("GameScene");
	}

	public void options(){
        SceneManager.LoadScene("OptionsScene");
	}
		
	public void exit(){
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
