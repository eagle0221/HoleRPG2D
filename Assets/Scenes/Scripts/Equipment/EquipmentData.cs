using UnityEngine;

[System.Serializable]
public class EquipmentData
{
    public string itemName;
    public EquipmentItem.EquipmentType equipmentType;
    public float value;
    public string effectDescription;

    // コンストラクタを追加
    public EquipmentData(EquipmentItem item)
    {
        itemName = item.itemName;
        equipmentType = item.equipmentType;
        value = item.value;
        effectDescription = item.effectDescription;
    }
    public EquipmentData()
    {
    }
}
