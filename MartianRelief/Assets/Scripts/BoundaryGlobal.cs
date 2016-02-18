using UnityEngine;
using System.Collections;

public class BoundaryGlobal : MonoBehaviour {

    public float xMax, xMin, yMax, yMin;
	private float x, y;
    private bool stopX, stopY;

    void Start()
    {
        stopY = false;
        stopX = false;
    }

	void Update ()
    {
        x = transform.position.x;
        y = transform.position.y;
        transform.position = new Vector3(Mathf.Clamp(x, xMin, xMax), Mathf.Clamp(y, yMin, yMax), transform.position.z);
        if (transform.position.x != x) stopX = true;
        if (transform.position.y != y) stopY = true;
	}

    void FixedUpdate()
    {
        if (stopX)
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, GetComponent<Rigidbody2D>().velocity.y);
        if (stopY)
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
    }
}
