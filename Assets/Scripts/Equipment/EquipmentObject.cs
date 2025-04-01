using UnityEngine;

public class EquipmentObject : MonoBehaviour
{
    public EquipmentItem item;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemIcon;
    }

}
