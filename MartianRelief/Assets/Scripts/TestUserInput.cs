using UnityEngine;
using System.Collections;

public class TestUserInput : MonoBehaviour {

    public float speed;

    private Rigidbody2D myBody;
    private float mySpeedX, mySpeedY;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        mySpeedX = 0;
        mySpeedY = 0;
    }

	void Update () 
	{
        myBody.velocity = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * speed, Input.GetAxis("Vertical") * Time.deltaTime * speed);
    }
}
