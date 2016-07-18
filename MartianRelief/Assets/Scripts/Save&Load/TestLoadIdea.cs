using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestLoadIdea : MonoBehaviour {

    public PrefabHolder prefabHolder;

    void OnLevelWasLoaded()
    {
        prefabHolder = GameObject.Find("PrefabHolder").GetComponent<PrefabHolder>();
        if (NewGameOrLoad.LoadName != null)
        {
            Load(NewGameOrLoad.LoadName);
        }
    }

    public void Load(string name)
    {
        Debug.Log("Rozpoczynam Load");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + name + ".dat", FileMode.Open);
        MultipleInfoSave allInfo = (MultipleInfoSave)bf.Deserialize(file);
        Debug.Log(Random.seed + "przed zmianą");
        Random.seed = allInfo.gameInfo.seed;
        Static.randomSeed = Random.seed;
        Debug.Log(Random.seed + "po zmianie");
        WorldGenerator generator = GameObject.Find("RoomManager").GetComponent<WorldGenerator>();
        generator.Generation();
        Debug.Log("Generacja z Load");
        Time.timeScale = 0;
        generator.actX = allInfo.gameInfo.actX;
        generator.actY = allInfo.gameInfo.actY;
        BasicStats player = GameObject.FindWithTag("Player").GetComponent<BasicStats>();
        player.damage = allInfo.playerInfo.damage;
        player.hp = allInfo.playerInfo.hp;
        player.maxHp = allInfo.playerInfo.maxHp;
        player.moveSpeed = allInfo.playerInfo.moveSpeed;
        player.invulnerabilityTime = allInfo.playerInfo.invulnerabilityTime;
        player.attackSpeed = allInfo.playerInfo.attackSpeed;
        player.GetComponent<Transform>().position = new Vector3(allInfo.playerInfo.transformSave.x, allInfo.playerInfo.transformSave.y, allInfo.playerInfo.transformSave.z);
        player.GetComponent<Transform>().eulerAngles = new Vector3(allInfo.playerInfo.transformSave.rotX, allInfo.playerInfo.transformSave.rotY, allInfo.playerInfo.transformSave.rotZ);
        player.GetComponent<Transform>().localScale = new Vector3(allInfo.playerInfo.transformSave.sizeX, allInfo.playerInfo.transformSave.sizeY, allInfo.playerInfo.transformSave.sizeZ);
        for (int i = 0; i < allInfo.gameInfo.wasVisited.GetLength(0); i++)
        {
            for(int j = 0; j < allInfo.gameInfo.wasVisited.GetLength(1); j++)
            {
                generator.rooms[i, j].wasVisited = allInfo.gameInfo.wasVisited[i, j];
            }
        }
        for (int i = 0; i < allInfo.gameInfo.wasVisited.GetLength(0); i++)
        {
            for (int j = 0; j < allInfo.gameInfo.wasVisited.GetLength(1); j++)
            {
                if(allInfo.gameInfo.wasVisited[i, j])
                {
                    generator.rooms[i, j].vecEnemies.Clear();
                    Image temp = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                    temp.rectTransform.SetParent(generator.minimapPanel.transform);
					float x, y;
					//temp.rectTransform.localPosition = new Vector3((-generator.minimapPanel.GetComponent<RectTransform>().sizeDelta.x / 2) + ((temp.rectTransform.sizeDelta.x + 1.5f) * generator.actX - i), (-generator.minimapPanel.GetComponent<RectTransform>().sizeDelta.y / 2) + ((temp.rectTransform.sizeDelta.y + 1.5f) * generator.actY - j), 0);
					x = -((temp.rectTransform.sizeDelta.x + 1.5f) * (generator.actX - i))-(generator.minimapPanel.GetComponent<RectTransform>().sizeDelta.x / 2);
					y = -((temp.rectTransform.sizeDelta.y + 1.5f) * (generator.actY - j))-(generator.minimapPanel.GetComponent<RectTransform>().sizeDelta.y / 2);
					temp.rectTransform.localPosition = new Vector3(x, y, 0);
					temp.rectTransform.localScale = Vector3.one;
                    if (i == generator.actX && j == generator.actY)
                        temp.color = Color.green;
                    else temp.color = Color.cyan;
                    generator.rooms[i, j].minimapImage = temp;
                    Debug.Log("Zaczynam ladowanie sasiadow na minimapie");
                    try
                    {
                        if (generator.rooms[i, j + 1].wasVisited == false && generator.rooms[i, j + 1].isGenerated && generator.rooms[i, j + 1].minimapImage == null)
                        {
                            Image tempU;
                            tempU = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                            generator.rooms[i, j + 1].minimapImage = tempU;
                            tempU.rectTransform.SetParent(generator.minimapPanel.transform);
                            tempU.rectTransform.localPosition = new Vector3(generator.rooms[i, j].minimapImage.rectTransform.localPosition.x, generator.rooms[i, j].minimapImage.rectTransform.localPosition.y + generator.rooms[i, j].minimapImage.rectTransform.sizeDelta.y + 1.5f, 0);
                            tempU.rectTransform.localScale = Vector3.one;
                            tempU.color = Color.gray;
                        }
                    }
                    catch { }
                    try
                    {
                        if (generator.rooms[i, j - 1].wasVisited == false && generator.rooms[i, j - 1].isGenerated && generator.rooms[i, j - 1].minimapImage == null)
                        {
                            Image tempD;
                            tempD = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                            generator.rooms[i, j - 1].minimapImage = tempD;
                            tempD.rectTransform.SetParent(generator.minimapPanel.transform);
                            tempD.rectTransform.localPosition = new Vector3(generator.rooms[i, j].minimapImage.rectTransform.localPosition.x, generator.rooms[i, j].minimapImage.rectTransform.localPosition.y - generator.rooms[i, j].minimapImage.rectTransform.sizeDelta.y - 1.5f, 0);
                            tempD.rectTransform.localScale = Vector3.one;
                            tempD.color = Color.gray;
                        }
                    }
                    catch { }
                    try
                    {
                        if (generator.rooms[i + 1, j].wasVisited == false && generator.rooms[i + 1, j].isGenerated && generator.rooms[i + 1, j].minimapImage == null)
                        {
                            Image tempR;
                            tempR = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                            generator.rooms[i + 1, j].minimapImage = tempR;
                            tempR.rectTransform.SetParent(generator.minimapPanel.transform);
                            tempR.rectTransform.localPosition = new Vector3(generator.rooms[i, j].minimapImage.rectTransform.localPosition.x + generator.rooms[i, j].minimapImage.rectTransform.sizeDelta.x + 1.5f, generator.rooms[i, j].minimapImage.rectTransform.localPosition.y, 0);
                            tempR.rectTransform.localScale = Vector3.one;
                            tempR.color = Color.gray;
                        }
                    }
                    catch { }
                    try
                    {
                        if (generator.rooms[i - 1, j].wasVisited == false && generator.rooms[i - 1, j].isGenerated && generator.rooms[i - 1, j].minimapImage == null)
                        {
                            Image tempL;
                            tempL = Instantiate(prefabHolder.minimapRoomImage).GetComponent<Image>();
                            generator.rooms[i - 1, j].minimapImage = tempL;
                            tempL.rectTransform.SetParent(generator.minimapPanel.transform);
                            tempL.rectTransform.localPosition = new Vector3(generator.rooms[i, j].minimapImage.rectTransform.localPosition.x - generator.rooms[i, j].minimapImage.rectTransform.sizeDelta.x - 1.5f, generator.rooms[i, j].minimapImage.rectTransform.localPosition.y, 0);
                            tempL.rectTransform.localScale = Vector3.one;
                            tempL.color = Color.gray;
                        }
                    }
                    catch { }

                }
            }
        }
		/*generator.rooms [generator.actX, generator.actY].vecItems = new List<Vector3> ();
		Debug.Log ("size: "+allInfo.gameInfo.itemsGameObjects.GetLength (0));
		for (int i = 0; i < allInfo.gameInfo.itemsGameObjects.GetLength (0); i++) {
			generator.rooms [generator.actX, generator.actY].vecItems.Add(new Vector3(
				allInfo.gameInfo.itemsGameObjects[i,0],
				allInfo.gameInfo.itemsGameObjects[i,1],
				allInfo.gameInfo.itemsGameObjects[i,2]
			));
		}*/
		for (int i = 0; i < allInfo.gameInfo.itemsGameObjects.GetLength (0); i++) {
			float x1 = allInfo.gameInfo.itemsGameObjects[i,0];
			float y1 = allInfo.gameInfo.itemsGameObjects[i,1];
			float x2 = allInfo.gameInfo.itemsGameObjects[i,2];
			float y2 = allInfo.gameInfo.itemsGameObjects[i,3];
			float id = allInfo.gameInfo.itemsGameObjects[i,4];
			generator.rooms[(int)x1,(int)y1].vecItems.Add (new Vector3(x2, y2, id));
		}

        Time.timeScale = 1;
        file.Close();
        Debug.Log("Zakończono ładowanie");
    }

}
