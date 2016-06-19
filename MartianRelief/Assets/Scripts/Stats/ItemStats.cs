using UnityEngine;
using System.Collections.Generic;

public class ItemStats : MonoBehaviour
{
	/*
		zawiera informacje o itemach gracza
	 */

    public List<Item> myItems;    	//itemy gracza
    public Item[] items;			//wszystkie istniejace itemy pula
    BasicStats myStats;				//statsy gracza

    void Awake()
    {
        myItems = new List<Item>();
        items = new Item[12];
        myStats = GetComponent<BasicStats>();
        for(int i = 0; i < 12; i++)
        {
            items[i] = new Item(i, GenerateItemStatsFromID(i, "damage"), GenerateItemStatsFromID(i, "attackSpeed"), GenerateItemStatsFromID(i, "moveSpeed"));
        }
    }

    public void PickUp(int itemID)
    {
        myItems.Add(items[itemID]);
        myStats.damage = myStats.damage + myItems[myItems.Count - 1].damage;
        myStats.attackSpeed = myStats.attackSpeed + myItems[myItems.Count - 1].attackSpeed;
        myStats.moveSpeed = myStats.moveSpeed + myItems[myItems.Count - 1].moveSpeed;
    }

    Statistic GenerateItemStatsFromID(int id, string name)
    {
        float bonusV, multV, flatBonus;
        bonusV = flatBonus = 0;
        multV = 1;
        if(name == "damage")
        {
            switch(id)
            {
                case 0:
                    {
                        bonusV = 0.37f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 1:
                    {
                        bonusV = 0.7f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 2:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 3:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 4:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 5:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 1f;
                        break;
                    }
                case 6:
                    {
                        bonusV = 0.52f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 7:
                    {
                        bonusV = 0.18f;
                        multV = 1.05f;
                        flatBonus = 0f;
                        break;
                    }
                case 8:
                    {
                        bonusV = -0.32f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 9:
                    {
                        bonusV = 1.5f;
                        multV = 1f;
                        flatBonus = -1.8f;
                        break;
                    }
                case 10:
                    {
                        bonusV = 0.2f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 11:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                default:
                    break;
            }
        }
        else if(name == "attackSpeed")
        {
            switch (id)
            {
                case 0:
                    {
                        bonusV = 0.2f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 1:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 2:
                    {
                        bonusV = 0f;
                        multV = 1.05f;
                        flatBonus = 0f;
                        break;
                    }
                case 3:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0.25f;
                        break;
                    }
                case 4:
                    {
                        bonusV = 0.17f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 5:
                    {
                        bonusV = 0f;
                        multV = 0.95f;
                        flatBonus = 0f;
                        break;
                    }
                case 6:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0.31f;
                        break;
                    }
                case 7:
                    {
                        bonusV = 0f;
                        multV = 0.98f;
                        flatBonus = 0f;
                        break;
                    }
                case 8:
                    {
                        bonusV = 0f;
                        multV = 1.1f;
                        flatBonus = 0f;
                        break;
                    }
                case 9:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 10:
                    {
                        bonusV = 0.2f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 11:
                    {
                        bonusV = 0.43f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                default:
                    break;
            }
        }
        else if(name == "moveSpeed")
        {
            switch (id)
            {
                case 0:
                    {
                        bonusV = 0.1f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 1:
                    {
                        bonusV = -0.07f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 2:
                    {
                        bonusV = 0f;
                        multV = 1.02f;
                        flatBonus = 0f;
                        break;
                    }
                case 3:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0.3f;
                        break;
                    }
                case 4:
                    {
                        bonusV = 0.04f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 5:
                    {
                        bonusV = 0.7f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 6:
                    {
                        bonusV = -0.19f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 7:
                    {
                        bonusV = 0f;
                        multV = 0.95f;
                        flatBonus = 0f;
                        break;
                    }
                case 8:
                    {
                        bonusV = 0.12f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 9:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 10:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                case 11:
                    {
                        bonusV = 0f;
                        multV = 1f;
                        flatBonus = 0f;
                        break;
                    }
                default:
                    break;
            }
        }
        return new Statistic(0f, bonusV, multV, flatBonus);
    }
}
