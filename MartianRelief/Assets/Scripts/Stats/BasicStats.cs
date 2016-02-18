using UnityEngine;
using System.Collections;

public class BasicStats : MonoBehaviour{

	public int hp;
	public int maxHp;
	public int moveSpeed;
	public int attackSpeed;     //ile razy na sekunde? [Hz]
	public int dmg;

	Timer timerAttack;

	void Start()
	{
		timerAttack = new Timer ();
	}

	void Update(){
		timerAttack.update (Time.deltaTime);
	}

	public bool tryToShoot(){
		if (timerAttack.getTime () >= (60f / (float)attackSpeed) ) {
			timerAttack.reset();
			return true;
		}
		return false;
	}
}
