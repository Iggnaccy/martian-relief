using UnityEngine;
using System.Collections.Generic;

public class ItemStats : MonoBehaviour
{
	/*
		zawiera informacje o itemach gracza
	 */

    public List<Item> myItems;    	//itemy gracza
    
    BasicStats myStats;				//statsy gracza

    void Awake()
    {
		myItems = new List<Item>();
		myStats = GetComponent<BasicStats>();
    }

    public void PickUp(int itemID)
    {
		Debug.Log ("pickup: " + Static.items[itemID].name);
		Static.items [itemID].print ();
        myItems.Add(Static.items[itemID]);
		//Debug.Log (myStats.damage.bonusValue + " && " + myItems[myItems.Count - 1].damage.bonusValue);
		//Debug.Log (myStats.damage.GetValue () + "val");
		//Debug.Log (myStats.damage.mult + " && " + myItems[myItems.Count - 1].damage.mult);
        myStats.damage = myStats.damage + myItems[myItems.Count - 1].damage;
        myStats.attackSpeed = myStats.attackSpeed + myItems[myItems.Count - 1].attackSpeed;
        myStats.moveSpeed = myStats.moveSpeed + myItems[myItems.Count - 1].moveSpeed;
    }
}
