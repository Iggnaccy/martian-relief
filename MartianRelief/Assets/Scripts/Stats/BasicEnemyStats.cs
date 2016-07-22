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
	public float dropChance;        //in range [0,1]

	PrefabHolder prefabHolder;

	void Start ()
    {
		loadStatsFromStatic ();
		slider.maxValue = healthMax;
		slider.value = health;
		prefabHolder = GameObject.Find("PrefabHolder").GetComponent<PrefabHolder>();
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
		trySpawningCollectible ();
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

	public void trySpawningCollectible(){
		float rand = Random.value;
		//Debug.Log ("drop spawn! " + rand + "/" + dropChance);
		if (rand < dropChance) {
			float rand2 = Random.value;
			GameObject toSpawn = prefabHolder.cashPickup;
			if(rand2<= 0.75){
				//cash
				Debug.Log ("Drop cash");
			}
			else{
				//bomb
				toSpawn = prefabHolder.bombPickup;
				Debug.Log ("Drop bomb");
			}
			GameObject newDrop=Instantiate(toSpawn, new Vector3(transform.position.x, transform.position.y, transform.position.z), 
			            Quaternion.Euler(0,0,0)) as GameObject;
			Debug.Log (newDrop.transform.lossyScale);
		}
	}
}
