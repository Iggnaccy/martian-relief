using System;
using UnityEngine;

[Serializable]
public class RoomSave {
    public EnemySave[] myEnemies;
    public bool wasVisited;
    public int myX, myY;

    public RoomSave()
    {
        myEnemies = null;
        wasVisited = false;
        myX = myY = 0;
        Debug.Log("Tworzę RoomSave");
    }
}
