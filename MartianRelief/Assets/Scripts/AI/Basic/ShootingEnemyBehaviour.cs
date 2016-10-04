using UnityEngine;
using System.Collections;

public class ShootingEnemyBehaviour : MonoBehaviour
{
    GameObject target;
    public PrefabHolder prefabHolder;
    public string targetTag;
    public bool doIShootDirectly;
    Timer attackSpeed;
    public float attackDelay;
    public float pelletSpeed;

    void Start()
    {
        attackSpeed = new Timer();
        prefabHolder = GameObject.Find("PrefabHolder").GetComponent<PrefabHolder>();
    }

    void Update()
    {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag(targetTag);
        }
        if(attackSpeed.getTime() >= attackDelay)
        {
            if (doIShootDirectly)
            {
                ShootDirectly();
            }
            else ShootStraight();
            attackSpeed.reset();
        }
        attackSpeed.update(Time.deltaTime);
    }

    void ShootDirectly()
    {
        Debug.Log("Próbuję strzelać");
        GameObject temp = Instantiate(prefabHolder.enemyShots[0], transform.position, Quaternion.identity) as GameObject;
        temp.GetComponent<Rigidbody2D>().velocity = (target.transform.position - this.transform.position).normalized * pelletSpeed;
        temp.GetComponent<EnemyPelletBehaviour>().damage = GetComponent<BasicEnemyStats>().damage;
    }

    void ShootStraight()
    {
        GameObject temp = Instantiate(prefabHolder.enemyShots[0], transform.position, Quaternion.identity) as GameObject;
        temp.GetComponent<Rigidbody2D>().velocity = Straighten(target.transform.position - transform.position).normalized * pelletSpeed;
        temp.GetComponent<EnemyPelletBehaviour>().damage = GetComponent<BasicEnemyStats>().damage;
    }

    Vector3 Straighten(Vector3 vec)
    {
        float x, y;
        x = vec.x;
        y = vec.y;
        if (Mathf.Abs(x) >= Mathf.Abs(y))
        {
            vec.y = 0;
        }
        else vec.x = 0;
        return vec;
    }
}
