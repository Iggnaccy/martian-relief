using UnityEngine;
using System.Collections;
using System;

public static class Static {
	//wartości wziąłem z edytora, idk czemu enemyHp = 11.5, czy wy to z delty liczyliście? ;)
	public static int playerHp = 4;
	public static float playerMoveSpeed = 250.0f;
	public static float playerAttackSpeed = 3.5f;
	public static float playerDmg = 6f;

	public static float enemyHp = 11.5f;

	public static int randomSeed = (int)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;

	public static int getUnixTime(){
		return (int)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
	}
}
