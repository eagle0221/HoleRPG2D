using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentUI : MonoBehaviour
{
    public PlayerController player;
    public GameObject equipmentPanel;
    public Image[] equipmentSlots;
    public Button[] equipmentButtons; // Releaseボタンの配列を追加
    public GameObject itemButtonPrefab;
    public Transform itemListContent;
    public Button equipButton;
    public Button closeButton;
    public Button openEquipmentButton;
    public GameObject openEquipment;
    private EquipmentItem selectedItem;
    private EquipmentItem[] equipmentItems;

    void Start()
    {
        openEquipmentButton.onClick.AddListener(OpenEquipmentPanel);
        closeButton.onClick.AddListener(CloseEquipmentPanel);
        equipButton.onClick.AddListener(EquipItem);

        // Releaseボタンにリスナーを追加
        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            int slotIndex = i; // クロージャ対策
            equipmentButtons[i].onClick.AddListener(() => OnReleaseEquipmentButton(slotIndex));
        }
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
            TextMeshProUGUI buttonText = buttonObject.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>();
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
                equipmentSlots[i].color = new Color(1, 1, 1, 1); // スプライトを表示
                equipmentButtons[i].interactable = true; // 装備されているのでReleaseボタンを活性化
            }
            else
            {
                equipmentSlots[i].sprite = null;
                equipmentSlots[i].color = new Color(1, 1, 1, 0); // スプライトを非表示
                equipmentButtons[i].interactable = false; // 装備されていないのでReleaseボタンを非活性化
            }
        }
    }

    public void SelectItem(EquipmentItem item)
    {
        selectedItem = item;
    }

    public void OnReleaseEquipmentButton(int slotIndex)
    {
        if (player.equipments[slotIndex] != null)
        {
            // 装備解除処理を追加
            player.UnEquip(player.equipments[slotIndex]);
            // インベントリにアイテムを追加
            player.inventory.AddItem(player.equipments[slotIndex]);
            // 装備を解除
            player.equipments[slotIndex] = null;
            // UIを更新
            UpdateItemList();
            UpdateEquipmentSlots();
            // 装備解除によるステータス変更を反映
            player.UpdatePlayerStatus();
            player.UpdateStatusText();
        }
    }

    public void EquipItem()
    {
        if (selectedItem != null)
        {
            for (int i = 0; i < player.equipments.Length; i++)
            {
                if (player.equipments[i] == null)
                {
            player.Equip(selectedItem);
            player.inventory.RemoveItem(selectedItem);
            selectedItem = null;
            UpdateItemList();
            UpdateEquipmentSlots();
                    return;
                }
            }
            Debug.Log("装備スロットがいっぱいです");
        }
    }
}
