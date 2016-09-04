using UnityEngine;
using System.Collections;

public class ExplosiveBehaviour : MonoBehaviour {

    public float damage, knockback;
    public float range = 2.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<BasicEnemyStats>().health -= damage;
            // ==> velocity fix
            if (other.attachedRigidbody)
                other.attachedRigidbody.velocity += GetComponent<Rigidbody2D>().velocity * 2;
            Collider2D[] objectsHit = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), range);
            foreach (Collider2D hit in objectsHit)
            {
                if (hit.tag == "Player")
                    hit.GetComponent<BasicStats>().OnDamageTaking(1);
                if (hit.tag == "Enemy")
                    hit.GetComponent<BasicEnemyStats>().health -= damage;
                if (hit.tag == "") // Tutaj dodaj tag przeszkód na mapie. Zapewne będzie to bardziej skomplikowane niż po prostu "usuń", ale to się przemyśli jeszcze;
                    Destroy(hit.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
