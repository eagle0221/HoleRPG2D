using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonControll : MonoBehaviour
{
    private List<Button> parentButtons = new List<Button>();

    // 親のボタンを登録して非活性化
    public void RegisterParentButtons(List<Button> buttons)
    {
        parentButtons = buttons;

        foreach (Button btn in parentButtons)
        {
            btn.interactable = false; // ボタンを非活性化
        }
    }

    /// <summary>
    /// 閉じる押下処理
    /// </summary>
    public void CloseAction()
    {
        foreach (Button btn in parentButtons)
        {
            btn.interactable = true; // ボタンを再活性化
        }
    }

}
