using System;

[Serializable]
public class CombinedSave {
    public RoomSave[] roomSave;
    public WorldSave worldSave;
    public PlayerSave playerSave;
    public MinimapElementSave[] minimapElementSave;

    /*public CombinedSave(int roomSaveSize, int worldSizeX, int worldSizeY, int minimapElementsCount)
    {
        roomSave = new RoomSave[roomSaveSize];
        worldSave = new WorldSave();
        worldSave.visitedRooms = new bool[worldSizeX, worldSizeY];
        playerSave = new PlayerSave();
        minimapElementSave = new MinimapElementSave[minimapElementsCount];
    }*/
    public CombinedSave(RoomSave[] roomSaver, WorldSave worldSaver, PlayerSave playerSaver, MinimapElementSave[] minimapElementSaver)
    {
        roomSave = roomSaver;
        worldSave = worldSaver;
        playerSave = playerSaver;
        minimapElementSave = minimapElementSaver;
    }
}
