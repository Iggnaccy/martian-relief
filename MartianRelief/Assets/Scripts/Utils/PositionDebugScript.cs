using UnityEngine;
using System.Collections.Generic;
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
		List<int> realItemPool = new List<int> ();
		realItemPool = Static.listDifference (worldGenerator.rooms [worldGenerator.actX, worldGenerator.actY].itemPool, Static.itemsSpawned);
		foreach (int it in realItemPool) {
			text += it.ToString ();
			text += " ";
		} 
		text += "\n";
		text += "roomType: " + worldGenerator.rooms [worldGenerator.actX, worldGenerator.actY].roomType.ToString ();

		text += "\n\n";
		text += getDebugMiniMapString ();
		//Debug.Log(text);
        GetComponent<Text>().text = text;
	}

	string getDebugMiniMapString(){
		string toRender="";
		for (int j = worldGenerator.width-1; j >= 0; j--) {
			for(int i = 0; i < worldGenerator.height;i++){
				if(i==worldGenerator.actX && j==worldGenerator.actY)toRender+="<color=blue>x</color>";
				else if(worldGenerator.dfsArray[i,j])
					toRender += ("<color=green>"+worldGenerator.rooms[i,j].roomType +"</color>");
				else
					toRender += "<color=red>#</color>";
			}
			toRender += "\n";
			//GUI.Label (new Rect (25, 75+25+15*(width-j), 200, 50),toRender);
		}
		toRender += "pos = (" + worldGenerator.actX.ToString() + "," + worldGenerator.actY.ToString () + ")\n";
		return toRender;
	}
}
