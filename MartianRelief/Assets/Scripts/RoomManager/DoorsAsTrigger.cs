using UnityEngine;
using System.Collections;

public class DoorsAsTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("Collision with doors! " + other.gameObject.name);
		if (other.gameObject.tag == "Player") {
			GameObject.Find ("RoomManager").GetComponent<RoomManager>().tryDoors(gameObject);
		}
	}
}
