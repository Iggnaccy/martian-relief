using UnityEngine;

public class SolmyrBehaviour : MonoBehaviour
{
    Vector2 center;
    PrefabHolder prefabHolder;
    Timer attackTimer;

    void Start()
    {
        center = GetComponent<BoxCollider2D>().offset;
        center.y = center.y + 2f;
        prefabHolder = GameObject.Find("PrefabHolder").GetComponent<PrefabHolder>();
        attackTimer = new Timer();
    }

    void Update()
    {
        attackTimer.update(Time.deltaTime);
        if(attackTimer.getTime() >= 1.25f)
        {
            if (Random.value > 0.5f)
                RingAttack();
            else SideAttack();
            attackTimer.reset();
        }
    }

    void OnDestroy()
    {
        if(GetComponent<BasicEnemyStats>().health <= 0)
            Instantiate(prefabHolder.passageToNextFloor);
    }

    void RingAttack()
    {
        float angle = Mathf.PI/8;
        GameObject[] clones = new GameObject[7];
        for(int i = 0; i < 7; i++)
        {
            clones[i] = Instantiate(prefabHolder.bossAttacks[0]);
            clones[i].transform.position = new Vector3(center.x + Mathf.Sin((i + 1) * angle + Mathf.PI/2), center.y + Mathf.Cos((i + 1) * angle + Mathf.PI / 2), 0);
            clones[i].transform.parent = GameObject.Find("MissileHolder").transform;
            clones[i].GetComponent<EnemyPelletBehaviour>().damage = GetComponent<BasicEnemyStats>().damage;
            clones[i].GetComponent<Rigidbody2D>().velocity = (new Vector3(center.x, center.y, 0) - clones[i].transform.position).normalized * -4.5f;
        }
    }

    void SideAttack()
    {
        float angle = Mathf.PI / 10;
        GameObject[] clones = new GameObject[18];
        for(int i = 0; i < 9; i++)
        {
            clones[i] = Instantiate(prefabHolder.bossAttacks[0]);
            clones[17-i] = Instantiate(prefabHolder.bossAttacks[0]);
            clones[i].transform.parent = clones[17 - i].transform.parent = GameObject.Find("MissileHolder").transform;
            clones[i].transform.position = new Vector3(center.x + Mathf.Sin((i + 1) * angle), center.y + Mathf.Cos((i + 1) * angle), 0);
            clones[17-i].transform.position = new Vector3(center.x + Mathf.Sin((i + 1) * angle + Mathf.PI), center.y + Mathf.Cos((i + 1) * angle + Mathf.PI), 0);
            clones[i].transform.localScale = new Vector3(2.5f, 2.5f);
            clones[17-i].transform.localScale = new Vector3(2.5f, 2.5f);
            clones[i].GetComponent<Rigidbody2D>().velocity = (new Vector3(center.x, center.y, 0) - clones[i].transform.position).normalized * -1.5f;
            clones[17-i].GetComponent<Rigidbody2D>().velocity = (new Vector3(center.x, center.y, 0) - clones[17-i].transform.position).normalized * -1.5f;
            clones[i].GetComponent<EnemyPelletBehaviour>().damage = clones[17 - i].GetComponent<EnemyPelletBehaviour>().damage = GetComponent<BasicEnemyStats>().damage;
        }
    }
}
