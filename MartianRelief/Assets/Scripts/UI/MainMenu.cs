using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenu : MonoBehaviour {

	public void newGame(){
		Application.LoadLevel ("GameScene");
	}

	public void options(){
		Application.LoadLevel ("OptionsScene");
	}
		
	public void exit(){
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
