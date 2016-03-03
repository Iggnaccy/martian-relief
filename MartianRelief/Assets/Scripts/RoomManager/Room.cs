using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room{

	int x, y;
	public List <Vector3> vecEnemies;                      //Vector[x,y,rotacja]
	public List <Vector3> vecDoors;
	//List <Vector3> vecObstacles;
	public bool isGenerated = false;                //czy to jest pokój?
	int minX, maxX, minY, maxY;
	public float [] doors; //lewo, góra, prawo, dół,       jeżeli doors[x] < -1000 to doors[x] nie istnieje
	Room [,] allRooms;

	int width, height;
	int maxDoors = 4;
	public Room(int _x, int _y, Vector4 minmax, Room[,] _allRooms, int _width, int _height){
		x = _x;
		y = _y;
		width = _width;
		height = _height;
		vecEnemies = new List<Vector3>();
		vecDoors = new List<Vector3>();
		//vecObstacles = new List<Vector3>();

		doors = new float[4];
		allRooms = _allRooms;

		minX = (int)minmax.x;
		maxX = (int)minmax.y;
		minY = (int)minmax.z;
		maxY = (int)minmax.w;

		//Debug.Log (doors [0] + " " + doors [1] + " " + doors [2] + " " + doors [3]);
		doors [0] = doors [1] = doors [2] = doors [3] = -90000;

		//Debug.Log (allRooms.GetHashCode());
	}

	public void Generate(){
		isGenerated = true;
		int maxiEnemies = 1;
		for (int i = 0; i < maxiEnemies; i++) {
			generateEnemy();
		}
		for (int i = 0; i < maxDoors; i++) {
			//doors [i] = -90000;
		}
		for(int i = 0; i < maxDoors; i++){
			generateDoors(i, GameObject.Find ("PrefabHolder").GetComponent<PrefabHolder>().testDoors);
		}
	}

	public void spawnEnemies(){
		for(int i = 0; i < vecEnemies.Count; i++){
			spawnEnemy(GameObject.Find ("PrefabHolder").GetComponent<PrefabHolder>().testEnemy, new Vector2(vecEnemies[i].x, vecEnemies[i].y));
		}
	}

	public void spawnEnemy(GameObject enemyPrefab, Vector2 pos){
		Vector3 position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
		(GameObject.Instantiate (enemyPrefab, position, Quaternion.identity) as GameObject).transform.parent = GameObject.Find ("EnemyHolder").transform;
	}

	public void generateEnemy(){
		vecEnemies.Add(new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0));
	}

	public void spawnDoors(GameObject doorsPrefab){
		for (int i = 0; i < vecDoors.Count; i++) {
			Vector3 position = new Vector3(vecDoors[i].x, vecDoors[i].y, 0);
			if(position.x < -1000 || position.y < -1000)continue;
			GameObject tmpDoors = (GameObject.Instantiate (doorsPrefab.gameObject, position, Quaternion.identity) as GameObject);//
			tmpDoors.GetComponent<DoorsAsTrigger>().side = i;
			tmpDoors.transform.parent = GameObject.Find ("DoorsHolder").transform;
		}
	}

	//0->lewo, 1->góra, 2->prawo, 3 -> dół
	public void generateDoors(int side, GameObject doorsPrefab){
		float padding = 0.75f;
		float randX=0, randY=0;
		if (side == 0) {
			randX = minX;
			if(doors[0] < -1000){
				randY=Random.Range(minY+padding, maxY-padding);
				doors[side]=randY;
				if(x > 0 && allRooms[x-1,y].isGenerated) 
					allRooms[x-1,y].doors[2] = randY;
			}
			else{
				randY=doors[side];
			}

			/*if(x > 0 && allRooms[x-1,y].isGenerated){
				randY=Random.Range(minY+padding, maxY-padding);
				if(allRooms[x-1,y].doors[2] > -1000){
					randY = allRooms[x-1,y].doors[2];
				}
				else{
					Debug.Log ("AAAAAAAAAA");
				}
				randX = minX;
				doors[side]=randY;
			}*/
		}
		if (side == 1) {
			randY = maxY;
			if(doors[1] < -1000){
				randX=Random.Range(minX+padding, maxX-padding);
				doors[side]=randX;
				if(y < height-1 && allRooms[x,y+1].isGenerated) 
					allRooms[x,y+1].doors[3] = randX;
			}
			else{
				randX=doors[side];
			}
			/*if(y < height && allRooms[x,y+1].isGenerated){
				randX=Random.Range(minX+padding, maxX-padding);
				if(allRooms[x,y+1].doors[3] > -1000)
					randX = allRooms[x,y+1].doors[3];
				randY = maxY;
				doors[side]=randX;
			}*/
		}
		if (side == 2) {
			randX = maxX;
			if(doors[2] < -1000){
				randY=Random.Range(minY+padding, maxY-padding);
				doors[side]=randY;
				if(x < width-1 && allRooms[x+1,y].isGenerated) 
					allRooms[x+1,y].doors[0] = randY;
			}
			else{
				randY=doors[side];
			}
			/*if(x < width && allRooms[x+1,y].isGenerated){
				randY = Random.Range(minY+padding, maxY-padding);
				if(allRooms[x+1,y].doors[0] > -1000)
					randY = allRooms[x+1,y].doors[0];
				randX = maxX;
				doors[side]=randY;
			}*/
		}
		if (side == 3) {
			randY = minY;
			if(doors[3] < -1000){
				randX=Random.Range(minX+padding, maxX-padding);
				doors[side]=randX;
				if(y > 0 && allRooms[x,y-1].isGenerated) 
					allRooms[x,y-1].doors[1] = randX;
			}
			else{
				randX=doors[side];
			}
			/*if(y > 0 && allRooms[x,y-1].isGenerated){
				randX=Random.Range(minX+padding, maxX-padding);
				if(allRooms[x,y-1].doors[1] > -1000)
					randX = allRooms[x,y-1].doors[1];
				randY = minY;
				doors[side]=randX;
			}*/
		}
		if (doors [side] > -1000) {
			//Vector3 position = new Vector3(randX, randY, 0);
			//(GameObject.Instantiate (doorsPrefab.gameObject, position, Quaternion.identity) as GameObject).transform.parent = GameObject.Find ("DoorsHodler").transform;
			//
			vecDoors.Add(new Vector3(randX, randY, 0f));
		}
	}
}
