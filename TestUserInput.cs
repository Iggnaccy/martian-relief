using UnityEngine;
using System.Collections;

public class TestUserInput : MonoBehaviour {

    public GameObject myPelletPrefab;
    public float speedStatMultiplier;

    private Rigidbody2D myBody;
	private ConstantForce2D force;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
		force = GetComponent<ConstantForce2D> ();
    }

    void Shoot(int direction) // 1 - prawo, 2 - lewo, 3 - góra, 4 - dół
    {
		if (GetComponent<BasicStats> ().tryToShoot () == false)
			return;
        GameObject clone;
        clone = Instantiate(myPelletPrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(0,0,0)) as GameObject;
        if (direction == 1) {
			clone.GetComponent<PelletBehaviour> ().shotHorizontal = 1;
			myBody.velocity+=new Vector2(-10,0);
		}
        else
        if (direction == 2)
		{
            clone.GetComponent<PelletBehaviour>().shotHorizontal = -1;
			myBody.velocity += new Vector2 (10, 0);
		}
        else clone.GetComponent<PelletBehaviour>().shotHorizontal = 0;
        if (direction == 3)
		{
            clone.GetComponent<PelletBehaviour>().shotVertical = 1;
			myBody.velocity+=new Vector2(0,-10);
		}
        else
        if (direction == 4)
		{
            clone.GetComponent<PelletBehaviour>().shotVertical = -1;
			myBody.velocity+=new Vector2(0,10);
		}
        else clone.GetComponent<PelletBehaviour>().shotVertical = 0;
        clone.GetComponent<PelletBehaviour>().damage = GetComponent<BasicStats>().damage.GetValue();
        clone.transform.parent = GameObject.Find("MissileHolder").transform;

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
    }

	void FixedUpdate () 
	{
		myBody.velocity = myBody.velocity+new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier*0.25f, Input.GetAxis("Vertical") * Time.deltaTime * GetComponent<BasicStats>().moveSpeed.GetValue() * speedStatMultiplier*0.25f);
    }
}
