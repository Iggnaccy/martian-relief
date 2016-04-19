using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;


public class TestSaveIdea : MonoBehaviour {

    public void Save(string name)
    {
        Time.timeScale = 0;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + name + ".dat", FileMode.OpenOrCreate);
        WorldGenerator realGenerator = GameObject.Find("RoomManager").GetComponent<WorldGenerator>();
		//Debug.Log("Seed przed save " + UnityEngine.Random.seed);
        BasicGameInfoSave gameInfo = new BasicGameInfoSave(Static.randomSeed,realGenerator.actX,realGenerator.actY, TableCreation(realGenerator.rooms).value);
        //Debug.Log(UnityEngine.Random.seed + "Po save, unity" + gameInfo.seed + "Po save, save");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerInfoSave playerInfo = new PlayerInfoSave(player.GetComponent<BasicStats>(), player.transform);
        MultipleInfoSave myInfo = new MultipleInfoSave(playerInfo, gameInfo);
        Debug.Log("Save zakończony");
        bf.Serialize(file, myInfo);
        Time.timeScale = 1;
        file.Close();
    }

    TableHolder TableCreation(Room[,] rooms)
    {
        bool[,] table = new bool[rooms.GetLength(0), rooms.GetLength(1)];
        for(int i=0; i<rooms.GetLength(0); i++)
        {
            for(int j=0;j<rooms.GetLength(1); j++)
            {
                table[i, j] = rooms[i, j].wasVisited;
            }
        }

        TableHolder myTable = new TableHolder(table);
        return myTable;
    }
}

public class TableHolder
{
    public bool[,] value;

    public TableHolder(bool[,] myValues)
    {
        value = myValues;
    }
}

[Serializable]
public class TransformSave
{
    public float x, y, z;
    public float rotX, rotY, rotZ;
    public float sizeX, sizeY, sizeZ;
    public TransformSave(Transform saveMe)
    {
        x = saveMe.position.x;
        y = saveMe.position.y;
        z = saveMe.position.z;
        rotX = saveMe.rotation.x;
        rotY = saveMe.rotation.y;
        rotZ = saveMe.rotation.z;
        sizeX = saveMe.localScale.x;
        sizeY = saveMe.localScale.y;
        sizeZ = saveMe.localScale.z;
    }
}

[Serializable]
public class BasicGameInfoSave
{
    public int seed;
    public int actX, actY;
    public bool[,] wasVisited;

    public BasicGameInfoSave(int seeder, int activeX, int activeY, bool[,] visited)
    {
        seed = seeder;
        actX = activeX;
        actY = activeY;
        wasVisited = visited;
    }
}

[Serializable]
public class PlayerInfoSave
{
    public int hp, maxHp;
    public float damage, attackSpeed, moveSpeed, invulnerabilityTime;
    public TransformSave transformSave;

    public PlayerInfoSave(BasicStats stats, Transform transform)
    {
        transformSave = new TransformSave(transform);
        hp = stats.hp;
        maxHp = stats.maxHp;
        damage = stats.damage;
        attackSpeed = stats.attackSpeed;
        moveSpeed = stats.moveSpeed;
        invulnerabilityTime = stats.invulnerabilityTime;
    }
}

[Serializable]
public class MultipleInfoSave
{
    public PlayerInfoSave playerInfo;
    public BasicGameInfoSave gameInfo;

    public MultipleInfoSave(PlayerInfoSave playInfo, BasicGameInfoSave gameplayInfo)
    {
        playerInfo = playInfo;
        gameInfo = gameplayInfo;
    }
}
