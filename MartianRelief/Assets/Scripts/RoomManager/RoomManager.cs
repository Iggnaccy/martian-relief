using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

	//public Transform enemyPrefab;
	public Transform enemyHolder;
	public Transform doorsPrefab;
	public Transform doorsHolder;
    public Transform missileHolder;
    GameObject clear;
	public float minX, maxX, minY, maxY;

	enum RoomState{FIGHT,ROOM_CLEAR, LOAD_NEW_LEVEL};

	RoomState actualState = RoomState.LOAD_NEW_LEVEL;

	Room roomToLoad;

	void Start ()
    {
        clear = GameObject.FindWithTag("Clear").transform.GetChild(0).gameObject;
    }
	
	void Update ()
    {
		if (actualState == RoomState.LOAD_NEW_LEVEL)
        {
			actualState = RoomState.FIGHT;
            clearMissiles();
			spawnObjectsFromVectors();

            clear.SetActive(false);
		}
		else if (actualState == RoomState.FIGHT)
        {
			if (enemyHolder.transform.childCount == 0)
            {
                clear.SetActive(true);
				roomToLoad.vecEnemies.Clear();
				actualState = RoomState.ROOM_CLEAR;
			}
		}
        else if (actualState == RoomState.ROOM_CLEAR)
        {
			openTheGates();
		}
	}

	public void loadNewRoom(Room room)
    {
		roomToLoad = room;
		actualState = RoomState.LOAD_NEW_LEVEL;
	}

	void spawnObjectsFromVectors(){
		if (roomToLoad == null) {
			actualState = RoomState.LOAD_NEW_LEVEL;
			return;
		}
		clearDoors();
		roomToLoad.spawnEnemies ();
		roomToLoad.spawnDoors (GameObject.Find ("PrefabHolder").GetComponent<PrefabHolder>().testDoors);
	}

	void openTheGates()
    {
		for(int i = 0; i < doorsHolder.childCount; i++)
        {
			doorsHolder.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(0f,1f,0f);
		}
	}

	public void tryDoors(GameObject doors, int side)
    {
		if (actualState == RoomState.ROOM_CLEAR)
        {
			//actualState = RoomState.LOAD_NEW_LEVEL;
			if(side == 0){
				GetComponent<WorldGenerator>().goToRoom(-1, 0, side);
			}
			if(side == 1){
				GetComponent<WorldGenerator>().goToRoom(0, 1, side);
			}
			if(side == 2){
				GetComponent<WorldGenerator>().goToRoom(1, 0, side);
			}
			if(side == 3){
				GetComponent<WorldGenerator>().goToRoom(0, -1, side);
			}
		}
	}

    void clearMissiles()
    {
        foreach(Transform child in missileHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

	void clearDoors()
    {
		foreach(Transform child in GameObject.Find ("DoorsHolder").transform)
        {
            Destroy(child.gameObject);
		}
	}
}
