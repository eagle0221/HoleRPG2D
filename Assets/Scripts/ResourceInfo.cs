using UnityEngine;

[System.Serializable]
public class ResourceInfo
{
    public ulong money;
    public ulong chargeCrystal;

    // money のプロパティ
    public ulong Money
    {
        get { return money; }
        set { money = value; }
    }

    // chargeCrystal のプロパティ
    public ulong ChargeCrystal
    {
        get { return chargeCrystal; }
        set { chargeCrystal = value; }
    }
}
