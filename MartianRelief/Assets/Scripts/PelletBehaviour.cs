using UnityEngine;
using System.Collections;

public class PelletBehaviour : MonoBehaviour {

    public int shotVertical, shotHorizontal;
    public float missileSpeed;
    
	void Update ()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(shotHorizontal * missileSpeed * Time.deltaTime, shotVertical * missileSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Pocisk trafił");
            Destroy(other.gameObject);
        }
    }
}
