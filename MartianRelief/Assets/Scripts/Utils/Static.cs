using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Static {
	//wartości wziąłem z edytora, idk czemu enemyHp = 11.5, czy wy to z delty liczyliście? ;)
	public static int playerHp = 4;
	public static float playerMoveSpeed = 250.0f;
	public static float playerAttackSpeed = 3.5f;
	public static float playerDmg = 6f;

	public static float enemyHp = 11.5f;

	public static int randomSeed = (int)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;

	public static List<int> itemPoolGlobal;			//itemy odblokowane przez gracza

	public static int itemCount = 12;
	public static int itemTypesCount = 4;      //opisane w 21 linijce Room.cs

	public static int CANVAS_WIDTH = 800;
	public static int CANVAS_HEIGHT = 600;
	public static int MINIMAP_HEIGHT = 150;
	public static int MINIMAP_WIDTH = 150;


	public static void generateGlobalItemPool(){
		itemPoolGlobal = new List<int>();
		for(int i = 1; i <= itemCount; i++){
			itemPoolGlobal.Add (i);
		}
	}
	public static void setRoomItemPool(List<int> pool, int roomType /*od 1 do 4*/){
		pool.Clear ();
		for (int i = 0; i < itemCount/itemTypesCount; i++) {
			pool.Add ((roomType-1)*(itemCount/itemTypesCount)+i);
		}
	}

	public static int getUnixTime(){
		return (int)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
	}

	//template'y w c# są do bani więc na razie tylko na intach
	//c# przekazuje obiekty utworzone poprzez new przez wartosc referencji (plot_twist: nie przez referencje!)

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
	
	public static int randomIdxFromList<T>(List<T> vec){
		Debug.Log (UnityEngine.Random.value);
		int valToReturn = (int)(UnityEngine.Random.value * vec.Count);
		if (valToReturn < 0 || valToReturn >= vec.Count) {     //Random.value -> [0,1] inclusive!!
			return 0;
		}
		return valToReturn;
	}
}
