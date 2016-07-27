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

	int reloadCost=0;

	void Start(){
		canvas = GameObject.Find ("MerchantCanvas").GetComponent<Canvas> () as Canvas;
		room = GameObject.Find ("RoomManager").GetComponent<RoomManager>().roomToLoad;
		textButton1 = (Text)GameObject.Find ("TextItem1").GetComponent<Text>();
		textButton2 = GameObject.Find ("TextItem2").GetComponent<Text>();
		textButton3 = GameObject.Find ("TextItem3").GetComponent<Text>();
		textButtonReload = GameObject.Find ("TextReload").GetComponent<Text>();
		playerStats = GameObject.Find ("Player").GetComponent<BasicStats>() as BasicStats;
		prefabHolder = GameObject.Find ("PrefabHolder").GetComponent<PrefabHolder> ();
		itemHolder = GameObject.Find ("ItemHolder");

		randItems = new int[3];

		List<int> realItemPool = new List<int> ();
		realItemPool = Static.listDifference (room.itemPool, Static.itemsSpawned);
		for (int i = 0; i < randItems.GetLength(0); i++){
			if(realItemPool.Count == 0) {
				randItems[i] = -1;
				continue;
			}
			randItems[i] = realItemPool[Static.randomIdxFromList<int>(realItemPool)];
			realItemPool.Remove(randItems[i]);
		}
		/*textButton1.text = "ASDAF";*/
		Debug.Log ("wtf button names " + randItems[0]+" "+randItems[1]+" "+randItems[2]);
		if (randItems[0] != -1) {
			Debug.Log ("op 1");
			textButton1.text = Static.items [randItems [0]].name.ToString() + " -> " + Static.items [randItems [0]].cost.ToString () + "$";
			reloadCost += Static.items[randItems[0]].cost;
		} else {
			Debug.Log ("el 1");
			textButton1.text = "EMPTY";
			randItems[0]=-1;
		}
		if (randItems[1] != -1) {
			Debug.Log ("op 2");
			textButton2.text = Static.items [randItems [1]].name.ToString() + " -> " + Static.items [randItems [1]].cost.ToString () + "$";
			reloadCost += Static.items[randItems[1]].cost;
		} else {
			Debug.Log ("el 2");
			textButton2.text = "EMPTY";
			randItems [1] = -1;
		}
		if (randItems[2] != -1) {
			Debug.Log ("op 3");
			textButton3.text = Static.items [randItems [2]].name.ToString() + " -> " + Static.items [randItems [2]].cost.ToString () + "$";
			reloadCost += Static.items[randItems[2]].cost;
		}
		else{
			Debug.Log ("el 3");
			textButton3.text = "EMPTY";
			randItems[2]=-1;
		}

		textButtonReload.text = "Reload -> " + reloadCost.ToString();

	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player")
		{
			Debug.Log ("Czego chciał?");
			canvas = GameObject.Find ("MerchantCanvas").GetComponent<Canvas> () as Canvas; //idk czemu ale sa tu nulle czasami
			createShop ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player")
		{
			canvas = GameObject.Find ("MerchantCanvas").GetComponent<Canvas> () as Canvas; //idk czemu ale sa tu nulle czasami
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
		
		//koniecznie na koncu zeby nie zaburzac kolejnosci listy (inaczej kolejne 30 minut debugowania)
		room.itemPool.Remove (room.itemPool [randItems[idx]]);
	}

	public void onItemReloadClick(){
		Debug.Log ("item reload");
	}
}
