using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;


public class TestSaveIdea : MonoBehaviour {

    public void Save(string name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + name + ".dat", FileMode.OpenOrCreate);
        WorldGenerator realGenerator = GameObject.Find("RoomManager").GetComponent<WorldGenerator>();
		Debug.Log ("Saving seed as " + Static.randomSeed);
        BasicGameInfoSave gameInfo = new BasicGameInfoSave(Static.randomSeed,realGenerator.actX,realGenerator.actY,
		     ArrayCreationVisited(realGenerator.rooms).value, 
		     ArrayCreationItemsGO(realGenerator.rooms).value);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerInfoSave playerInfo = new PlayerInfoSave(player.GetComponent<BasicStats>(), player.transform);
        MultipleInfoSave myInfo = new MultipleInfoSave(playerInfo, gameInfo, new DropSaver());
        Debug.Log("Save zakończony");
        bf.Serialize(file, myInfo);
        file.Close();
    }

	ArrayHolder<bool[,]> ArrayCreationVisited(Room[,] rooms){
        bool[,] table = new bool[rooms.GetLength(0), rooms.GetLength(1)];
        for(int i=0; i<rooms.GetLength(0); i++)
        {
            for(int j=0;j<rooms.GetLength(1); j++)
            {
                table[i, j] = rooms[i, j].wasVisited;
            }
        }

		ArrayHolder<bool[,]> myTable = new ArrayHolder<bool[,]>(table);
        return myTable;
    }

	ArrayHolder<float[,]> ArrayCreationItemsGO(Room[,] rooms){
		int ctr = 0;
		for (int i = 0; i < rooms.GetLength(0); i++) {
			for(int j = 0; j < rooms.GetLength (1); j++){
				ctr += rooms[i,j].vecItems.Count;
			}
		}


		float[,] table = new float[ctr, 5];      //5 --> {(x,y)[na mapie]}+{(x,y)[w pokoju]}+{id itemka} 
		int tmp = 0;
		for (int i = 0; i < rooms.GetLength (0); i++) {
			for(int j = 0; j < rooms.GetLength (1); j++){
				for(int k = 0; k < rooms[i,j].vecItems.Count; k++){
					table[tmp,0] = i;
					table[tmp,1] = j;
					table[tmp,2] = rooms[i,j].vecItems[k].x;
					table[tmp,3] = rooms[i,j].vecItems[k].y;
					table[tmp,4] = rooms[i,j].vecItems[k].z;
					tmp++;
				}
			}
		}
		ArrayHolder<float[,]> myTable = new ArrayHolder<float[,]>(table);
		return myTable;
	}
}

//DataType to typ tablicy, np bool[,]
public class ArrayHolder<DataType>
{
    public DataType value;

	public ArrayHolder(DataType myValues)
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
	public float[,] itemsGameObjects;
	public int[] itemsSpawned;
	public int[] itemsGlobal;

    public BasicGameInfoSave(int seeder, int activeX, int activeY, bool[,] visited, float[,] itemGO)
    {
        seed = seeder;
        actX = activeX;
        actY = activeY;
        wasVisited = visited;
		itemsGameObjects = itemGO;

		itemsSpawned = new int[Static.itemsSpawned.Count];
		for(int i = 0; i < Static.itemsSpawned.Count; i++){
			itemsSpawned[i] = Static.itemsSpawned[i];
		}
		itemsGlobal = new int[Static.itemPoolGlobal.Count];
		for(int i = 0; i < Static.itemPoolGlobal.Count; i++){
			itemsGlobal[i] = Static.itemPoolGlobal[i];
		}
    }
}

[Serializable]
public class DropSaver{
	public int dropCtr=0;
	public int[,] spawnedDropInts;
	public float[,] spawnedDropFloats;

	public DropSaver(){
		dropCtr = Static.actDrop;
		spawnedDropInts = new int[Static.spawnedDrops.Count,4];
		spawnedDropFloats = new float[Static.spawnedDrops.Count, 2];
		for(int i = 0; i < Static.spawnedDrops.Count; i++){
			spawnedDropFloats[i,0] = Static.spawnedDrops[i].x;
			spawnedDropFloats[i,1] = Static.spawnedDrops[i].y;
			spawnedDropInts[i,0] = Static.spawnedDrops[i].roomX;
			spawnedDropInts[i,1] = Static.spawnedDrops[i].roomY;
			spawnedDropInts[i,2] = Static.spawnedDrops[i].id;
			spawnedDropInts[i,3] = Static.spawnedDrops[i].spawnedID;
		}
	}

}

[Serializable]
public class PlayerInfoSave
{
    public int hp, maxHp;
    public Statistic damage, attackSpeed, moveSpeed;
    public float invulnerabilityTime;
    public TransformSave transformSave;
	public int cash, bombs;

    public PlayerInfoSave(BasicStats stats, Transform transform)
    {
        transformSave = new TransformSave(transform);
        hp = stats.hp;
        maxHp = stats.maxHp;
        damage = stats.damage;
        attackSpeed = stats.attackSpeed;
        moveSpeed = stats.moveSpeed;
        invulnerabilityTime = stats.invulnerabilityTime;
		cash = stats.cash;
		bombs = stats.bombs;
    }
}

[Serializable]
public class MultipleInfoSave
{
    public PlayerInfoSave playerInfo;
    public BasicGameInfoSave gameInfo;
	public DropSaver dropSaver;

    public MultipleInfoSave(PlayerInfoSave playInfo, BasicGameInfoSave gameplayInfo, DropSaver dropsy)
    {
        playerInfo = playInfo;
        gameInfo = gameplayInfo;
		dropSaver = dropsy;
    }
}
