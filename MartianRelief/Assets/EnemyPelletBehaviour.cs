using UnityEngine;
using System.Collections;

public class EnemyPelletBehaviour : MonoBehaviour
{
    public int damage;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<BasicStats>().OnDamageTaking(damage);
            Destroy(gameObject);
        }
    }
}
