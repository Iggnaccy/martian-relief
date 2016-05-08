using System;

[Serializable]
public class Statistic
{
    float baseValue;
    float bonusValue;
    float mult;
    float flatBonus;

    public Statistic(float baseV, float bonusV, float multV, float flatV)
    {
        baseValue = baseV;
        bonusValue = bonusV;
        mult = multV;
        flatBonus = flatV;
    }

    public Statistic(float baseV)
    {
        baseValue = baseV;
        bonusValue = flatBonus = 0;
        mult = 1.0f;
    }

    public float GetValue()
    {
        return (baseValue + bonusValue) * mult + flatBonus;
    }
}
