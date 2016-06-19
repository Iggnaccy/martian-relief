using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Room{
	public int x, y;
	public List <Vector4> 	vecEnemies;                      	//Vector[x,y,rotacja,id]
	public List <Vector3> 	vecDoors;
	public List <Vector3> 	vecItems; 							//[x,y,id]
	//List <Vector3> vecObstacles;
	public bool isGenerated = false;                //czy to jest pokój?
    public bool wasVisited = false;					//czy byl juz odwiedzony?
	public bool wasCleared = false;					//czy powybijani wrogowie?
	public int minX, maxX, minY, maxY;
	public float [] doors; //lewo, góra, prawo, dół,       jeżeli doors[x] < -1000 to doors[x] nie istnieje
    public int maxiEnemies = 3;
    public Image minimapImage;
    PrefabHolder prefabHolder = GameObject.Find("PrefabHolder").GetComponent<PrefabHolder>();
	public int roomType;  //1->normaly, 2->sklep, 3->boss, 4->final boss (powinien byc tylko jeden pokoj z 4)
	public List<int> itemPool;     //pula itemów dla danego pokoju

	int width, height;           //szer i  wys całego levelu (domyślnie 11x11)
	int maxDoors = 4;
	public Room(int _x, int _y, Vector4 minmax, int _width, int _height){
		x = _x;
		y = _y;
		width = _width;
		height = _height;
		vecEnemies = new List<Vector4>();
		vecDoors = new List<Vector3>();
		vecItems = new List<Vector3>();
        //vecObstacles = new List<Vector3>();
        minimapImage = prefabHolder.minimapRoomImage.GetComponent<Image>();
		doors = new float[4];
		minX = (int)minmax.x;
		maxX = (int)minmax.y;
		minY = (int)minmax.z;
		maxY = (int)minmax.w;
		doors [0] = doors [1] = doors [2] = doors [3] = -9000;
		roomType = (int)(Random.value * 3.999)+1;
		itemPool = new List<int> ();
		Static.setRoomItemPool (itemPool, roomType);                        	  	//ogarnąć to jakoś sensowniej
		itemPool = Static.listIntersect (itemPool, Static.itemPoolGlobal);				
	}

	public void Generate(bool [,] dfsArray){
		isGenerated = true;
		for (int i = vecEnemies.Count; i < maxiEnemies; i++) {
			generateEnemy();
		}
		for(int i = vecDoors.Count; i < maxDoors; i++){
			generateDoors(i,prefabHolder.testDoors, dfsArray);
		}
	}

	public void spawnEnemies(){
		for(int i = 0; i < vecEnemies.Count; i++){
			spawnEnemy(prefabHolder.enemies[(int)vecEnemies[i].w], new Vector2(vecEnemies[i].x, vecEnemies[i].y));
		}
	}

	public void spawnEnemy(GameObject enemyPrefab, Vector2 pos){
		Vector3 position = new Vector3(pos.x, pos.y, 0);
		(GameObject.Instantiate (enemyPrefab, position, Quaternion.identity) as GameObject).transform.parent = GameObject.Find ("EnemyHolder").transform;
	}

	public void generateEnemy(){
		vecEnemies.Add(new Vector4(Random.Range(minX, maxX), Random.Range(minY, maxY), 0,Random.Range(0,prefabHolder.enemies.Length)));
	}

	public void spawnDoors(GameObject doorsPrefab){
		for (int i = 0; i < vecDoors.Count; i++) {
			//if(vecDoors[i] == null)continue;
			Vector3 position = new Vector3(vecDoors[i].x, vecDoors[i].y, 0);
			if(position.x < -1000 || position.y < -1000)continue;
			GameObject tmpDoors = (GameObject.Instantiate (doorsPrefab.gameObject, position, Quaternion.identity) as GameObject);//
			tmpDoors.GetComponent<DoorsAsTrigger>().side = i;
			tmpDoors.transform.parent = GameObject.Find ("DoorsHolder").transform;
		}
	}

	//0->lewo, 1->góra, 2->prawo, 3 -> dół
	public void generateDoors(int side, GameObject doorsPrefab, bool [,] dfsArray){
		float padding = 0.75f;
		float randX = 0, randY = 0;
		doors [side] = -90000;
		if (side == 0) {
			randX = minX;
			if(x != 0 && dfsArray[x-1,y]==true){
				randY = Random.Range (minY + padding, maxY - padding);
				doors[side]=randY;
			}
		}
		if (side == 1) {
			randY = maxY;
			if(y != height-1 && dfsArray[x,y+1]==true){
				randX = Random.Range (minX + padding, maxX - padding);
				doors[side]=randX;
			}
		}
		if (side == 2) {
			randX = maxX;
			if(x != width-1 && dfsArray[x+1,y]==true){
				randY = Random.Range (minY + padding, maxY - padding);
				doors[side]=randY;
			}

		}
		if (side == 3) {
			randY = minY;
			if(y != 0 && dfsArray[x,y-1]==true){
				randX = Random.Range (minX + padding, maxX - padding);
				doors[side]=randX;
			}
		}
		if (doors [side] > -1000) {
			vecDoors.Add (new Vector3 (randX, randY, 0f));
		} else {
			vecDoors.Add (new Vector3 (-90000, -90000, 0f));
		}
	}
}
