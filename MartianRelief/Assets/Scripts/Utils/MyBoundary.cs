using UnityEngine;
using System.Collections;

public class MyBoundary : MonoBehaviour
{
    public float minX, maxX, minY, maxY;
	
	void Update ()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), 0);
	}
}
