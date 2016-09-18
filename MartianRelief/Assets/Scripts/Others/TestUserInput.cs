using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestUserInput : MonoBehaviour
{

    public PrefabHolder prefabHolder;
    public float speedStatMultiplier;
    public BasicStats myStats;

    private Rigidbody2D myBody;
    private LineRenderer line;


    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myStats = GetComponent<BasicStats>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    public void StartCalledByWorldGenerator()
    {
        prefabHolder = GameObject.Find("PrefabHolder").GetComponent<PrefabHolder>();
    }

    /*void Shoot(int direction) // 1 - prawo, 2 - lewo, 3 - góra, 4 - dół
    {
        if (GetComponent<BasicStats>().tryToShoot() == false)
            return;
        GameObject clone;
        clone = Instantiate(prefabHolder.pellet, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(0, 0, 0)) as GameObject;
        if (direction == 1)
        {
            clone.GetComponent<PelletBehaviour>().shotHorizontal = 1;
            // ===> velocity fix
            myBody.velocity += new Vector2(-10, 0);
        }
        else
        if (direction == 2)
        {
            clone.GetComponent<PelletBehaviour>().shotHorizontal = -1;
            // ===> velocity fix
            myBody.velocity += new Vector2(10, 0);
        }
        else clone.GetComponent<PelletBehaviour>().shotHorizontal = 0;
        if (direction == 3)
        {
            clone.GetComponent<PelletBehaviour>().shotVertical = 1;
            // ===> velocity fix
            myBody.velocity += new Vector2(0, -10);
        }
        else
        if (direction == 4)
        {
            clone.GetComponent<PelletBehaviour>().shotVertical = -1;
            // ===> velocity fix
            myBody.velocity += new Vector2(0, 10);
        }
        else clone.GetComponent<PelletBehaviour>().shotVertical = 0;
        clone.GetComponent<PelletBehaviour>().damage = myStats.damage.GetValue();
        clone.transform.parent = GameObject.Find("MissileHolder").transform;

    }*/

    void PlaceBomb(float damage)
    {
        BombBehaviour temp;
        temp = (Instantiate(prefabHolder.bomb, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject).GetComponent<BombBehaviour>();
        temp.damage = damage;
    }

    void Update()
    {
        myStats.timerAttack.update(Time.deltaTime);
        if (Input.GetAxis("FireHorizontal") > 0)
        {
            ShootMany(1);
        }
        if (Input.GetAxis("FireHorizontal") < 0)
        {
            ShootMany(2);
        }
        if (Input.GetAxis("FireVertical") > 0)
        {
            ShootMany(3);
        }
        if (Input.GetAxis("FireVertical") < 0)
        {
            ShootMany(4);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GetComponent<BasicStats>().bombs > 0)
            {
                PlaceBomb(GetComponent<BasicStats>().damage.GetValue());
                GetComponent<BasicStats>().bombs--;
            }
        }
        if (Input.GetAxis("FireVertical") == 0 && Input.GetAxis("FireHorizontal") == 0)
            line.enabled = false;
    }

    void FixedUpdate()
    {
        //myBody.velocity = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier, Input.GetAxis("Vertical") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier);
        // ===> velocity fix
        myBody.velocity = myBody.velocity + new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier * 0.25f, Input.GetAxis("Vertical") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier * 0.25f);

    }

    void ShootMany(int side)
    {
        if (myStats.shootType > 2)
        {
            if(myStats.shootType == 3)
                ShootLaser(side, false);
            if (myStats.shootType == 4)
                ShootLaser(side, true);
            return;
        }
        if (GetComponent<BasicStats>().tryToShoot() == false)
            return;
        GameObject[] clones = new GameObject[myStats.shootAmount];
        Transform missileHolder = GameObject.Find("MissileHolder").transform;
        List<float> angles = GenerateAngles(myStats.shootAmount, side);
        for (int i = 0; i < clones.Length; i++)
        {
            clones[i] = Instantiate(prefabHolder.pellets[myStats.shootType], new Vector3(transform.position.x + 0.1f * Mathf.Cos((angles[i])), transform.position.y + 0.1f * Mathf.Sin((angles[i])), 0f), Quaternion.Euler(0, 0, 0)) as GameObject;
            clones[i].transform.parent = missileHolder;
            if(myStats.shootType == 0)      //normal
                clones[i].GetComponent<PelletBehaviour>().damage = myStats.damage.GetValue();
            if (myStats.shootType == 1)     //shell
                clones[i].GetComponent<ShellBehaviour>().damage = myStats.damage.GetValue();
            if (myStats.shootType == 2)    //explosive
                clones[i].GetComponent<ExplosiveBehaviour>().damage = myStats.damage.GetValue();
            clones[i].GetComponent<Rigidbody2D>().velocity = new Vector2(clones[i].transform.position.x - transform.position.x, clones[i].transform.position.y - transform.position.y).normalized * myStats.shotSpeed;
        }
    }

    void ShootLaser(int side, bool isPenetrative)
    {
        line.SetVertexCount (myStats.shootAmount*2);
        line.enabled = true;
        List<float> angles = GenerateAngles(myStats.shootAmount, side);
		float range = 120.0f;
        for (int i = 0; i < angles.Count; i++) 
        {
			Vector2 direction = new Vector2(0.1f * Mathf.Cos((angles[i])), 0.1f * Mathf.Sin((angles[i])));
            //if(isPenetrative == false){
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 150f, LayerMask.GetMask("Raycastable"));
                line.SetPosition(i * 2, transform.position);
                if (hit.collider == null)
                    line.SetPosition(i * 2 + 1, new Vector2(transform.position.x, transform.position.y) + direction * range);
                else
                {
                    line.SetPosition(i * 2 + 1, hit.point);
                    if (hit.collider.tag == "Enemy")
                    {
                        if (myStats.tryToShoot())
                            hit.collider.GetComponent<BasicEnemyStats>().health -= myStats.damage.GetValue() * Time.deltaTime * myStats.attackSpeed.GetValue();
                        Debug.Log(myStats.timerAttack.getTime());
                    }
                }
                /*line.SetVertexCount (4);
                line.SetPosition (0, transform.position);
                line.SetPosition (1, transform.position + new Vector3(0, 1, 0));
                line.SetPosition (2, transform.position);
                line.SetPosition (3, transform.position + new Vector3(1, 0, 0));*/
            /*}
            else {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 150f, LayerMask.GetMask("Raycastable"));
                line.SetPosition(i * 2, transform.position);
                for(int j = 0; j < hits.Length; j++)
                {
                    line.SetPosition(i * 2 + 1, hits[j].point);
                    if (hits[j].collider.tag == "Enemy")
                    {
                        hits[j].collider.transform.position += new Vector3(hits[j].collider.transform.position.x - transform.position.x, hits[j].collider.transform.position.y - transform.position.y, 0).normalized / 10;
                        if (myStats.tryToShoot())
                            hits[j].collider.GetComponent<BasicEnemyStats>().health -= myStats.damage.GetValue() * Time.deltaTime * myStats.attackSpeed.GetValue();
                        
                    }
                }
            }*/
        }
    }

    List<float> GenerateAngles(int count, int side)
    {
        List<float> angles = new List<float>();
        angles.Add(Mathf.PI / 2 - (Mathf.PI / 72) * (count-1));
        float sider = 0;
        if (side == 1) sider = -Mathf.PI / 2;
        if (side == 2) sider = Mathf.PI / 2;
        if (side == 4) sider = Mathf.PI;
        for (int i = 1; i < count; i++)
        {
            angles.Add(angles[i - 1] + Mathf.PI / 36);
        }
        for (int i = 0; i < angles.Count; i++) 
        {
            angles[i] += sider;
        }
        return angles;
    }
}
