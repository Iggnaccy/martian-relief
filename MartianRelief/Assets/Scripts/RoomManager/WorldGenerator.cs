using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class WorldGenerator : MonoBehaviour
{
	
	public GameObject minimapPanel;
    public PrefabHolder prefabHolder;
	public Room [,] rooms;
	public List<int>[,] merchantItems;
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
		Static.generateGlobalItemPool ();
		Random.seed = Static.randomSeed;
		rooms = new Room[width,height];
		merchantItems = new List<int>[width,height];
		dfsArray = new bool[width, height];
		dfsArray[width / 2, height / 2] = true;
		edges = new List<Vector2>();
		minimapPanel = GameObject.Find("MinimapPanel");
        prefabHolder = GameObject.Find("PrefabHolder").GetComponent<PrefabHolder>();
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				rooms [i, j] = new Room (i, j, new Vector4(minX, maxX, minY, maxY), width, height);
                rooms[i, j].minimapImage = null;
			}
		}
		//Random.seed = -1402559560;
		Debug.Log ("WorldGenerator::Awake() seed=" + Random.seed);
		//Static.randomSeed = 1466330462;
		Debug.Log("Static::randomSeed " + Static.randomSeed);
	}

	void Start()
	{
		Random.seed = Static.randomSeed;
		if (!didIGenerateYet)    //new game only
		{
			//Static.randomSeed = Static.getUnixTime ();
			actX = width / 2;
			actY = height / 2;
			Generation();
		}
		//Random.seed = -1986870875;
		Debug.Log ("WorldGenerator::Start() seed=" + Random.seed);
		//load game & new game
		mergeDoors();
		loadRoom(0, 0);
		
	}
	
	/*void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Q)){
			Debug.Log("Debug neighbors:");
			if(dfsArray[actX-1,actY] == true) Debug.Log("x-1, y   = true");
			if(dfsArray[actX,actY-1] == true) Debug.Log("x,   y-1 = true");
			if(dfsArray[actX+1,actY] == true) Debug.Log("x+1, y   = true");
			if(dfsArray[actX,actY+1] == true) Debug.Log("x,   y+1 = true");
		}
	}*/
	
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
		if (rooms [actX, actY].vecEnemies.Count == 0) {
			rooms[actX,actY].wasCleared=true;
		}
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

			}
		}
		if(rooms[actX - deltaX, actY - deltaY].minimapImage != null){
			rooms[actX - deltaX, actY - deltaY].minimapImage.color = Color.cyan;
		}
		if (rooms[actX, actY].wasVisited == false)
		{
            if (rooms[actX, actY].minimapImage == null)
            {
                Image temp;
                temp = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                rooms[actX, actY].minimapImage = temp;
                temp.rectTransform.SetParent(minimapPanel.transform);
                temp.rectTransform.localPosition = new Vector3(-75, -75, 0);
                temp.rectTransform.localScale = Vector3.one;
            }
            try
            {
                if (rooms[actX, actY + 1].wasVisited == false && rooms[actX,actY + 1].isGenerated)
                {
                    Image tempU;
                    tempU = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                    rooms[actX, actY + 1].minimapImage = tempU;
                    tempU.rectTransform.SetParent(minimapPanel.transform);
                    tempU.rectTransform.localPosition = new Vector3(rooms[actX, actY].minimapImage.rectTransform.localPosition.x, rooms[actX, actY].minimapImage.rectTransform.localPosition.y + rooms[actX, actY].minimapImage.rectTransform.sizeDelta.y + 1.5f, 0);
                    tempU.rectTransform.localScale = Vector3.one;
                    tempU.color = Color.gray;
                }
            }
            catch { }
            try
            {
                if (rooms[actX, actY - 1].wasVisited == false && rooms[actX, actY - 1].isGenerated)
                {
                    Image tempD;
                    tempD = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                    rooms[actX, actY - 1].minimapImage = tempD;
                    tempD.rectTransform.SetParent(minimapPanel.transform);
                    tempD.rectTransform.localPosition = new Vector3(rooms[actX, actY].minimapImage.rectTransform.localPosition.x, rooms[actX, actY].minimapImage.rectTransform.localPosition.y - rooms[actX, actY].minimapImage.rectTransform.sizeDelta.y - 1.5f, 0);
                    tempD.rectTransform.localScale = Vector3.one;
                    tempD.color = Color.gray;
                }
            }
            catch { }
            try
            {
                if (rooms[actX + 1, actY].wasVisited == false && rooms[actX + 1, actY].isGenerated)
                {
                    Image tempR;
                    tempR = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                    rooms[actX+1, actY].minimapImage = tempR;
                    tempR.rectTransform.SetParent(minimapPanel.transform);
                    tempR.rectTransform.localPosition = new Vector3(rooms[actX, actY].minimapImage.rectTransform.localPosition.x + rooms[actX, actY].minimapImage.rectTransform.sizeDelta.x + 1.5f, rooms[actX, actY].minimapImage.rectTransform.localPosition.y, 0);
                    tempR.rectTransform.localScale = Vector3.one;
                    tempR.color = Color.gray;
                }
            }
            catch { }
            try
            {
                if (rooms[actX - 1, actY].wasVisited == false && rooms[actX - 1, actY].isGenerated)
                {
                    Image tempL;
                    tempL = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                    rooms[actX-1, actY].minimapImage = tempL;
                    tempL.rectTransform.SetParent(minimapPanel.transform);
                    tempL.rectTransform.localPosition = new Vector3(rooms[actX, actY].minimapImage.rectTransform.localPosition.x - rooms[actX, actY].minimapImage.rectTransform.sizeDelta.x - 1.5f, rooms[actX, actY].minimapImage.rectTransform.localPosition.y, 0);
                    tempL.rectTransform.localScale = Vector3.one;
                    tempL.color = Color.gray;
                }
            }
            catch { }

        }
        Debug.Log("Ustawiam " + actX + ", " + actY + " na zielony");
		rooms[actX, actY].minimapImage.color = Color.green;
        rooms[actX, actY].wasVisited = true;
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
		Random.seed = Static.randomSeed;
		//Debug.Log ("WorldGenerator::Generation() seed=" + Random.seed);
		int posX = width/2, posY = height/2;
		dfsArray = new bool[width, height];
		dfsArray [posX, posY] = true;
		edges.Clear();
		for (int i = 0; i < roomCount; i++)
		{
			if(posX >= 0 && posX < width && posY + 1 >= 0 && posY + 1 < height)
				edges.Add(new Vector2((int)posX, (int)(posY + 1)));

			if(posX + 1>= 0 && posX + 1 < width && posY >= 0 && posY < height)
				edges.Add(new Vector2((int)(posX + 1), (int)posY));

			if(posX >= 0 && posX < width && posY - 1>= 0 && posY -1 < height)
				edges.Add(new Vector2((int)posX, (int)(posY - 1)));

			if(posX - 1 >= 0 && posX -1 < width && posY >= 0 && posY < height)
				edges.Add(new Vector2((int)(posX - 1), (int)posY));
			while (true)
			{
				int z = Random.Range(0, edges.Count - 1);           //integery
				//Debug.Log ("zzz " + z);
				//Debug.Log ("WorldGenerator::Generation() after Random.Range seed=" + Random.seed);
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
		addShop ();
		//addBoss();
		didIGenerateYet = true;
	}

	public void removeItemFromList(int id){
		foreach (Vector3 vec in rooms[actX, actY].vecItems) {
			if(vec.z == id){
				rooms[actX,actY].vecItems.Remove(vec);
				//rooms[actX,actY].itemPool.Remove((int)vec.z);
				return;
			}
		}
	}
	public Vector2 addSpecialRoom(int edgesMin, int edgesMax, int roomId)
	{
		List<Vector2> candidates;
		candidates=new List<Vector2>();
		int tmp;
		for(int i=0; i<width; i++)
		{
			for(int j=0; j<height; j++)
			{
				if(dfsArray[i, j]==true)
				{
					tmp=0;
					if(i-1>-1)
						if(dfsArray[i-1,j]==true)
							tmp++;
					if(j-1>-1)
						if(dfsArray[i,j-1]==true)
							tmp++;
					if(i+1<width)
						if(dfsArray[i+1,j]==true)
							tmp++;
					if(j+1<height)
						if(dfsArray[i,j+1]==true)
							tmp++;
					if(tmp>=edgesMin&& tmp<=edgesMax)
						candidates.Add(new Vector2(i, j));
				}
			}
		}
		tmp = (int)(UnityEngine.Random.value * candidates.Count);
		if (tmp < 0 || tmp >= candidates.Count) {     //Random.value -> [0,1] inclusive!!
			tmp= 0;
		}
		rooms [(int)candidates [tmp].x, (int)candidates [tmp].y].roomType = roomId;
		rooms [(int)candidates [tmp].x, (int)candidates [tmp].y].vecEnemies.Clear();
		rooms [(int)candidates [tmp].x, (int)candidates [tmp].y].maxiEnemies=0;
		return candidates [tmp];
	}
	void addShop()
	{
		addSpecialRoom (2, 4, 2);
	}
	void addBoss(List<Vector2> bossPositions, int unnkownVariable, List<int> bossNumber, int numberOfBosses)
	{
		Vector2 bossRoom;
		bossRoom = addSpecialRoom (1, 1, 3);
		rooms [(int)bossRoom.x, (int)bossRoom.y].maxiEnemies = numberOfBosses;
		for (int i=0; i<numberOfBosses; i++)
			rooms [(int)bossRoom.x, (int)bossRoom.y].vecEnemies.Add (new Vector4(bossPositions [i].x, bossPositions [i].y, unnkownVariable, bossNumber [i]));
	}
}
