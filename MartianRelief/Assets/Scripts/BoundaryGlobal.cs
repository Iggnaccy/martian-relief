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
	}

}
