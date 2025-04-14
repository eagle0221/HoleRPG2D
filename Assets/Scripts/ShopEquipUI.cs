using UnityEngine;
using UnityEngine.UI;

public class ShopEquipUI : MonoBehaviour
{

    [SerializeField]private GameObject areaEquipShop;

    // プレイヤーが入った時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            areaEquipShop.SetActive(true);
        }
    }

    // プレイヤーが出た時の処理
    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
