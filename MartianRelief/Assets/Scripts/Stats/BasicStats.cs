using UnityEngine;
using System.Collections;

public class BasicStats : MonoBehaviour{

	public int hp;
	public int maxHp;
	public float moveSpeed;
	public float attackSpeed;     //ile razy na sekunde? [Hz]
	public float damage;
    public float invoulnerabilityTime;

	Timer timerAttack;
    Timer timerInvoulnerable;

	void Start()
	{
		timerAttack = new Timer ();
        timerInvoulnerable = new Timer();
        invoulnerabilityTime = 1.5f;
	}

	void Update(){
		timerAttack.update (Time.deltaTime);
		timerInvoulnerable.update (Time.deltaTime);
	}

	public bool tryToShoot(){
		if (timerAttack.getTime () >= (1f / attackSpeed) ) {
			timerAttack.reset();
			return true;
		}
		return false;
	}

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }

    public void OnDamageTaking(int damageToTake)
    {
        if(timerInvoulnerable.getTime() >= invoulnerabilityTime)
        {
            timerInvoulnerable.reset();
            hp -= damageToTake;
			Debug.Log ("dmg " + hp.ToString() );
			if(hp <= 0){
				OnDeath();
			}
        }
    }
}
