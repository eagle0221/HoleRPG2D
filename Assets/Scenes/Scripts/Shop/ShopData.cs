using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Data", menuName = "Shop Data")]
public class ShopData : ScriptableObject
{
    public List<ShopItemData> shopItems;
}
