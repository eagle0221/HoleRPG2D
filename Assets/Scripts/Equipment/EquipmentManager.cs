using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public EquipmentItem equippedItem;

    public void EquipItem(EquipmentItem item)
    {
        equippedItem = item;
    }
}
