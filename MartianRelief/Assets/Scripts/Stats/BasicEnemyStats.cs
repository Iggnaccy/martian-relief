using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasicEnemyStats : MonoBehaviour {

	public float health;
	public float healthMax;
    public float movespeed;
    public int damage;
    public int collisionDamage;
	public Slider slider;

	void Start ()
    {
		loadStatsFromStatic ();
		slider.maxValue = healthMax;
		slider.value = health;
	}

    void Update ()
    {
	    if(health <= 0)
        {
            OnDeath();
        }
		slider.value = health;
	}

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BasicStats>().OnDamageTaking(collisionDamage);
        }
    }

	public void loadStatsFromStatic(){
		health = Static.enemyHp;
		healthMax = Static.enemyHp;
	}
}
