using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<EquipmentItem> items = new List<EquipmentItem>();

    public void AddItem(EquipmentItem item)
    {
        items.Add(item);
    }

    public void RemoveItem(EquipmentItem item)
    {
        items.Remove(item);
    }
}
