﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	public Transform enemyHolder;
	public Transform doorsHolder;
	public Transform missileHolder;
	public Transform itemHolder;
	public Transform prefabHolder;
	public float minX, maxX, minY, maxY;

	enum RoomState{FIGHT,ROOM_CLEAR, LOAD_NEW_LEVEL};
	
	RoomState actualState = RoomState.LOAD_NEW_LEVEL;
	
	public Room roomToLoad;

	Timer timeElapsed;

	void Start ()
	{
		timeElapsed = new Timer ();
		prefabHolder = GameObject.Find("PrefabHolder").transform;
	}
	
	void Update ()
	{
		timeElapsed.update (Time.deltaTime);
		if (actualState == RoomState.LOAD_NEW_LEVEL)
		{
			//Debug.Log ("wtf\n");  	//przy ladowaniu wczesniej wyczyszczonego pokoju wykonuje sie to kilkanascie razy
										//przenioslem czesc rzeczy do laodNewRoom
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
		List<int> realItemPool = new List<int> ();
		realItemPool = Static.listDifference (roomToLoad.itemPool, Static.itemsSpawned);
		if (realItemPool.Count == 0) {
			return;
		}
		int rand = Static.randomIdxFromList<int> (realItemPool);
		GameObject temp = (Instantiate(prefabHolder.GetComponent<PrefabHolder>().itemObject, new Vector3(Random.value * 4 - 2, Random.value * 4 - 2, 0), Quaternion.identity)) as GameObject;
		temp.GetComponent<ItemBehaviour>().itemID = realItemPool[rand];
		temp.transform.parent = itemHolder;
		addItemPositionToRoom (temp.transform.position.x, temp.transform.position.y,realItemPool[rand]);

		Static.itemsSpawned.Add (realItemPool[rand]);

		//koniecznie na koncu zeby nie zaburzac kolejnosci listy (inaczej kolejne 30 minut debugowania)
		roomToLoad.itemPool.Remove (roomToLoad.itemPool [rand]);
	}

	public void addItemPositionToRoom(float x, float y, int id){
		roomToLoad.vecItems.Add (new Vector3 (x, y, id)); 
	}

	public void buildItemsFromRoom(List<Vector3> vecIt){
		foreach(Vector3 v in vecIt){
			GameObject temp = (Instantiate(prefabHolder.GetComponent<PrefabHolder>().itemObject, new Vector3(v.x, v.y, 0), Quaternion.identity)) as GameObject;
			temp.transform.parent = itemHolder.transform;
			temp.GetComponent<ItemBehaviour>().itemID = (int)v.z;
		}
	}
	
	public void loadNewRoom(Room room)
	{
		roomToLoad = room;
		timeElapsed.reset ();
		timeElapsed.start ();
		actualState = RoomState.LOAD_NEW_LEVEL;


		//rzeczy z LOAD_NEW_LEVEL które chcemy uruchomic tylko raz
		Debug.Log("new level");
		clearMissiles();
		spawnObjectsFromVectors();
		spawnRoomTypeSpecialObjects();
	}
	
	void spawnObjectsFromVectors(){
		if (roomToLoad == null) {
			actualState = RoomState.LOAD_NEW_LEVEL;
			return;
		}
		clearDoors();
		clearItems ();
		clearDrops ();
		roomToLoad.spawnEnemies ();
		roomToLoad.spawnDoors (GameObject.Find ("PrefabHolder").GetComponent<PrefabHolder>().testDoors);
		buildDrops ();
		buildItemsFromRoom (roomToLoad.vecItems);
	}

	void buildDrops(){
		foreach(SpawnedDrop drop in Static.spawnedDrops){
			if(drop.roomX == roomToLoad.x && drop.roomY == roomToLoad.y){
				GameObject spawnPrefab = prefabHolder.GetComponent<PrefabHolder>().cashPickup;
				if(drop.id == 2){
					spawnPrefab = prefabHolder.GetComponent<PrefabHolder>().bombPickup;
				}
				GameObject newDrop=Instantiate(spawnPrefab, new Vector3(drop.x, drop.y, 0), 
				                               Quaternion.Euler(0,0,0)) as GameObject;
				newDrop.transform.SetParent(GameObject.Find ("DropHolder").transform);
				newDrop.GetComponent<IDScript>().id = drop.spawnedID;
			}
		}
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

	void clearDrops(){
		foreach(Transform child in GameObject.Find ("DropHolder").transform)
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

	void spawnRoomTypeSpecialObjects() {
		if (GameObject.FindGameObjectWithTag ("Merchant") != null) {
			Destroy (GameObject.FindGameObjectWithTag ("Merchant"));
		}
		if (roomToLoad.roomType == 2) //sklep 
		{
			GameObject newObj = Instantiate(prefabHolder.GetComponent<PrefabHolder>().merchant, 
			new Vector3(0.0f, 0.0f, transform.position.z), Quaternion.Euler(0,0,0)) as GameObject;
			newObj.transform.SetParent(GameObject.Find ("RoomManager").transform);
			newObj.tag = "Merchant";
			Debug.Log ("spawn!");
		}
	}
}
