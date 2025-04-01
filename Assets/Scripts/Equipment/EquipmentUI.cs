using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentUI : MonoBehaviour
{
    public PlayerController player;
    public Image[] equipmentSlots;
    public Button[] equipmentButtons; // Releaseボタンの配列を追加
    public GameObject itemButtonPrefab;
    public Transform itemListContent;
    private EquipmentItem selectedItem;
    public TextMeshProUGUI[] equipmentsName = new TextMeshProUGUI[2]; // 装備スロット（2つ）
    public TextMeshProUGUI[] equipmentsText = new TextMeshProUGUI[2]; // 装備スロット（2つ）

    void Start()
    {
        // Releaseボタンにリスナーを追加
        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            int slotIndex = i; // クロージャ対策
            equipmentButtons[i].onClick.AddListener(() => OnReleaseEquipmentButton(slotIndex));
        }
        // インベントリの変更イベントを購読
        player.inventory.OnInventoryChanged += UpdateItemList;
    }

    void OnEnable()
    {
        UpdateItemList();
        UpdateEquipmentSlots();
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
                // 装備名と効果の説明をUIに表示
                equipmentsName[i].text = player.equipments[i].itemName;
                equipmentsText[i].text = player.equipments[i].effectDescription;
            }
            else
            {
                equipmentSlots[i].sprite = null;
                equipmentSlots[i].color = new Color(1, 1, 1, 0); // スプライトを非表示
                equipmentButtons[i].interactable = false; // 装備されていないのでReleaseボタンを非活性化
                // 装備名と効果の説明をUIから削除
                equipmentsName[i].text = "";
                equipmentsText[i].text = "";
            }
        }
    }

    public void SelectItem(EquipmentItem item)
    {
        selectedItem = item;
        EquipItem();
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
        bool childRet = false;
        if (selectedItem != null)
        {
            for (int i = 0; i < player.equipments.Length; i++)
            {
                if (player.equipments[i] == null)
                {
                    childRet = player.Equip(selectedItem);
                    if(childRet)  // 装備成功
                    {
                        // インベントリからアイテムを削除
                        player.inventory.RemoveItem(selectedItem);
                    }
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
