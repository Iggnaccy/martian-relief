using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour {

    GameObject minimapPanel;
	Room [,] rooms;
	bool [,] dfsArray;
	const int width = 11;
	const int height = 11;
	int actX;
	int actY;
    int deltaMap = 26;
    List<Vector2> edges;

	public float minX, maxX, minY, maxY;

	void Start () {
		rooms = new Room[width,height];
		dfsArray = new bool[width, height];
        edges = new List<Vector2>();
		actX = width  / 2;
		actY = height / 2;
        minimapPanel = GameObject.Find("MinimapPanel");
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				constructRoom (i, j);
			}
		}
        dfsArray[width / 2, height / 2] = true;
        Generation();
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				if(dfsArray[i, j])
				rooms[i,j].Generate(dfsArray);
			}
		}

		mergeDoors ();
        loadRoom(0, 0);
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
		loadRoom(deltaX, deltaY);
		return true;
	}

	void loadRoom(int deltaX, int deltaY){
		GetComponent<RoomManager> ().loadNewRoom (rooms[actX, actY]);
        foreach (RectTransform child in minimapPanel.transform)
        {
            child.transform.localPosition = new Vector3(child.transform.localPosition.x - deltaMap * deltaX, child.transform.localPosition.y - deltaMap * deltaY, 0);
            child.GetComponent<Image>().color = Color.red;
        }
        if (rooms[actX, actY].wasVisited == false)
        {
            Image temp;
            temp = Instantiate(rooms[actX, actY].minimapImage) as Image;
            rooms[actX, actY].minimapImage = temp;
            temp.transform.SetParent(minimapPanel.transform);
            temp.rectTransform.localPosition = new Vector3(-75, -75, 0);
        }
        rooms[actX, actY].wasVisited = true;
        rooms[actX, actY].minimapImage.color = Color.yellow;

	}
	void constructRoom(int x, int y){
		//rooms [x, y] = new Room (actX, actY, new Vector4(minX, maxX, minY, maxY), rooms, width, height);     kappa
		rooms [x, y] = new Room (x, y, new Vector4(minX, maxX, minY, maxY), rooms, width, height);
	}

	/*void OnGUI() {
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
	}*/

	void randomShuffle(List<int> _list){
		for (int i = 0; i < _list.Count; i++) {
			int temp = _list[i];
			int randomIndex = Random.Range(i, _list.Count);
			_list[i] = _list[randomIndex];
			_list[randomIndex] = temp;
		}
	}

	/*void dfsGenerate(int x, int y){
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
	}*/

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
    void Generation()
    {
        int posX = width/2, posY = height/2;
        for (int i = 0; i < 18; i++)
        {
            edges.Add(new Vector2((int)posX, (int)(posY + 1)));
            edges.Add(new Vector2((int)(posX + 1), (int)posY));
            edges.Add(new Vector2((int)posX, (int)(posY - 1)));
            edges.Add(new Vector2((int)(posX - 1), (int)posY));
            while (true)
            {
                int z = Random.Range(1, edges.Count - 2);
                if (!dfsArray[(int)edges[z].x, (int)edges[z].y])
                {
                    dfsArray[(int)edges[z].x, (int)edges[z].y] = true;
                    posX = (int)edges[z].x;
                    posY = (int)edges[z].y;
                    edges.RemoveAt(z);
                    break;
                }
                else edges.RemoveAt(z);
            }
        }
    }
}
