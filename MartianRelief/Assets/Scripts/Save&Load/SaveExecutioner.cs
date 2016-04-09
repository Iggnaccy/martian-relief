using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveExecutioner : MonoBehaviour{

    int sizeX, sizeY, roomCount, minimapElementsCount;
    WorldGenerator worldGenerator;
    BasicStats playerStats;

    void Start()
    {
        
    }

    public void Save(string saveFileName)
    {
        worldGenerator = GameObject.Find("RoomManager").GetComponent<WorldGenerator>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicStats>();
        sizeX = worldGenerator.width;
        sizeY = worldGenerator.height;
        roomCount = worldGenerator.roomCount;
        minimapElementsCount = worldGenerator.minimapPanel.transform.childCount;
        Debug.Log(minimapElementsCount + "elementów minimapy");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/martianRelief/" + saveFileName + ".dat",FileMode.OpenOrCreate);
        Debug.Log(Application.persistentDataPath + "/martianRelief/" + saveFileName + ".dat");
        PlayerSave playerSave = new PlayerSave();
        WorldSave worldSave = new WorldSave(sizeX, sizeY);
        RoomSave[] roomSave = new RoomSave[roomCount];
        MinimapElementSave[] minimapElementSave = new MinimapElementSave[minimapElementsCount];
        playerSave.hp = playerStats.hp;
        playerSave.maxHp = playerStats.maxHp;
        playerSave.movespeed = playerStats.moveSpeed;
        playerSave.attackspeed = playerStats.attackSpeed;
        playerSave.damage = playerStats.damage;
        playerSave.invulnerabilityTime = playerStats.invulnerabilityTime;
        playerSave.posX = playerStats.transform.position.x;
        playerSave.posY = playerStats.transform.position.y;
        playerSave.posZ = playerStats.transform.position.z;
        playerSave.scaleX = playerStats.transform.localScale.x;
        playerSave.scaleY = playerStats.transform.localScale.y;
        playerSave.scaleZ = playerStats.transform.localScale.z;
        worldSave.seed = Random.seed;
        worldSave.activeRoomX = worldGenerator.actX;
        worldSave.activeRoomY = worldGenerator.actY;
        int roomSaveNumber = 0;
        int minimapSaveNumber = 0;
        for (int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++)
            {
                if (worldGenerator.rooms[x, y].isGenerated)
                {
                    if (worldGenerator.rooms[x, y].minimapImage != null)
                    {
                        if (worldGenerator.rooms[x, y].minimapImage.tag == "MinimapRoom")
                        {
                            Debug.Log(minimapSaveNumber + " ustawienie obiektu minimapy");
                            minimapElementSave[minimapSaveNumber].x = x;
                            minimapElementSave[minimapSaveNumber].y = y;
                            minimapElementSave[minimapSaveNumber].posX = worldGenerator.rooms[x, y].minimapImage.rectTransform.localPosition.x;
                            minimapElementSave[minimapSaveNumber].posY = worldGenerator.rooms[x, y].minimapImage.rectTransform.localPosition.y;
                            minimapElementSave[minimapSaveNumber].posZ = worldGenerator.rooms[x, y].minimapImage.rectTransform.localPosition.z;
                            minimapElementSave[minimapSaveNumber].width = worldGenerator.rooms[x, y].minimapImage.rectTransform.sizeDelta.x;
                            minimapElementSave[minimapSaveNumber].height = worldGenerator.rooms[x, y].minimapImage.rectTransform.sizeDelta.y;
                            minimapSaveNumber++;
                        }
                    }
                    worldSave.visitedRooms[x, y] = worldGenerator.rooms[x, y].wasVisited;
                    roomSave[roomSaveNumber].myEnemies = new EnemySave[worldGenerator.rooms[x, y].vecEnemies.Count];
                    roomSave[roomSaveNumber].myX = x;
                    roomSave[roomSaveNumber].myY = y;
                    roomSave[roomSaveNumber].wasVisited = worldGenerator.rooms[x, y].wasVisited;
                    for(int i=0;i< roomSave[roomSaveNumber].myEnemies.Length;i++)
                    {
                        roomSave[roomSaveNumber].myEnemies[i].enemyID = (int)worldGenerator.rooms[x, y].vecEnemies[i].w;
                        roomSave[roomSaveNumber].myEnemies[i].x = worldGenerator.rooms[x, y].vecEnemies[i].x;
                        roomSave[roomSaveNumber].myEnemies[i].y = worldGenerator.rooms[x, y].vecEnemies[i].y;
                    }
                    roomSaveNumber++;
                    
                }
                
            }
        }
        CombinedSave data = new CombinedSave(roomSave, worldSave, playerSave, minimapElementSave);
        bf.Serialize(file, data);
        file.Close();
    }
}
