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
            other.GetComponent<BasicEnemyStats>().health -= damage;
            // ==> velocity fix
            try
            {
                other.attachedRigidbody.velocity += GetComponent<Rigidbody2D>().velocity * 2;
            }
            catch { }
			Destroy(gameObject);
        }
    }
}
