using UnityEngine;
using System.Collections;

public class Item
{
    public int id;
    public Statistic damage;
    public Statistic attackSpeed;
    public Statistic moveSpeed;

    public Item(int ID, Statistic Damage, Statistic AttackSpeed, Statistic MoveSpeed)
    {
        id = ID;
        damage = Damage;
        attackSpeed = AttackSpeed;
        moveSpeed = MoveSpeed;
    }

    public Item(int ID)
    {
        id = ID;
        damage = new Statistic(0);
        attackSpeed = new Statistic(0);
        moveSpeed = new Statistic(0);
    }
}
