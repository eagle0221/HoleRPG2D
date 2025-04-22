using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public List<EquipmentData> items = new List<EquipmentData>();

    public InventoryData()
    {
    }

    public InventoryData(Inventory inventory)
    {
        foreach (EquipmentItem item in inventory.items)
        {
            items.Add(new EquipmentData(item));
        }
    }
}
