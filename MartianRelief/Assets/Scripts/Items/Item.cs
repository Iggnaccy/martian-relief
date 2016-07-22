using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Item 
{
	public string name;
    public int id;
    public Statistic damage;
    public Statistic attackSpeed;
    public Statistic moveSpeed;
	public List<int> roomTypes;
	public string iconFile;

	public Item(){
		name = "unknown";
		id = -1;
		damage = new Statistic (0, 0, 0, 0);
		attackSpeed = new Statistic (0, 0, 0, 0);
		moveSpeed = new Statistic (0, 0, 0, 0);
		roomTypes = new List<int>();
		iconFile = "unknown.png";
	}

    public Item(int ID, string Name, Statistic Damage, Statistic AttackSpeed, Statistic MoveSpeed, List<int> RoomTypes, string IconFile)
    {
        id = ID;
		name = Name;
        damage = Damage;
        attackSpeed = AttackSpeed;
        moveSpeed = MoveSpeed;
		roomTypes = RoomTypes;
		IconFile = IconFile;
    }

    public Item(int ID)
    {
        id = Static.items[ID].id;
		name = Static.items[ID].name;
		damage = Static.items[ID].damage;
		attackSpeed = Static.items[ID].attackSpeed;
		moveSpeed = Static.items[ID].moveSpeed;
    }

	public void print(){
		string str = "";
		foreach (int rt in roomTypes) {
			str+=rt.ToString ();
			str+=",";
		} 
		/*Debug.Log ("[item(" + id + ")]=> name=" + name + ",damage=" + damage.GetValue()
		           + ",attackSpeed=" + attackSpeed.GetValue () + ",moveSpeed=" + moveSpeed.GetValue ()
		           + ",icon="+iconFile+",roomTypes="+str);*/
	}


	//deep copy -> potrzebne w Static.cs
	public Item deepClone(Item a)
	{
		using (MemoryStream stream = new MemoryStream())
		{
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, a);
			stream.Position = 0;
			return (Item) formatter.Deserialize(stream);
		}
	}
}
