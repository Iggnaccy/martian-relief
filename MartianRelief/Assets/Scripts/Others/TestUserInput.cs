using UnityEngine;
using System.Collections;

public class TestUserInput : MonoBehaviour {

    public PrefabHolder prefabHolder;
    public float speedStatMultiplier;

    private Rigidbody2D myBody;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        prefabHolder = GameObject.Find("PrefabHolder").GetComponent<PrefabHolder>();
    }

    void Shoot(int direction) // 1 - prawo, 2 - lewo, 3 - góra, 4 - dół
    {
		if (GetComponent<BasicStats> ().tryToShoot () == false)
			return;
        GameObject clone;
        clone = Instantiate(prefabHolder.pellet, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(0,0,0)) as GameObject;
        if (direction == 1) {
			clone.GetComponent<PelletBehaviour> ().shotHorizontal = 1;
			// ===> velocity fix
			myBody.velocity += new Vector2 (-10, 0);
		} else
        if (direction == 2) {
			clone.GetComponent<PelletBehaviour> ().shotHorizontal = -1;
			// ===> velocity fix
			myBody.velocity += new Vector2 (10, 0);
		}
        else clone.GetComponent<PelletBehaviour>().shotHorizontal = 0;
        if (direction == 3) {
			clone.GetComponent<PelletBehaviour> ().shotVertical = 1;
			// ===> velocity fix
			myBody.velocity += new Vector2 (0, -10);
		} else
        if (direction == 4) {
			clone.GetComponent<PelletBehaviour> ().shotVertical = -1;
			// ===> velocity fix
			myBody.velocity+=new Vector2(0,10);
		}
        else clone.GetComponent<PelletBehaviour>().shotVertical = 0;
        clone.GetComponent<PelletBehaviour>().damage = GetComponent<BasicStats>().damage.GetValue();
        clone.transform.parent = GameObject.Find("MissileHolder").transform;

    }

    void PlaceBomb(float damage)
    {
        BombBehaviour temp;
        temp = (Instantiate(prefabHolder.bomb, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject).GetComponent<BombBehaviour>();
        temp.damage = damage;
        GetComponent<BasicStats>().bombs--;
    }

    void Update()
    {
        if(Input.GetAxis("FireHorizontal") > 0)
        {
            Shoot(1);
        }
        if (Input.GetAxis("FireHorizontal") < 0)
        {
            Shoot(2);
        }
        if (Input.GetAxis("FireVertical") > 0)
        {
            Shoot(3);
        }
        if (Input.GetAxis("FireVertical") < 0)
        {
            Shoot(4);
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

	void FixedUpdate () 
	{
		//myBody.velocity = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier, Input.GetAxis("Vertical") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier);
    	// ===> velocity fix
		myBody.velocity = myBody.velocity+new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier*0.25f, Input.GetAxis("Vertical") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier*0.25f);

	}
}
