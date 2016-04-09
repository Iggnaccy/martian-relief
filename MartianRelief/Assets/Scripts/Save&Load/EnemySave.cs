using System;

[Serializable]
public class EnemySave {
    public int enemyID;
    public float x, y;

    public EnemySave()
    {
        x = y = 0;
        enemyID = 0;
    }
}
