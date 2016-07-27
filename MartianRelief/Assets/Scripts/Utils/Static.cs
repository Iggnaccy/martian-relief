using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class SpawnedDrop{
	public float x, y;				//w przestrzeni pokoju
	public int roomX, roomY;		//w przestrzeni mapy
	public int id;
	public int spawnedID=0;

	public SpawnedDrop(float _x, float _y, int _roomX, int _roomY, int _id){
		x = _x;
		y = _y;
		roomX = _roomX;
		roomY = _roomY;
		id = _id;
		spawnedID = Static.actDrop;
	}
}

public static class Static {
	//wartości wziąłem z edytora, idk czemu enemyHp = 11.5, czy wy to z delty liczyliście? ;)
	public static int playerHp = 4;
	public static float playerMoveSpeed = 250.0f;
	public static float playerAttackSpeed = 3.5f;
	public static float playerDmg = 6f;

	public static float enemyHp = 11.5f;

	public static int randomSeed = (int)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;

	public static int CANVAS_WIDTH = 800;
	public static int CANVAS_HEIGHT = 600;
	public static int MINIMAP_HEIGHT = 150;
	public static int MINIMAP_WIDTH = 150;

	/*Itemy*/
	public static List<int> itemPoolGlobal;				//itemy odblokowane przez gracza
	public static List<int> itemsSpawned;				//itemy ktore pojawily sie chociaz raz w trakcie gry
														//zeby nie generowac powtorek
														//itemPoolGlobal oraz itemsSpawned wymagaja zapisywania/odczytywania

	public static List<SpawnedDrop> spawnedDrops;		//x,y,roomX,roomY,collectibleId{1->cash,2->bomb}
	public static int actDrop = 0;

	public static int itemCount = 12;					//ile itemkow
	public static List<Item> items;						//wszystkie istniejace itemy pula
	public static int itemTypesCount = 4;      			//opisane w 21 linijce Room.cs

	public static void removeDrop(int id){
		for(int i = 0; i < spawnedDrops.Count; i++){
			if(spawnedDrops[i].spawnedID == id){
				Debug.Log ("removing id="+spawnedDrops[i].spawnedID);
				spawnedDrops.Remove(spawnedDrops[i]);
			}
		}
	}

	public static void generateGlobalItemPool(){
		itemPoolGlobal = new List<int>();
		itemsSpawned = new List<int> ();
		items = new List<Item>();
		spawnedDrops = new List<SpawnedDrop>();
		readItemsFromFile ();
		itemCount = items.Count;
		Debug.Log("item size: " + items.Count);
		foreach (Item it in items) {
			it.print ();
		}
		for(int i = 0; i < itemCount; i++){
			itemPoolGlobal.Add (i);
		}
	}

	static void readItemsFromFile(){
		TextAsset txtAsset = Resources.Load("items") as TextAsset;
		//Debug.Log (txtAsset.text);
		string[] linesWithComments = txtAsset.text.Split('\n');
		List<string> lines = new List<string>();
		foreach(string str in linesWithComments){
			if(str.Length==0 || (str.Length!=0 && str[0]=='#') || !str.Contains(":")){
				continue;
			} 
			else{
				string tmp=stripSpaces(str);
				if(tmp.Length > 0){
					if(tmp[tmp.Length-1] == '\n'){
						tmp.Remove(tmp.Length-1);
					}
				}
				lines.Add (tmp);
			}
		}
		Item newItem=new Item();
		bool firstItem = true;
		int id = 0;
		for(int i = 0; i < lines.Count; i++){         //dla każdej linii pliku po usunieciu głupot i komentarzy
			string type="", value="";
			bool wasSemicolon = false;
			foreach(char c in lines[i]){
				if(!wasSemicolon && c == ':'){
					wasSemicolon = true;
				}else{
					if(c!=':'){
						if(wasSemicolon){
							value += c;
						}
						else{
							type += c;
						}
					}
				}
			}
			if(type == "item" && !firstItem){
				newItem.id=id;
				id++;
				//Debug.Log ("adding new item!");
				//newItem.print ();
				items.Add(newItem.deepClone(newItem));
			}
			if(type == "item"){
				firstItem=false;
			}
			List<string> values = new List<string>(value.Split (','));
			for(int j = 0; j < values.Count; j++){
				values[j] = Regex.Replace(values[j], @"\t|\n|\r", "");    //usuwamy białe znaki, chcemy tylko liczby bądź '*'
			}
			addItemStat(newItem, type, values);
			//Debug.Log (type+"=>"+value);
		}
		items.Add(newItem.deepClone(newItem));
	}

