using System;

[Serializable]
public class Statistic
{
    public float baseValue;
    public float bonusValue;
    public float mult;
    public float flatBonus;

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

    public static Statistic operator +(Statistic a, Statistic b)
    {
        return new Statistic(a.baseValue, a.bonusValue + b.bonusValue, a.mult * b.mult, a.flatBonus + b.flatBonus);
    }

    public float GetValue()
    {
        return (baseValue + bonusValue) * mult + flatBonus;
    }
}
