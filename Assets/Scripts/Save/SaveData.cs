using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<ObjectData> objects;
    public PlayerStatus player;
    public InventoryData inventoryData;
    public EquipmentData[] equipments; // EquipmentDataの配列に変更
}
