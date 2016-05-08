﻿using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour
{
    public int itemID;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            ItemStats pickUpPlayer = other.GetComponent<ItemStats>();
            pickUpPlayer.PickUp(itemID);
           /* Debug.Log("Podnoszę");*/
            Destroy(gameObject);
        }
    }
}
