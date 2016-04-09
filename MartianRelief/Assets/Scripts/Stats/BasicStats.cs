using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasicStats : MonoBehaviour{


	//[UWAGA!] żeby ktos sie nie dziwil
	//[UWAGA!] że zmienia a tam żadnej reakcji, to jest napisywane przez loadFromStatic()
	public int hp;
	public int maxHp;
	public float moveSpeed;
	public float attackSpeed;     //ile razy na sekunde? [Hz]
	public float damage;
	public float invulnerabilityTime;
	public Slider healthSlider;

	Timer timerAttack;
    Timer timerInvoulnerable;

	void Start()
	{
		loadStatsFromStatic ();
		timerAttack = new Timer ();
        timerInvoulnerable = new Timer();
        invulnerabilityTime = 1.5f;
        healthSlider.maxValue = maxHp;
        healthSlider.value = hp;
	}

	void Update(){
		timerAttack.update (Time.deltaTime);
		timerInvoulnerable.update (Time.deltaTime);
        if (hp <= 0) OnDeath();
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
        Debug.Log("Umarłeś!");
        Destroy(this.gameObject);
    }

    public void OnDamageTaking(int damageToTake)
    {
        if(timerInvoulnerable.getTime() >= invulnerabilityTime)
        {
            timerInvoulnerable.reset();
            hp -= damageToTake;
			Debug.Log ("dmg " + hp.ToString() );
			if(hp <= 0){
				OnDeath();
			}
            healthSlider.value = hp;
        }
    }

	public void loadStatsFromStatic(){
		hp = Static.playerHp;
		maxHp = Static.playerHp;
		moveSpeed = Static.playerMoveSpeed;
		attackSpeed = Static.playerAttackSpeed;     //ile razy na sekunde? [Hz]
		damage = Static.playerDmg;
	}
}
