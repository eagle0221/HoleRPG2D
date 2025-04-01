using System.Collections.Generic;
using UnityEngine;

public class ItemDropUIController : MonoBehaviour
{
    public GameObject itemDropUIPrefab;
    public Transform itemDropUIContainer;
    public float displayDuration = 2f;
    public int maxDisplayedItems = 5;
    private List<GameObject> displayedItems = new List<GameObject>();

    public void ShowItemDrop(EquipmentItem item, Vector3 position)
    {
        // 表示中のアイテム数が上限を超えていたら、最も古いアイテムを削除
        if (displayedItems.Count >= maxDisplayedItems)
        {
            Destroy(displayedItems[0]);
            displayedItems.RemoveAt(0);
        }

        // アイテムドロップUIを生成
        if (itemDropUIPrefab == null)
        {
            Debug.LogError("itemDropUIPrefab is null!");
            return;
        }
        if (itemDropUIContainer == null)
        {
            Debug.LogError("itemDropUIContainer is null!");
            return;
        }
        GameObject itemDropUIObject = Instantiate(itemDropUIPrefab, itemDropUIContainer);
        if (itemDropUIObject == null)
        {
            Debug.LogError("itemDropUIObject is null!");
            return;
        }
        itemDropUIObject.transform.position = position;

        // アイテム情報を設定
        ItemDropUI itemDropUI = itemDropUIObject.GetComponent<ItemDropUI>();
        if (itemDropUI == null)
        {
            Debug.LogError("itemDropUI is null!");
            Destroy(itemDropUIObject);
            return;
        }
        itemDropUI.SetItem(item);

        // 表示リストに追加
        displayedItems.Add(itemDropUIObject);

        // 一定時間後に削除
        Destroy(itemDropUIObject, displayDuration);
        
        // 表示リストから削除
        StartCoroutine(RemoveItemFromList(itemDropUIObject, displayDuration));
    }

    private System.Collections.IEnumerator RemoveItemFromList(GameObject itemObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        displayedItems.Remove(itemObject);
    }
}