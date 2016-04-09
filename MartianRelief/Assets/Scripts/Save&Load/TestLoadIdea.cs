using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class TestLoadIdea : MonoBehaviour {

    public static GameObject loader;

    void Start()
    {
        if(loader == null)
        {
            loader = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if(loader != gameObject)
        {
            Destroy(gameObject);
        }
    }

    public void Load(string name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/martianRelief/" + name + ".dat", FileMode.Open);
        MultipleInfoSave allInfo = (MultipleInfoSave)bf.Deserialize(file);
        Random.seed = allInfo.gameInfo.seed;
        Application.LoadLevel("GameScene");
        WorldGenerator generator = GameObject.Find("RoomManager").GetComponent<WorldGenerator>();
        generator.actX = allInfo.gameInfo.actX;
        generator.actY = allInfo.gameInfo.actY;
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
                    temp.rectTransform.localPosition = new Vector3((-generator.minimapPanel.GetComponent<RectTransform>().sizeDelta.x / 2) + ((temp.rectTransform.sizeDelta.x + 1.5f) * generator.actX - i), (-generator.minimapPanel.GetComponent<RectTransform>().sizeDelta.y / 2) + ((temp.rectTransform.sizeDelta.y + 1.5f) * generator.actY - j), 0);
                    temp.rectTransform.localScale = Vector3.one;
                    if (i == generator.actX && j == generator.actY)
                        temp.color = Color.green;
                    else temp.color = Color.cyan;
                    
                }
            }
        }
        file.Close();
    }

}
