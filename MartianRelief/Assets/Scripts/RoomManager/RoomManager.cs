using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	
	//public Transform enemyPrefab;
	public Transform enemyHolder;
	public Transform doorsHolder;
	public Transform missileHolder;
	public Transform itemHolder;
	public Transform prefabHolder;
	public float minX, maxX, minY, maxY;
	public List<Item> itemPool;
	
	enum RoomState{FIGHT,ROOM_CLEAR, LOAD_NEW_LEVEL};
	
	RoomState actualState = RoomState.LOAD_NEW_LEVEL;
	
	Room roomToLoad;

	Timer timeElapsed;

	void Start ()
	{
		timeElapsed = new Timer ();
		itemPool = new List<Item>();
		prefabHolder = GameObject.Find("PrefabHolder").transform;
	}
	
	void Update ()
	{
		timeElapsed.update (Time.deltaTime);
		if (actualState == RoomState.LOAD_NEW_LEVEL)
		{
			GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
			foreach(GameObject obj in objs){
				obj.GetComponent<BasicStats>().timerInvoulnerable.reset();
			}
			if(roomToLoad.wasCleared){
				if (timeElapsed.getTime () > 0.5f) {	
					actualState = RoomState.ROOM_CLEAR;
				}
			}
			else {
				actualState = RoomState.FIGHT;
			}
			clearMissiles();
			spawnObjectsFromVectors();
		}
		else if (actualState == RoomState.FIGHT)
		{
			if (enemyHolder.transform.childCount == 0)
			{
				//Debug.Log ("roomstate->fight, time=" + timeElapsed.getTime());
				roomToLoad.vecEnemies.Clear();
				generateItem();
				actualState = RoomState.ROOM_CLEAR;
			}
		}
		else if (actualState == RoomState.ROOM_CLEAR)
		{
			openTheGates();
		}
	}
	
	public void generateItem()
	{
		//int rand = (int)(Random.value * (itemPool.Count-1));
		int rand = Static.randomIdxFromList<Item> (itemPool);
		//Debug.Log ("generating item: " + itemPool [rand].myID);
		GameObject temp = (Instantiate(prefabHolder.GetComponent<PrefabHolder>().itemObject, new Vector3(Random.value * 4 - 2, Random.value * 4 - 2, 0), Quaternion.identity)) as GameObject;
		temp.GetComponent<ItemBehaviour>().itemID = itemPool[rand].myID;
		temp.transform.parent = itemHolder;
		//itemPool.RemoveAt(rand);
		addItemToRoom (temp.transform.position.x, temp.transform.position.y, itemPool[rand].myID);

		//koniecznie na koncu zeby nie zaburzac kolejnosci listy (inaczej kolejne 30 minut debugowania)
		itemPool.Remove (itemPool[rand]);
	}

	public void addItemToRoom(float x, float y, int id){
		roomToLoad.vecItems.Add (new Vector3 (x, y, id)); 
	}

	public void buildItemsFromRoom(List<Vector3> vecIt){
		foreach(Vector3 v in vecIt){
			GameObject temp = (Instantiate(prefabHolder.GetComponent<PrefabHolder>().itemObject, new Vector3(v.x, v.y, 0), Quaternion.identity)) as GameObject;
			temp.transform.parent = itemHolder.transform;
			temp.GetComponent<ItemBehaviour>().itemID = (int)v.z;
		}
		itemPool.Clear ();
		for (int i = 0; i < roomToLoad.itemPool.Count; i++) {
			itemPool.Add (new Item(roomToLoad.itemPool[i]));
		}
	}
	
	public void loadNewRoom(Room room)
	{
		roomToLoad = room;
		timeElapsed.reset ();
		timeElapsed.start ();
		actualState = RoomState.LOAD_NEW_LEVEL;
	}
	
	void spawnObjectsFromVectors(){
		if (roomToLoad == null) {
			actualState = RoomState.LOAD_NEW_LEVEL;
			return;
		}
		clearDoors();
		clearItems ();
		roomToLoad.spawnEnemies ();
		roomToLoad.spawnDoors (GameObject.Find ("PrefabHolder").GetComponent<PrefabHolder>().testDoors);
		buildItemsFromRoom (roomToLoad.vecItems);
	}
	
	void openTheGates()
	{
		for(int i = 0; i < doorsHolder.childCount; i++)
		{
			doorsHolder.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(0f,1f,0f);
		}
	}
	
	public void tryDoors(GameObject doors, int side)
	{
		if (actualState == RoomState.ROOM_CLEAR)
		{
			//actualState = RoomState.LOAD_NEW_LEVEL;
			if(side == 0){
				GetComponent<WorldGenerator>().goToRoom(-1, 0, side);
			}
			if(side == 1){
				GetComponent<WorldGenerator>().goToRoom(0, 1, side);
			}
			if(side == 2){
				GetComponent<WorldGenerator>().goToRoom(1, 0, side);
			}
			if(side == 3){
				GetComponent<WorldGenerator>().goToRoom(0, -1, side);
			}
		}
	}
	
	void clearMissiles()
	{
		foreach(Transform child in missileHolder.transform)
		{
			Destroy(child.gameObject);
		}
	}
	
	void clearDoors()
	{
		foreach(Transform child in GameObject.Find ("DoorsHolder").transform)
		{
			Destroy(child.gameObject);
		}
	}

	void clearItems(){
		foreach(Transform child in GameObject.Find ("ItemHolder").transform)
		{
			Destroy(child.gameObject);
		}
	}
}
