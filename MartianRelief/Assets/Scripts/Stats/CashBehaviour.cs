using UnityEngine;
using System.Collections;

public class CashBehaviour : MonoBehaviour {

    public int value;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<BasicStats>().cash += value;
            Destroy(gameObject);
        }
    }
}
