using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasicStats : MonoBehaviour{

    public int hp;
	public int maxHp;
    public int cash;
    public int bombs;
    public int shootType;
    public int shootAmount;
    public float shotSpeed;
	public Statistic moveSpeed;
	public Statistic attackSpeed;     //ile razy na sekunde? [Hz]
	public Statistic damage;
	public float invulnerabilityTime;
	public Slider healthSlider;

	public Timer timerAttack;
    public Timer timerInvoulnerable;

	public static bool WeAreResponsibleToOurselvesForOurOwnExistence_ConsequentlyWeWantToBeTheTrueHelmsmanOfThisExistenceAndRefuseToAllowOurExistenceToResembleAMindlessActOfChance = false;

	void Awake(){
		timerInvoulnerable = new Timer ();
		timerAttack = new Timer ();
		if (WeAreResponsibleToOurselvesForOurOwnExistence_ConsequentlyWeWantToBeTheTrueHelmsmanOfThisExistenceAndRefuseToAllowOurExistenceToResembleAMindlessActOfChance) {
			GameObject.Destroy (gameObject);
		} 
		else {
			WeAreResponsibleToOurselvesForOurOwnExistence_ConsequentlyWeWantToBeTheTrueHelmsmanOfThisExistenceAndRefuseToAllowOurExistenceToResembleAMindlessActOfChance = true;
		}
	}

	void Start()
	{
		invulnerabilityTime = 1.5f;
		if (NewGameOrLoad.LoadName == null)
		{
			moveSpeed = new Statistic(350f);
			attackSpeed = new Statistic(4f);
			damage = new Statistic(7.5f);
            //shootType = 0;
            //shootAmount = 1;
		}
	}

	public void StartCalledByWorldGenerator(){
		if (timerInvoulnerable == null) {
			Debug.Log ("!null 1");
		}

		/*if (timerInvoulnerable == null) {
			Debug.Log ("!null 2");
		}*/

		healthSlider = GameObject.Find ("PlayerHealthBarSlider").GetComponent<Slider>();
		healthSlider.maxValue = maxHp;
		healthSlider.value = hp;

		DontDestroyOnLoad(gameObject);
		Debug.Log ("Koniec");
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
