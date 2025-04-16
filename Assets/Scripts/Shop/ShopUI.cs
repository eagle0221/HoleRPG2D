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
    public TextMeshProUGUI messageText;

    void OnEnable()
    {
        if(gameObject.activeInHierarchy)
        {
            OpenShopPanel();
            messageText.text = "";
        }
    }

    void Start()
    {
        closeButton.onClick.AddListener(CloseShopPanel);
    }

    public void OpenShopPanel()
    {
        UpdateShopItemList();
    }

    public void CloseShopPanel()
    {
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
            messageText.text = shopItem.item.itemName + "を購入しました";
            Debug.Log(shopItem.item.itemName + "を購入しました");
        }
        else
        {
            messageText.text = "お金が足りません";
            Debug.Log("お金が足りません");
        }
    }
}