	static void addItemStat(Item item, string type, List<string> values){
		/*Debug.Log ("type:" + type);
		foreach (string str in values) {
			Debug.Log ("value:"+str);
		}*/
		if (type == "item") {
			if(values[0] == "*")
				item.name = "unknown item";
			else
				item.name = values[0];
			if(values[1] == "*")
				item.iconFile = "unknown.png";
			else
				item.iconFile = values[1];
		}
		if (type == "cost"){
			item.cost = int.Parse(values[0]);
		}
		Statistic newStat = new Statistic (0, 0, 1, 0);
		if ((type == "damage" || type == "attackSpeed" || type == "moveSpeed") && values.Count == 3) {
			newStat = new Statistic(0, float.Parse (values[0]),float.Parse (values[1]),float.Parse (values[2]));
		}
		if (type == "damage") {
			item.damage = newStat;
		}
		if (type == "attackSpeed") {
			item.attackSpeed = newStat;
		}
		if (type == "moveSpeed") {
			item.moveSpeed = newStat;
		}
		if (type == "roomType") {
			item.roomTypes.Clear();
			for(int i = 0; i<values.Count; i++){
				item.roomTypes.Add (Int32.Parse(values[i]));
			}
		}
	}

	static string stripSpaces(string str){
		string ret = "";
		int apoCtr=0;
		for(int i = 0; i < str.Length; i++){
			if(str[i]=='"'){
				apoCtr++;
				ret += '"';
			}
			else if(str[i] == ' ' || str[i] == '\t' || str[i] == '\n'){
				if(apoCtr %2 == 1){
					ret += str[i];
				}
			}
			else{
				ret += str[i];
			}
		}
		return ret;
	}

	public static void setRoomItemPool(List<int> pool, int roomType /*od 1 do 4*/){
		pool.Clear ();
		for (int i = 0; i < itemCount; i++) {
			for(int j = 0; j < items[i].roomTypes.Count; j++){
				if(items[i].roomTypes[j] == roomType){
					pool.Add (i);
					break;
				}
			}
		}
	}
	
	public static int getUnixTime(){
		return (int)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
	}
	
	//template'y w c# są do bani więc na razie tylko na intach
	//c# przekazuje obiekty utworzone poprzez new przez wartosc referencji (plot_twist: nie przez referencje!)

	//left ^ right
	public static List<int> listIntersect(List<int> left, List<int> right){
		int maxi = 0;
		for (int i = 0; i < left.Count; i++) {
			maxi = Math.Max (maxi, left [i]);
		}
		for (int i = 0; i < right.Count; i++) {
			maxi = Math.Max (maxi, right [i]);
		}
		List<int> vec = new List<int> ();
		for (int i = 0; i <= maxi; i++) {
			vec.Add (0);
		}
		for (int i = 0; i < left.Count; i++) {
			vec[left[i]]++;
		}
		for (int i = 0; i < right.Count; i++) {
			vec[right[i]]++;
		}
		List<int> res = new List<int>();
		for (int i = 0; i < vec.Count; i++) {
			if(vec[i] >= 2){
				res.Add (i);
			}
		}
		return res;
	}

	//let \ right, dla left={1,2,3,4}, right={2,4} zwroci {1,3}
	public static List<int> listDifference(List<int> left, List<int> right){
		List<int> result = new List<int>();
		for(int i = 0; i < left.Count; i++){
			if(!right.Contains(left[i]))
				result.Add (left[i]);
		}
		return result;
	}

	public static int randomIdxFromList<T>(List<T> vec){
		int valToReturn = (int)(UnityEngine.Random.value * vec.Count);
		if (valToReturn < 0 || valToReturn >= vec.Count) {     //Random.value -> [0,1] inclusive!!
			return 0;
		}
		return valToReturn;
	}
}
