using UnityEngine;
using System.Collections;

public class DoorsAsTrigger : MonoBehaviour {

	public int side;           

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Collision with doors! " + other.gameObject.name);
		if (other.gameObject.tag == "Player") {
			GameObject.Find ("RoomManager").GetComponent<RoomManager>().tryDoors(gameObject, side);
		}
	}

    void Start()
    {
        if (side == 0 || side == 2)
            transform.eulerAngles = new Vector3(0, 0, 90);
    }
}
