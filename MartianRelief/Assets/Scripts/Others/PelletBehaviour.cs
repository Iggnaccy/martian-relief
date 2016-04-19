using UnityEngine;
using System.Collections;

public class PelletBehaviour : MonoBehaviour {

    public int shotVertical, shotHorizontal;
    public float missileSpeed, damage, knockback;
    
	void Update ()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(shotHorizontal * missileSpeed * Time.deltaTime, shotVertical * missileSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //Debug.Log("Pocisk trafił, życie przed obrażeniami: " + other.GetComponent<BasicEnemyStats>().health.ToString());
            other.GetComponent<BasicEnemyStats>().health -= damage;
            //Debug.Log("Po obrażeniach: " + Mathf.Max(0, other.GetComponent<BasicEnemyStats>().health).ToString());
            Destroy(this.gameObject);
        }
    }
}
