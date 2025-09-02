using System;
using Random = UnityEngine.Random;
public class Utils
{
    public static NormalItem.eNormalType GetRandomNormalType()
    {
        int random = Random.Range(0, Enum.GetValues(typeof(NormalItem.eNormalType)).Length);
        return (NormalItem.eNormalType)random;
    }

    public static NormalItem.eNormalType GetRandomNormalTypeExcept(NormalItem.eNormalType[] except)
    {
        NormalItem.eNormalType type;
        do
        {
            type = GetRandomNormalType();
        } while (Array.Exists(except, t => t == type));
        return type;
    }
}
