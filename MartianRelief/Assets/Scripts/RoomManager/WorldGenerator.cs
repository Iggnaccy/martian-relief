using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class WorldGenerator : MonoBehaviour
{

    public GameObject minimapPanel;
	public Room [,] rooms;
	bool [,] dfsArray;
	public int width = 11;
	public int height = 11;
    public int roomCount = 16;
	public int actX;
	public int actY;
    List<Vector2> edges;
    bool didIGenerateYet = false;

	public float minX, maxX, minY, maxY;

	void Awake ()
    {
		Random.seed = Static.randomSeed;
		rooms = new Room[width,height];
		dfsArray = new bool[width, height];
        dfsArray[width / 2, height / 2] = true;
        edges = new List<Vector2>();
        minimapPanel = GameObject.Find("MinimapPanel");
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				constructRoom (i, j);
			}
		}
		Debug.Log ("WorldGenerator::Awake() seed=" + Random.seed);
		Debug.Log("Static::randomSeed " + Static.randomSeed);
	}
	
    void Start()
    {
		Random.seed = Static.randomSeed;
        if (!didIGenerateYet)    //new game only
        {
            actX = width / 2;
            actY = height / 2;
            Generation();
        }
		Debug.Log ("WorldGenerator::Start() seed=" + Random.seed);
		//load game & new game
        mergeDoors();
        loadRoom(0, 0);

    }

	void Update ()
    {
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
        MinimapManagement(deltaX, deltaY);

	}

    void MinimapManagement(int deltaX, int deltaY)
    {
        foreach (RectTransform child in minimapPanel.transform)
        {
            if (child.tag == "MinimapRoom")
            {
                child.localPosition -= new Vector3((child.sizeDelta.x + 1.5f) * deltaX, (child.sizeDelta.y + 1.5f) * deltaY, 0);
                //child.transform.localPosition = new Vector3(child.transform.localPosition.x - deltaMap * deltaX, child.transform.localPosition.y - deltaMap * deltaY, 0);
                child.GetComponent<Image>().color = Color.cyan;
            }
        }
        if (rooms[actX, actY].wasVisited == false)
        {
            Image temp;
            temp = Instantiate(rooms[actX, actY].minimapImage) as Image;
            rooms[actX, actY].minimapImage = temp;
            temp.rectTransform.SetParent(minimapPanel.transform);
            temp.rectTransform.localPosition = new Vector3(-75, -75, 0);
            temp.rectTransform.localScale = Vector3.one;
            rooms[actX, actY].wasVisited = true;
        }
        rooms[actX, actY].minimapImage.color = Color.green;
    }

	void constructRoom(int x, int y){
		rooms [x, y] = new Room (x, y, new Vector4(minX, maxX, minY, maxY), width, height);
	}

	void randomShuffle(List<int> _list){
		for (int i = 0; i < _list.Count; i++) {
			int temp = _list[i];
			int randomIndex = Random.Range(i, _list.Count);
			_list[i] = _list[randomIndex];
			_list[randomIndex] = temp;
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
    public void Generation()
    {
		Debug.Log ("WorldGenerator::Generation() seed=" + Random.seed);
        int posX = width/2, posY = height/2;
        dfsArray = new bool[width, height];
        edges.Clear();
        for (int i = 0; i < roomCount; i++)
        {
            edges.Add(new Vector2((int)posX, (int)(posY + 1)));
            edges.Add(new Vector2((int)(posX + 1), (int)posY));
            edges.Add(new Vector2((int)posX, (int)(posY - 1)));
            edges.Add(new Vector2((int)(posX - 1), (int)posY));
            while (true)
            {
                int z = Random.Range(0, edges.Count - 1);
				Debug.Log ("WorldGenerator::Generation() after Random.Range seed=" + Random.seed);
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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (dfsArray[i, j])
                    rooms[i, j].Generate(dfsArray);
            }
        }
        didIGenerateYet = true;
    }
}
