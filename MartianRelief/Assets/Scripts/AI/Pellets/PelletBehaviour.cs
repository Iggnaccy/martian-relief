using UnityEngine;
using System.Collections;

public class PelletBehaviour : MonoBehaviour {
    
    public float missileSpeed, damage, knockback;
    
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
