using System;

[Serializable]
public class WorldSave {
    public int activeRoomX, activeRoomY;
    public int seed;
    public bool[,] visitedRooms;

    public WorldSave(int width, int height)
    {
        activeRoomX = activeRoomY = 0;
        seed = 0;
        visitedRooms = new bool[width, height];
    }
}
