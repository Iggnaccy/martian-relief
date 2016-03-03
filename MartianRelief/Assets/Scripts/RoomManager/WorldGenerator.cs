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
		/*dfsArray = new bool[width, height] { 
			{ true, true, true, true, true }, 
			{ true, true, true, true, true }, 
			{ true, true, true, true, true }, 
			{ true, true, true, true, true },
			{ true, true, true, true, true } 
		};*/
		actX = width  / 2;
		actY = height / 2;

		//dfsArray [actX, actY] = true;
		//dfsArray [actX - 1, actY] = true;

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				constructRoom (i, j);
			}
		}

		dfsGenerate (actX, actY);

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				if(dfsArray[i, j])
				rooms[i,j].Generate();
			}
		}

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				/*if(i > 0){
					rooms[i-1,j].doors[2] = rooms[i,j].doors[0];
					rooms[i-1,j].vecDoors[2] = new Vector3(rooms[i-1,j].vecDoors[2].x, rooms[i,j].doors[0], 0);
				}*/
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
				/*if(j < height-1){
					rooms[i,j+1].doors[3] = rooms[i,j].doors[1];
					rooms[i,j+1].vecDoors[3] = new Vector3(rooms[i,j].doors[1], rooms[i,j+1].vecDoors[1].y, 0);
				}*/
			}
		}

		loadRoom ();



		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				Debug.Log (i + " " + j + " " + rooms[i,j].isGenerated);
			}
		}
		//Debug.Log ("bazowy: " + rooms.GetHashCode());
	}
	
	void Update () {
	
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
		rooms [x, y] = new Room (actX, actY, new Vector4(minX, maxX, minY, maxY), rooms, width, height);
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
					toRender += " ";
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

		dfsArray[x,y]=true;
		if(x - 2 >= 0 && dfsArray[x-2,y]==false){
			dfsArray[x-1,y]=true;
			dfsGenerate(x-2,y);
		}
		if(y - 2 >= 0 && dfsArray[x,y-2]==false){
			dfsArray[x,y-1]=true;
			dfsGenerate(x,y-2);
		}
		if(x + 2 < width && dfsArray[x+2,y]==false){
			dfsArray[x+1,y]=true;
			dfsGenerate(x+2,y);
		}
		if(y + 2 < height && dfsArray[x,y+2]==false){
			dfsArray[x,y+1]=true;
			dfsGenerate(x,y+2);
		}

		/*List<int> sides = new List<int>(){1, 2, 3, 4};
		randomShuffle (sides);
		for(int i = 0; i < sides.Count; i++){
			if(sides[i] == 1){
				if(x>1)dfsArray[x-1,y]=true;
				if(x>2 && !dfsArray[x-2,y]){
					dfsGenerate(x-2,y);
				}
			}
			if(sides[i] == 2){
				if(y>1)dfsArray[x,y-1]=true;
				if(y>2 && !dfsArray[x,y-2]){
					dfsGenerate(x,y-2);
				}
			}
			if(sides[i] == 3){
				if(x+1<width)dfsArray[x+1,y]=true;
				if(x+2<width && !dfsArray[x+2,y]){
					dfsGenerate(x+2,y);
				}
			}
			if(sides[i] == 4){
				if(y+1<height)dfsArray[x,y+1]=true;
				if(y+2<height && !dfsArray[x,y+2]){
					dfsGenerate(x,y+2);
				}
			}
		}*/
	}
}
