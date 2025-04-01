using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<EquipmentItem> items = new List<EquipmentItem>();

    // インベントリが変更されたことを通知するイベント
    public event Action OnInventoryChanged;

    public void AddItem(EquipmentItem item)
    {
        items.Add(item);
        // インベントリが変更されたことを通知
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(EquipmentItem item)
    {
        items.Remove(item);
        // インベントリが変更されたことを通知
        OnInventoryChanged?.Invoke();
    }
}
