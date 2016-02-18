using UnityEngine;
using System.Collections;

public class BasicEnemyStats : MonoBehaviour {

    public float health;
    public float healthMax;
    public float movespeed;
    public int damage;
    public int collisionDamage;

	void Start ()
    {
        collisionDamage = 1;
	}

    void Update ()
    {
	    if(health <= 0)
        {
            OnDeath();
        }
	}

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }

    void OnCollision2DEnter(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BasicStats>().OnDamageTaking(collisionDamage);
        }
    }
}
