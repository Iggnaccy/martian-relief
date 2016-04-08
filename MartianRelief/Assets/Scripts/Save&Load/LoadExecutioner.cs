using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadExecutioner : MonoBehaviour {

    GameObject loader;


    void Start()
    {
        if (loader == null)
        {
            DontDestroyOnLoad(gameObject);
            loader = gameObject;
        }
        else if (loader != this) Destroy(gameObject);
    }

    public void Load(string saveFileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/martianrelief/" + saveFileName + ".dat", FileMode.Open);
        CombinedSave data = (CombinedSave)bf.Deserialize(file);
        Application.LoadLevel("GameScene");
    }
}
