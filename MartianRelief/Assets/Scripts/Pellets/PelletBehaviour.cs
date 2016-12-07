using UnityEngine;
using System.Collections;

public class PelletBehaviour : MonoBehaviour
{
	public enum Effects
	{
		NONE = 0,
		FIERY = 1,
		PENETRATIVE = 2,
		POISON = 4,
		KNOCKBACK = 8,
		LIGHTNING_BOLT = 16
	}

    public float damage, knockback;
    public string targetTag;
    public SpecialItemEffects myEffects;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == targetTag)
        {
            other.GetComponent<BasicEnemyStats>().health -= damage;
            myEffects.OnHitEffects(other.gameObject, gameObject);
            if((myEffects.myEffects & SpecialItemEffects.Effects.PENETRATIVE) != SpecialItemEffects.Effects.PENETRATIVE)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        if ((myEffects.myEffects & SpecialItemEffects.Effects.LIGHTNING_BOLT) == SpecialItemEffects.Effects.LIGHTNING_BOLT)
        {
            StartCoroutine("Lightning");
        }
    }

    IEnumerator Lightning()
    {
        Timer timer = new Timer();
        while(true)    
        {
            if (timer.getTime() > 0.5f)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2f);
                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag(targetTag))
                    {
                        hit.GetComponent<BasicEnemyStats>().health -= damage / 4;
                    }
                }
                timer.reset();
            }
            yield return null;
        }
    }
}
