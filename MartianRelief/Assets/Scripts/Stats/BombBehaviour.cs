using UnityEngine;
using System.Collections;

public class BombBehaviour : MonoBehaviour {

    Timer fuseTimer;
    public float fuseLength;
    public float range;
    public float damage;

    void Start()
    {
        fuseTimer = new Timer();
        fuseTimer.reset();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GetComponent<Collider2D>().isTrigger = false;
    }

    void Update()
    {
        fuseTimer.update(Time.deltaTime);
        if(fuseTimer.getTime() >= fuseLength)
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), range);
        foreach(Collider2D hit in objectsHit)
        {
            if (hit.tag == "Player")
                hit.GetComponent<BasicStats>().OnDamageTaking(1);
            if (hit.tag == "Enemy")
                hit.GetComponent<BasicEnemyStats>().health -= damage;
            if (hit.tag == "") // Tutaj dodaj tag przeszkód na mapie. Zapewne będzie to bardziej skomplikowane niż po prostu "usuń", ale to się przemyśli jeszcze;
                Destroy(hit.gameObject);
        }
        // Instantiate(prefabHolder.bombExplosionImage);
        Destroy(gameObject);
    }
}
