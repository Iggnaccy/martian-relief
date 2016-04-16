using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PositionDebugScript : MonoBehaviour {

    WorldGenerator worldGenerator;
    string text;

	void Start () {
        worldGenerator = GameObject.Find("RoomManager").GetComponent<WorldGenerator>();
	}

	void Update () {
        Debug.Log("zaczynam się ustawiać");
        text = "x: ";
        text += worldGenerator.actX.ToString();
        text += "   y: ";
        text += worldGenerator.actY.ToString();
        Debug.Log(text);
        GetComponent<Text>().text = text;
	}
}
