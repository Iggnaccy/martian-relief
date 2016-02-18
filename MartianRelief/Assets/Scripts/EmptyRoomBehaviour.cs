using UnityEngine;
using System.Collections;

public class EmptyRoomBehaviour : MonoBehaviour {

    
    // Use this for initialization
    void Start ()
    {
        // Dodawanie przeciwników
        
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(transform.childCount == 0)
        {
            GameObject clear = GameObject.FindWithTag("Clear").transform.GetChild(0).gameObject;
            clear.SetActive(true);
        }
	}
}
