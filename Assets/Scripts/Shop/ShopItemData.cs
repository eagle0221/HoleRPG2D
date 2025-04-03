using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop Item")]
public class ShopItemData : ScriptableObject
{
    public EquipmentItem item;
    public ulong price;
}
