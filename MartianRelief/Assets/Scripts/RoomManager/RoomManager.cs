using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	
	//public Transform enemyPrefab;
	public Transform enemyHolder;
	public Transform doorsHolder;
	public Transform missileHolder;
	public Transform prefabHolder;
	public float minX, maxX, minY, maxY;
	public List<Item> itemPool;
	
	enum RoomState{FIGHT,ROOM_CLEAR, LOAD_NEW_LEVEL};
	
	RoomState actualState = RoomState.LOAD_NEW_LEVEL;
	
	Room roomToLoad;
	
	void Start ()
	{
		itemPool = new List<Item>();
		generateItemPool();
		prefabHolder = GameObject.Find("PrefabHolder").transform;
	}
	
	void Update ()
	{
		if (actualState == RoomState.LOAD_NEW_LEVEL)
		{
			if(roomToLoad.wasCleared){
				actualState = RoomState.ROOM_CLEAR;
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
		int rand = (int)(Random.value * (itemPool.Count-1));
		GameObject temp = (Instantiate(prefabHolder.GetComponent<PrefabHolder>().itemObject, new Vector3(Random.value * 4 - 2, Random.value * 4 - 2, 0), Quaternion.identity)) as GameObject;
		temp.GetComponent<ItemBehaviour>().itemID = rand;
		itemPool.RemoveAt(rand);
	}
	
	void generateItemPool()
	{
		ItemStats player = GameObject.Find("Player").GetComponent<ItemStats>();
		// Dodać typy pokoi, dla których bazowy itemPool będzie różny
		for(int i = 0; i < player.items.GetLength (0); i++)
		{
			itemPool.Add(player.items[i]);
		}
	}
	
	public void loadNewRoom(Room room)
	{
		roomToLoad = room;
		actualState = RoomState.LOAD_NEW_LEVEL;
	}
	
	void spawnObjectsFromVectors(){
		if (roomToLoad == null) {
			actualState = RoomState.LOAD_NEW_LEVEL;
			return;
		}
		clearDoors();
		roomToLoad.spawnEnemies ();
		roomToLoad.spawnDoors (GameObject.Find ("PrefabHolder").GetComponent<PrefabHolder>().testDoors);
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
}
