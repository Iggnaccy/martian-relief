using System;

[Serializable]
public class WorldSave {
    public int activeRoomX, activeRoomY;
    public int seed;
    public bool[,] visitedRooms;
}
