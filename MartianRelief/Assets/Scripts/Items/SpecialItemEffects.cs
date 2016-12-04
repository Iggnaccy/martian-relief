using UnityEngine;
using System.Collections;

public class SpecialItemEffects : MonoBehaviour
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

    public Effects myEffects;

    public void OnHitEffects(GameObject hit, GameObject source)
    {
        if((myEffects & Effects.FIERY) == Effects.FIERY)
        {
            StartCoroutine("Fire", hit);
        }
        if((myEffects & Effects.POISON) == Effects.POISON)
        {
            StartCoroutine("Poison", hit);
        }
        if((myEffects & Effects.KNOCKBACK) == Effects.KNOCKBACK)
        {
            hit.GetComponent<Rigidbody2D>().velocity += source.GetComponent<Rigidbody2D>().velocity * 2;
        }
    }

    IEnumerator Fire(GameObject hit)
    {
        Timer timer = new Timer();
        while(timer.getTime() < 1.5f)
        {
            hit.GetComponent<BasicEnemyStats>().health -= 7.5f * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Poison(GameObject hit)
    {
        Timer timer = new Timer();
        while (timer.getTime() < 2.5f)
        {
            hit.GetComponent<BasicEnemyStats>().health -= 9f * Time.deltaTime;
            yield return null;
        }
    }
}
