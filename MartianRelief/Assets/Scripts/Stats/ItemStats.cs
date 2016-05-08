using UnityEngine;
using System.Collections.Generic;

public class ItemStats : MonoBehaviour
{
    public List<Item> myItems;
    public Item[] items;

    void Awake()
    {
        myItems = new List<Item>();
        items = new Item[150];
        for(int i = 0; i < 150; i++)
        {
            items[i] = new Item(i);
        }
    }

    public void PickUp(int itemID)
    {
        myItems.Add(items[itemID]);
    }

}
