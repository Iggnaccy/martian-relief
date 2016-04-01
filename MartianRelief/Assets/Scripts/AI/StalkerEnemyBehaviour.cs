using UnityEngine;
using System.Collections;

public class StalkerEnemyBehaviour : MonoBehaviour
{

    Rigidbody2D myBody;
    Transform toStalk;
    string toSearch;

	void Start ()
    {
        myBody = GetComponent<Rigidbody2D>();
        toSearch = "Player";
	}
	
	void Update ()
    {
	    if(toStalk == null)
        {
            FindEnemy();
        }
	}

    void FixedUpdate()
    {
        AdjustVelocity();
    }

    void FindEnemy()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(toSearch);
        int randomTarget = Random.Range(0, possibleTargets.GetLength(0) - 1);
        while (possibleTargets[randomTarget] == this.gameObject)
        {
            randomTarget = Random.Range(0, possibleTargets.GetLength(0) - 1);
        }
        toStalk = possibleTargets[randomTarget].transform;
    }

    void AdjustVelocity()
    {
        myBody.velocity = (toStalk.position - transform.position).normalized * GetComponent<BasicEnemyStats>().movespeed * Time.deltaTime;
    }
}
