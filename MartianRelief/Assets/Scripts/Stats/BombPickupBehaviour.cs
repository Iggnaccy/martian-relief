using UnityEngine;

public class BombPickupBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<BasicStats>().bombs++;
            Destroy(gameObject);
        }
    }
}
