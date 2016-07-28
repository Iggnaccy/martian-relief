using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MerchantBehavoiur : MonoBehaviour{

	Canvas canvas;
	int[] randItems;
	Room room;
	Text textButton1;
	Text textButton2;
	Text textButton3;
	Text textButtonReload;
	BasicStats playerStats;
	PrefabHolder prefabHolder;
	GameObject itemHolder;
	WorldGenerator worldGenerator;

	int reloadCost=0;

	void Start(){
		Debug.Log ("START!");
		canvas = GameObject.Find ("MerchantCanvas").GetComponent<Canvas> () as Canvas;
		room = GameObject.Find ("RoomManager").GetComponent<RoomManager>().roomToLoad;
		textButton1 = (Text)GameObject.Find ("TextItem1").GetComponent<Text>();
		textButton2 = GameObject.Find ("TextItem2").GetComponent<Text>();
		textButton3 = GameObject.Find ("TextItem3").GetComponent<Text>();
		textButtonReload = GameObject.Find ("TextReload").GetComponent<Text>();
		playerStats = GameObject.Find ("Player").GetComponent<BasicStats>() as BasicStats;
		prefabHolder = GameObject.Find ("PrefabHolder").GetComponent<PrefabHolder> ();
		itemHolder = GameObject.Find ("ItemHolder");
		worldGenerator = GameObject.Find ("RoomManager").GetComponent<WorldGenerator> ();

		randItems = new int[3];
		if (worldGenerator.merchantItems [room.x, room.y] == null) {
			Debug.Log ("nowy klient");
			List<int> realItemPool = new List<int> ();
			realItemPool = Static.listDifference (room.itemPool, Static.itemsSpawned);
			for (int i = 0; i < randItems.GetLength(0); i++) {
				if (realItemPool.Count == 0) {
					randItems [i] = -1;
					continue;
				}
				randItems [i] = realItemPool [Static.randomIdxFromList<int> (realItemPool)];
				realItemPool.Remove (randItems [i]);
			}
			worldGenerator.merchantItems [room.x, room.y] = new List<int> ();
			worldGenerator.merchantItems [room.x, room.y].Add (randItems [0]);
			worldGenerator.merchantItems [room.x, room.y].Add (randItems [1]);
			worldGenerator.merchantItems [room.x, room.y].Add (randItems [2]);
			foreach(int x in worldGenerator.merchantItems[room.x, room.y]){
				Debug.Log ("generated:" + x);
			}
		} 
		else {
			Debug.Log("staly klient");
			List<int> realItemPool = new List<int> ();
			realItemPool = Static.listDifference (room.itemPool, Static.itemsSpawned);
			for(int i = 0; i < randItems.GetLength(0); i++) {
				if(realItemPool.Contains(worldGenerator.merchantItems [room.x, room.y][i])){
					randItems[i] = worldGenerator.merchantItems [room.x, room.y][i];
					Debug.Log ("Contains: " + Static.items[randItems[i]].name);
				}
				else
					randItems[i] = -1;
			}
			foreach(int x in randItems){
				Debug.Log ("Contains2: " + x);
			}
		}
		//Debug.Log ("wtf button names " + randItems[0]+" "+randItems[1]+" "+randItems[2]);
		//Debug.Log (Static.items [randItems [0]].name + " " + Static.items [randItems [1]].name + " " + Static.items [randItems [2]].name);
		//Debug.Log (Static.items [randItems [0]].cost + " " + Static.items [randItems [1]].cost + " " + Static.items [randItems [2]].cost);
	}

	void Update() {
		textButton1 = (Text)GameObject.Find ("TextItem1").GetComponent<Text>();
		textButton2 = GameObject.Find ("TextItem2").GetComponent<Text>();
		textButton3 = GameObject.Find ("TextItem3").GetComponent<Text>();
		textButtonReload = GameObject.Find ("TextReload").GetComponent<Text>();
		if (textButton1 == null || textButton2 == null || textButton3 == null || textButtonReload == null)
			return;
		reloadCost = 0;
		if (randItems[0] != -1) {
			textButton1.text = Static.items [randItems [0]].name.ToString() + " -> " + Static.items [randItems [0]].cost.ToString () + "$";
			reloadCost += Static.items[randItems[0]].cost;
		} else {
			textButton1.text = "EMPTY";
		}
		if (randItems[1] != -1) {
			textButton2.text = Static.items [randItems [1]].name.ToString() + " -> " + Static.items [randItems [1]].cost.ToString () + "$";
			reloadCost += Static.items[randItems[1]].cost;
		} else {
			textButton2.text = "EMPTY";
		}
		if (randItems[2] != -1) {
			textButton3.text = Static.items [randItems [2]].name.ToString() + " -> " + Static.items [randItems [2]].cost.ToString () + "$";
			reloadCost += Static.items[randItems[2]].cost;
		}
		else{
			textButton3.text = "EMPTY";
		}
		
		textButtonReload.text = "Reload -> " + reloadCost.ToString();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player")
		{
			Debug.Log ("Czego chciał?");
			canvas = GameObject.Find ("MerchantCanvas").GetComponent<Canvas> () as Canvas; //idk czemu ale tu jest canvas == null czasami
			createShop ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player")
		{
			canvas = GameObject.Find ("MerchantCanvas").GetComponent<Canvas> () as Canvas; //idk czemu ale tu jest canvas == null czasami
			canvas.enabled=false;
			Debug.Log ("nq");
		}
	}

	void createShop(){
		canvas.enabled = true;
	}

	public void onItem1Click(){
		if (randItems [0] == -1)
			return;
		if (playerStats.cash >= Static.items [randItems [0]].cost) {
			playerStats.cash -= Static.items [randItems [0]].cost;
			spawnItem(0);
		}
		Debug.Log ("item 1");
	}

	public void onItem2Click(){
		if (randItems [1] == -1)
			return;
		if (playerStats.cash >= Static.items [randItems [1]].cost) {
			playerStats.cash -= Static.items [randItems [1]].cost;
			spawnItem(1);
		}
		Debug.Log ("item 2");
	}

	public void onItem3Click(){
		if (randItems [2] == -1)
			return;
		if (playerStats.cash >= Static.items [randItems [2]].cost) {
			playerStats.cash -= Static.items [randItems [2]].cost;
			spawnItem(2);
		}
		Debug.Log ("item 3");
	}

	public void spawnItem(int idx) {
		GameObject temp = (Instantiate(prefabHolder.itemObject, new Vector3(Random.value * 4 - 2, Random.value * 4 - 2, 0), Quaternion.identity)) as GameObject;
		temp.GetComponent<ItemBehaviour>().itemID = randItems[idx];
		temp.transform.parent = itemHolder.transform;
		room.vecItems.Add (new Vector3 (temp.transform.position.x, temp.transform.position.y, randItems[idx]));
		
		Static.itemsSpawned.Add (randItems[idx]);
		
		if(room.itemPool.Contains(randItems[idx]))
			room.itemPool.Remove (randItems[idx]);
		randItems [idx] = -1;
	}

	public void onItemReloadClick(){
		Debug.Log ("item reload");
		if (playerStats.cash >= reloadCost) {
			playerStats.cash -= reloadCost;
		} else
			return;
		List<int> realItemPool = new List<int> ();
		realItemPool = Static.listDifference (room.itemPool, Static.itemsSpawned);
		for (int i = 0; i < randItems.GetLength(0); i++) {
			if (realItemPool.Count == 0) {
				randItems [i] = -1;
				continue;
			}
			randItems [i] = realItemPool [Static.randomIdxFromList<int> (realItemPool)];
			realItemPool.Remove (randItems [i]);
		}
		worldGenerator.merchantItems [room.x, room.y] = new List<int> ();
		worldGenerator.merchantItems [room.x, room.y].Add (randItems [0]);
		worldGenerator.merchantItems [room.x, room.y].Add (randItems [1]);
		worldGenerator.merchantItems [room.x, room.y].Add (randItems [2]);
		foreach(int x in worldGenerator.merchantItems[room.x, room.y]){
			Debug.Log ("generated: " + x);
		} 
	}
}
