using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveExecutioner : MonoBehaviour{

    int sizeX, sizeY, roomCount, minimapElementsCount;
    WorldGenerator worldGenerator;
    BasicStats playerStats;

    void Start()
    {
        worldGenerator = GameObject.Find("RoomManager").GetComponent<WorldGenerator>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicStats>();
        sizeX = worldGenerator.width;
        sizeY = worldGenerator.height;
        roomCount = worldGenerator.roomCount;
        minimapElementsCount = worldGenerator.minimapPanel.transform.childCount;
    }

    public void Save(string saveFileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/martianRelief/" + saveFileName + ".dat");
        Debug.Log(Application.persistentDataPath + "/martianRelief/" + saveFileName + ".dat");
        CombinedSave data = new CombinedSave(roomCount, sizeX, sizeY, minimapElementsCount);
        data.playerSave.hp = playerStats.hp;
        data.playerSave.maxHp = playerStats.maxHp;
        data.playerSave.movespeed = playerStats.moveSpeed;
        data.playerSave.attackspeed = playerStats.attackSpeed;
        data.playerSave.damage = playerStats.damage;
        data.playerSave.invoulnerabilityTime = playerStats.invoulnerabilityTime;
        data.playerSave.posX = playerStats.transform.position.x;
        data.playerSave.posY = playerStats.transform.position.y;
        data.playerSave.posZ = playerStats.transform.position.z;
        data.playerSave.scaleX = playerStats.transform.localScale.x;
        data.playerSave.scaleY = playerStats.transform.localScale.y;
        data.playerSave.scaleZ = playerStats.transform.localScale.z;
        data.worldSave.seed = Random.seed;
        data.worldSave.activeRoomX = worldGenerator.actX;
        data.worldSave.activeRoomY = worldGenerator.actY;
        int roomSaveNumber = 0;
        int minimapSaveNumber = 0;
        for (int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++)
            {
                if (worldGenerator.rooms[x, y].isGenerated)
                {
                    data.worldSave.visitedRooms[x, y] = worldGenerator.rooms[x, y].wasVisited;
                    data.roomSave[roomSaveNumber].myX = x;
                    data.roomSave[roomSaveNumber].myY = y;
                    data.roomSave[roomSaveNumber].wasVisited = worldGenerator.rooms[x, y].wasVisited;
                    data.roomSave[roomSaveNumber].myEnemies = new EnemySave[worldGenerator.rooms[x, y].maxiEnemies];
                    for(int i=0;i< data.roomSave[roomSaveNumber].myEnemies.Length;i++)
                    {
                        data.roomSave[roomSaveNumber].myEnemies[i].enemyID = (int)worldGenerator.rooms[x, y].vecEnemies[i].w;
                        data.roomSave[roomSaveNumber].myEnemies[i].x = worldGenerator.rooms[x, y].vecEnemies[i].x;
                        data.roomSave[roomSaveNumber].myEnemies[i].y = worldGenerator.rooms[x, y].vecEnemies[i].y;
                    }
                    roomSaveNumber++;
                }
                if(worldGenerator.rooms[x, y].minimapImage != null)
                {
                    data.minimapElementSave[minimapSaveNumber].x = x;
                    data.minimapElementSave[minimapSaveNumber].y = y;
                    data.minimapElementSave[minimapSaveNumber].posX = worldGenerator.rooms[x, y].minimapImage.rectTransform.localPosition.x;
                    data.minimapElementSave[minimapSaveNumber].posY = worldGenerator.rooms[x, y].minimapImage.rectTransform.localPosition.y;
                    data.minimapElementSave[minimapSaveNumber].posZ = worldGenerator.rooms[x, y].minimapImage.rectTransform.localPosition.z;
                    data.minimapElementSave[minimapSaveNumber].width = worldGenerator.rooms[x, y].minimapImage.rectTransform.sizeDelta.x;
                    data.minimapElementSave[minimapSaveNumber].height = worldGenerator.rooms[x, y].minimapImage.rectTransform.sizeDelta.y;
                    minimapSaveNumber++;
                }
            }
        }
        bf.Serialize(file, data);
    }
}
