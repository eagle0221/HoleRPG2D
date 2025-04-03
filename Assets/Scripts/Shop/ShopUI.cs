using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public PlayerController player;
    public ShopData shopData;
    public GameObject shopPanel;
    public GameObject shopItemButtonPrefab;
    public Transform shopItemListContent;
    public Button closeButton;
    public GameObject openShop;
    public Button openButton;

    void Start()
    {
        closeButton.onClick.AddListener(CloseShopPanel);
    }

    public void OpenShopPanel()
    {
        openShop.SetActive(false);
        shopPanel.SetActive(true);
        UpdateShopItemList();
    }

    public void CloseShopPanel()
    {
        openShop.SetActive(true);
        shopPanel.SetActive(false);
    }

    public void UpdateShopItemList()
    {
        // 現在のアイテムリストをクリア
        foreach (Transform child in shopItemListContent)
        {
            Destroy(child.gameObject);
        }

        // ショップ内のアイテムをリストに追加
        foreach (ShopItemData shopItem in shopData.shopItems)
        {
            GameObject buttonObject = Instantiate(shopItemButtonPrefab, shopItemListContent);
            Button button = buttonObject.GetComponentInChildren<Button>();
            TextMeshProUGUI buttonText = buttonObject.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = buttonObject.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
            buttonText.text = shopItem.item.itemName;
            priceText.text = shopItem.price.ToString();
            button.onClick.AddListener(() => BuyItem(shopItem));
        }
    }

    public void BuyItem(ShopItemData shopItem)
    {
        if (GameManager.Instance.resourceInfo.Money >= shopItem.price)
        {
            GameManager.Instance.resourceInfo.Money -= shopItem.price;
            player.inventory.AddItem(shopItem.item);
        }
        else
        {
            Debug.Log("お金が足りません");
        }
    }
}
