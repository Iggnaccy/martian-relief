using System;

[Serializable]
public class PlayerSave {
    public float posX, posY, posZ;
    public float scaleX, scaleY, scaleZ;
    public int hp, maxHp;
    public float movespeed, attackspeed, damage, invulnerabilityTime;

    public PlayerSave()
    {
        posX = posY = posZ = 0;
        scaleX = scaleY = scaleZ = 1;
        hp = maxHp = 1;
        movespeed = 250;
        attackspeed = damage = 4;
        invulnerabilityTime = 1;
    }
}
