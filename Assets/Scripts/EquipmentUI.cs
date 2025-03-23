using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentUI : MonoBehaviour
{
    public PlayerController player;
    public GameObject equipmentPanel;
    public Image[] equipmentSlots;
    public GameObject itemButtonPrefab;
    public Transform itemListContent;
    public Button equipButton;
    public Button closeButton;
    public Button openEquipmentButton;
    public GameObject openEquipment;
    private EquipmentItem selectedItem;

    void Start()
    {
        openEquipmentButton.onClick.AddListener(OpenEquipmentPanel);
        closeButton.onClick.AddListener(CloseEquipmentPanel);
        equipButton.onClick.AddListener(EquipItem);
    }

    public void OpenEquipmentPanel()
    {
        openEquipment.SetActive(false);
        equipmentPanel.SetActive(true);
        UpdateItemList();
        UpdateEquipmentSlots();
    }

    public void CloseEquipmentPanel()
    {
        equipmentPanel.SetActive(false);
        openEquipment.SetActive(true);
    }

    public void UpdateItemList()
    {
        // 現在のアイテムリストをクリア
        foreach (Transform child in itemListContent)
        {
            Destroy(child.gameObject);
        }

        // インベントリ内のアイテムをリストに追加
        foreach (EquipmentItem item in player.inventory.items)
        {
            GameObject buttonObject = Instantiate(itemButtonPrefab, itemListContent);
            Button button = buttonObject.GetComponentInChildren<Button>();
            //TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>(); // 変更前
            TextMeshProUGUI buttonText = buttonObject.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>(); // 変更後
            buttonText.text = item.itemName;
            button.onClick.AddListener(() => SelectItem(item));
        }
    }

    public void UpdateEquipmentSlots()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (player.equipments[i] != null)
            {
                equipmentSlots[i].sprite = player.equipments[i].itemIcon;
            }
            else
            {
                equipmentSlots[i].sprite = null;
            }
        }
    }

    public void SelectItem(EquipmentItem item)
    {
        selectedItem = item;
    }

    public void EquipItem()
    {
        if (selectedItem != null)
        {
            player.Equip(selectedItem);
            player.inventory.RemoveItem(selectedItem);
            selectedItem = null;
            UpdateItemList();
            UpdateEquipmentSlots();
        }
    }
}
