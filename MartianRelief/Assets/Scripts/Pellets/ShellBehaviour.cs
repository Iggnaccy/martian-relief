using UnityEngine;
using System.Collections;

public class ShellBehaviour : MonoBehaviour
{
    public float damage, knockback;
    public GameObject shellFragment;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<BasicEnemyStats>().health -= damage;
            // ==> velocity fix
            if (other.attachedRigidbody)
                other.attachedRigidbody.velocity += GetComponent<Rigidbody2D>().velocity * 2;
            GameObject temp = Instantiate(shellFragment, transform.position, Quaternion.identity) as GameObject;
            GameObject temp2 = Instantiate(shellFragment, transform.position, Quaternion.identity) as GameObject;
            temp.transform.localScale = temp2.transform.localScale = new Vector3(1.5f, 1.5f);
            temp.transform.parent = temp2.transform.parent = GameObject.Find("MissileHolder").transform;
            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.y, -GetComponent<Rigidbody2D>().velocity.x);
            temp2.GetComponent<Rigidbody2D>().velocity = new Vector2(-GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x);
            temp.GetComponent<PelletBehaviour>().damage = damage / 2;
            temp2.GetComponent<PelletBehaviour>().damage = damage / 2;
            Destroy(gameObject);
        }
    }
}
