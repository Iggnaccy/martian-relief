using UnityEngine;
using System.Collections;

public class TestUserInput : MonoBehaviour
{

    public PrefabHolder prefabHolder;
    public float speedStatMultiplier;
    public BasicStats myStats;

    private Rigidbody2D myBody;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myStats = GetComponent<BasicStats>();
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
    }

    void FixedUpdate()
    {
        //myBody.velocity = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier, Input.GetAxis("Vertical") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier);
        // ===> velocity fix
        myBody.velocity = myBody.velocity + new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier * 0.25f, Input.GetAxis("Vertical") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier * 0.25f);

    }

    void ShootMany(int side)
    {
        if (GetComponent<BasicStats>().tryToShoot() == false)
            return;
        GameObject[] clones = new GameObject[myStats.shootAmount];
        Transform missileHolder = GameObject.Find("MissileHolder").transform;
        float[] angles = new float[myStats.shootAmount];
        angles[0] = Mathf.PI / 2;
        for (int i = 1; i < myStats.shootAmount; i++)
        {
            int x = 0;
            while (x < i)
            {
                angles[x] -= Mathf.PI / 72;
                x++;
            }
            angles[i] = angles[i - 1] + Mathf.PI / 36;
        }
        float sider = 0;
        if (side == 1) sider = -Mathf.PI / 2;
        if (side == 2) sider = Mathf.PI / 2;
        if (side == 4) sider = Mathf.PI;
        for(int i = 0; i < clones.Length; i++)
        {
            clones[i] = Instantiate(prefabHolder.pellets[myStats.shootType], new Vector3(transform.position.x + 0.1f * Mathf.Cos((angles[i] + sider)), transform.position.y + 0.1f * Mathf.Sin((angles[i] + sider)), 0f), Quaternion.Euler(0, 0, 0)) as GameObject;
            clones[i].transform.parent = missileHolder;
            if(myStats.shootType == 0)
                clones[i].GetComponent<PelletBehaviour>().damage = myStats.damage.GetValue();
            if (myStats.shootType == 1)
                clones[i].GetComponent<ShellBehaviour>().damage = myStats.damage.GetValue();
            if (myStats.shootType == 2) ;
            if (myStats.shootType == 3) ;
            if (myStats.shootType == 4) ;
            if (myStats.shootType == 5) ;
            clones[i].GetComponent<Rigidbody2D>().velocity = new Vector2(clones[i].transform.position.x - transform.position.x, clones[i].transform.position.y - transform.position.y).normalized * myStats.shotSpeed * Time.deltaTime;
        }
    }
}
