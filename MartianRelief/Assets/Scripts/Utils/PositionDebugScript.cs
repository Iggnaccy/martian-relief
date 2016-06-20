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
        //Debug.Log("zaczynam się ustawiać");
        text = "x: ";
        text += worldGenerator.actX.ToString();
        text += "   y: ";
        text += worldGenerator.actY.ToString();
		text += "\n";
		text += "rt: " + worldGenerator.rooms[worldGenerator.actX,worldGenerator.actY].roomType.ToString();
		text += "\n";
		text += "item pool:\n";
		foreach (int it in worldGenerator.rooms[worldGenerator.actX,worldGenerator.actY].itemPool) {
			text += it.ToString ();
			text += " ";
		} 
		text += "\n";
		text += "roomType: " + worldGenerator.rooms [worldGenerator.actX, worldGenerator.actY].roomType.ToString ();
		//Debug.Log(text);
        GetComponent<Text>().text = text;
	}
}
