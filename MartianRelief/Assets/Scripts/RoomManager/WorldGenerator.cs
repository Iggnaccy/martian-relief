using UnityEngine;
using System.Collections;

public class WorldGenerator : MonoBehaviour {

	Room [,] rooms;
	int width = 10;
	int height = 10;
	int actX;
	int actY;

	public float minX, maxX, minY, maxY;

	void Start () {
		rooms = new Room[width,height];
		actX = width / 2;
		actY = height / 2;

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				constructRoom (i, j);
			}
		}

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				rooms[i,j].Generate();
			}
		}

		loadRoom ();

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
		rooms [x, y] = new Room (actX, actY, new Vector4(minX, maxX, minY, maxY), rooms, width, height, true);
	}

	void OnGUI() {
		GUI.contentColor = Color.red;
		GUI.Label (new Rect (25, 25, 200, 50), "actPos=(x="+actX.ToString()+", y="+actY.ToString ()+")");
	}
}
