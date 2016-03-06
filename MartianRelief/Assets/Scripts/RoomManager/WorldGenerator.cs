using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {

	Room [,] rooms;
	bool [,] dfsArray;
	const int width = 11;
	const int height = 11;
	int actX;
	int actY;

	public float minX, maxX, minY, maxY;
	
	void Start () {
		rooms = new Room[width,height];
		dfsArray = new bool[width, height];
		actX = width  / 2;
		actY = height / 2;

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				constructRoom (i, j);
			}
		}
		dfsGenerate (actX, actY);                           
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				if(dfsArray[i, j])
				rooms[i,j].Generate(dfsArray);
			}
		}

		mergeDoors ();
		loadRoom ();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)){
			Debug.Log("Debug neighbors:");
			if(dfsArray[actX-1,actY] == true) Debug.Log("x-1, y   = true");
			if(dfsArray[actX,actY-1] == true) Debug.Log("x,   y-1 = true");
			if(dfsArray[actX+1,actY] == true) Debug.Log("x+1, y   = true");
			if(dfsArray[actX,actY+1] == true) Debug.Log("x,   y+1 = true");
		}
	}

	public bool goToRoom(int deltaX, int deltaY, int side){
		int newX = actX + deltaX;
		int newY = actY + deltaY;

		if (newX < 0 || newY < 0 || newX >= width || newY >= height) {
			return false;
		}
		if (side == 0 || side == 2) {
			Vector3 actPos = GameObject.Find ("Player").transform.position;
			GameObject.Find ("Player").transform.position = new Vector3(actPos.x*-1, actPos.y, actPos.z);
		}
		if (side == 1 || side == 3) {
			Vector3 actPos = GameObject.Find ("Player").transform.position;
			GameObject.Find ("Player").transform.position = new Vector3(actPos.x, actPos.y*-1, actPos.z);
		}
		actX = newX;
		actY = newY;
		loadRoom();
		return true;
	}

	void loadRoom(){
		GetComponent<RoomManager> ().loadNewRoom (rooms[actX, actY]);
	}
	void constructRoom(int x, int y){
		//rooms [x, y] = new Room (actX, actY, new Vector4(minX, maxX, minY, maxY), rooms, width, height);     kappa
		rooms [x, y] = new Room (x, y, new Vector4(minX, maxX, minY, maxY), rooms, width, height);
	}

	void OnGUI() {
		GUI.contentColor = Color.red;
		GUI.Label (new Rect (25, 25, 200, 50), "actPos=(x="+actX.ToString()+", y="+actY.ToString ()+")");
		if (rooms [actX,actY].isGenerated) {
			GUI.Label (new Rect (25, 75, 200, 50), "is generated");
		} else {
			GUI.Label (new Rect (25, 75, 200, 50), "not generated");
		}
		for (int j = 0; j < width; j++) {
			string toRender="";
			for(int i = 0; i < height;i++){
				if(i==actX && j==actY)toRender+="x";
				else if(dfsArray[i,j])
					toRender += "_";
				else
					toRender += "#";
			}
			GUI.Label (new Rect (25, 75+25+15*(width-j), 200, 50),toRender);
		}
	}

	void randomShuffle(List<int> _list){
		for (int i = 0; i < _list.Count; i++) {
			int temp = _list[i];
			int randomIndex = Random.Range(i, _list.Count);
			_list[i] = _list[randomIndex];
			_list[randomIndex] = temp;
		}
	}

	void dfsGenerate(int x, int y){
		dfsArray [x, y] = true;

		List<int> vDirections = new List<int> ();
		vDirections.Add (0);
		vDirections.Add (1);
		vDirections.Add (2);
		vDirections.Add (3);

		randomShuffle (vDirections);
		for (int i = 0; i < vDirections.Count; i++) {
			if (vDirections[i] == 0 && x - 2 >= 0 && dfsArray [x - 2, y] == false) {
				dfsArray [x - 1, y] = true;
				dfsGenerate (x - 2, y);
			}
			if (vDirections[i] == 1 && y - 2 >= 0 && dfsArray [x, y - 2] == false) {
				dfsArray [x, y - 1] = true;
				dfsGenerate (x, y - 2);
			}
			if (vDirections[i] == 2 && x + 2 < width && dfsArray [x + 2, y] == false) {
				dfsArray [x + 1, y] = true;
				dfsGenerate (x + 2, y);
			}
			if (vDirections[i] == 3 && y + 2 < height && dfsArray [x, y + 2] == false) {
				dfsArray [x, y + 1] = true;
				dfsGenerate (x, y + 2);
			}
		}
	}

	void mergeDoors(){
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				if(j > 0){
					if(rooms[i,j-1].isGenerated){
						rooms[i,j-1].doors[1] = rooms[i,j].doors[3];
						rooms[i,j-1].vecDoors[1] = new Vector3(rooms[i,j].doors[3], rooms[i,j-1].vecDoors[1].y, 0);
					}
					else{
						rooms[i,j].doors[3]=-90000;
						rooms[i,j-1].doors[1]=-90000;
					}
				}
				if(i < width-1){
					if(rooms[i+1,j].isGenerated){
						rooms[i+1,j].doors[0] = rooms[i,j].doors[2];
						rooms[i+1,j].vecDoors[0] = new Vector3(rooms[i+1,j].vecDoors[0].x, rooms[i,j].doors[2], 0);
					}
					else{
						rooms[i,j].doors[2]=-90000;
						rooms[i+1,j].doors[0]=-90000;
					}
				}
			}
		}
	}
}
