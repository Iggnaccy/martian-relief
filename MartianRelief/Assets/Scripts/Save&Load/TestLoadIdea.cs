using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class TestLoadIdea : MonoBehaviour {

    void OnLevelWasLoaded()
    {
        Debug.Log(NewGameOrLoad.LoadName + "Debug z Loada");
        if (NewGameOrLoad.LoadName != null)
        {
            Debug.Log("Wchodzę w Load");
            Load(NewGameOrLoad.LoadName);
        }
        else Debug.Log("Nie wchodzę w Load");        //nowa gra
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
                    Image temp = Instantiate(generator.rooms[i, j].minimapImage) as Image;
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
                }
            }
        }
        Time.timeScale = 1;
        file.Close();
        Debug.Log("Zakończono ładowanie");
    }

}
