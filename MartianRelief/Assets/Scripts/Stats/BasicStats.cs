using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasicStats : MonoBehaviour{

    public int hp;
	public int maxHp;
    public int cash;
    public int bombs;
	public Statistic moveSpeed;
	public Statistic attackSpeed;     //ile razy na sekunde? [Hz]
	public Statistic damage;
	public float invulnerabilityTime;
	public Slider healthSlider;

	public Timer timerAttack;
    public Timer timerInvoulnerable;

	void Start()
	{
		timerAttack = new Timer ();
        timerInvoulnerable = new Timer();
        invulnerabilityTime = 1.5f;
        healthSlider.maxValue = maxHp;
        healthSlider.value = hp;
        if (NewGameOrLoad.LoadName == null)
        {
            moveSpeed = new Statistic(350f);
            attackSpeed = new Statistic(4f);
            damage = new Statistic(7.5f);
        }
        DontDestroyOnLoad(gameObject);
	}

	void Update(){
		timerAttack.update (Time.deltaTime);
		timerInvoulnerable.update (Time.deltaTime);
        if (hp <= 0) OnDeath();
	}

	public bool tryToShoot(){
		if (timerAttack.getTime () >= (1f / attackSpeed.GetValue()) ) {
			timerAttack.reset();
			return true;
		}
		return false;
	}

    public void OnDeath()
    {
        Debug.Log("Umarłeś!");
        Destroy(gameObject);
    }

    public void OnDamageTaking(int damageToTake)
    {
        if(timerInvoulnerable.getTime() >= invulnerabilityTime)
        {
            timerInvoulnerable.reset();
            hp -= damageToTake;
			if(hp <= 0){
				OnDeath();
			}
            healthSlider.value = hp;
        }
    }
}
