using UnityEngine;
using UnityEngine.UI;

public class EquipmentObject : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    public EquipmentItem item;

    void Start()
    {
        itemIcon.sprite = item.itemIcon;
    }

}
