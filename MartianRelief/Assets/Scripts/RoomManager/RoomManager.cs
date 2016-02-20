using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviour
{

	public Transform enemyPrefab;
	public Transform enemyHolder;
	public Transform doorsPrefab;
	public Transform doorsHolder;
    public Transform missileHolder;
    GameObject clear;
	public float minX, maxX, minY, maxY;
	float padding = 0.1f;

	enum RoomState{FIGHT,ROOM_CLEAR, LOAD_NEW_LEVEL};

	RoomState actualState = RoomState.LOAD_NEW_LEVEL;

	void Start ()
    {
        //spawnDoors (2);
        //spawnEnemies (5);
        clear = GameObject.FindWithTag("Clear").transform.GetChild(0).gameObject;
    }
	
	void Update ()
    {
		if (actualState == RoomState.LOAD_NEW_LEVEL)
        {
            clearMissiles();
			spawnDoors (2);
			spawnEnemies (5);
			actualState = RoomState.FIGHT;
            clear.SetActive(false);
		}
		else if (actualState == RoomState.FIGHT)
        {
			if (enemyHolder.transform.childCount == 0)
            {
                clear.SetActive(true);
				actualState = RoomState.ROOM_CLEAR;
			}
		}
        else if (actualState == RoomState.ROOM_CLEAR)
        {
			openTheGates();
		}
	}

	void loadNewLevel()
    {
	
	}

	void openTheGates()
    {
		for(int i = 0; i < doorsHolder.childCount; i++)
        {
			doorsHolder.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(0f,1f,0f);
		}
	}

	public void tryDoors(GameObject doors)
    {
		if (actualState == RoomState.ROOM_CLEAR)
        {
			actualState = RoomState.LOAD_NEW_LEVEL;
		}
	}

    void clearMissiles()
    {
        foreach(Transform child in missileHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

	void spawnEnemies(int numb)
    {
		for (int i = 0; i < numb; i++)
        {
			//GameObject go = Instantiate(enemyPrefab, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), Quaternion.identity) as GameObject;
			//Debug.Log ("asdf: " + go.name);
			Vector3 position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
			(Instantiate (enemyPrefab.gameObject, position, Quaternion.identity) as GameObject).transform.parent = enemyHolder.transform;
		}
	}

	void spawnDoors(int counter)
    {
		foreach(Transform child in doorsHolder.transform)
        {
            Destroy(child.gameObject);
		}

		for (int i = 0; i < counter; i++)
        {
			int side = (int)Mathf.Ceil(Random.Range(0, 4));     //1->lewo, 2->góra, 3->prawo, 4->dół
			float randX = Random.Range (minX, maxX);
			float randY = Random.Range (minY, maxY);
			if(side <= 1){randX=minX+padding;}
			if(side == 2){randY=minY+padding;}
			if(side == 3){randX=maxX-padding;}
			if(side >= 4){randY=maxY-padding;}
			Vector3 position = new Vector3(randX, randY, 0);
			(Instantiate (doorsPrefab.gameObject, position, Quaternion.identity) as GameObject).transform.parent = doorsHolder.transform;
		}
	}
}
