﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemIconPlaceholder : MonoBehaviour
{
    public GameObject myID;
	
    void Start()
    {
        GetComponent<Text>().text = myID.GetComponent<ItemBehaviour>().itemID.ToString();
		GetComponent<Text> ().color = Color.red;
		GetComponent<Text> ().fontSize = 100;
    }
}
