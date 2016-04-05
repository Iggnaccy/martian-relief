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
        if (possibleTargets.Length == 0) return;
        int randomTarget = Random.Range(0, possibleTargets.GetLength(0) - 1);
        while (possibleTargets[randomTarget] == this.gameObject)
        {
            randomTarget = Random.Range(0, possibleTargets.GetLength(0) - 1);
        }
        toStalk = possibleTargets[randomTarget].transform;
    }

    void AdjustVelocity()
    {
        if (toStalk != null)
            myBody.velocity = (toStalk.position - transform.position).normalized * GetComponent<BasicEnemyStats>().movespeed * Time.deltaTime;
        else
            myBody.velocity = new Vector3(myBody.velocity.x * 0.9f, myBody.velocity.y * 0.9f, 0);
    }
}
