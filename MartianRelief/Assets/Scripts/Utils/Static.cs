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

	public static List<int> itemPoolGlobal;

	public static void generateGlobalItemPool(){
		itemPoolGlobal = new List<int>();
		for(int i = 2; i <= 12; i+=2){
			itemPoolGlobal.Add (i);
		}
	}
	public static void setRoomItemPool(List<int> pool, int roomType){
		pool.Clear ();
		for (int i = 0; i < 20; i++) {
			pool.Add (roomType*20+i);
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
		return (int)(UnityEngine.Random.value * (vec.Count-1));
	}
}
