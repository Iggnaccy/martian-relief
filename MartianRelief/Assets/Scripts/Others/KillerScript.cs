﻿using UnityEngine;
using System.Collections;

public class KillerScript : MonoBehaviour {
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
        }
        
    }
}
