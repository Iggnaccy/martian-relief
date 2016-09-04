using UnityEngine;
using System.Collections;

public class PelletBehaviour : MonoBehaviour
{
    
    public float damage, knockback;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<BasicEnemyStats>().health -= damage;
            // ==> velocity fix
            if (other.attachedRigidbody)
                other.attachedRigidbody.velocity += GetComponent<Rigidbody2D>().velocity * 2;
            Destroy(gameObject);
        }
    }
}
