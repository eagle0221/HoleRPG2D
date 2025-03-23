using UnityEngine;

public class EquipmentObject : MonoBehaviour
{
    public EquipmentItem item;
    public Sprite itemSprite;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemIcon;
    }

}
