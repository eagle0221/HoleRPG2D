using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDropUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;

    public void SetItem(EquipmentItem item)
    {
        itemIcon.sprite = item.itemIcon;
        itemNameText.text = item.itemName;
    }
}
