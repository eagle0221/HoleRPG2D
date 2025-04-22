using UnityEngine;

public class ButtonHoldDown : MonoBehaviour
{
    // ボタンを押したときtrue、離したときfalseになるフラグ
    private bool buttonDownFlag = false;
    private void Update()
    {
        if (buttonDownFlag)
        {
            // ボタンが押しっぱなしの状態の時にのみ「Hold」を表示する。
            //Log.LogOutput("Hold");
        }
    }
    // ボタンを押したときの処理
    public void OnButtonDown()
    {
        Debug.Log("Down");
        buttonDownFlag = true;
    }
    // ボタンを離したときの処理
    public void OnButtonUp()
    {
        Debug.Log("Up");
        buttonDownFlag = false;
    }
    // 
    public void OnButtonPointerEnter()
    {
        Debug.Log("OnButtonPointerEnter");
    }
    // 
    public void OnButtonPointerExit()
    {
        Debug.Log("OnButtonPointerExit");
    }
    // 
    public void OnButtonExit()
    {
        Debug.Log("OnButtonExit");
    }
    // 
    public void OnButtonPointerClick()
    {
        Debug.Log("OnButtonPointerClick");
    }
    // 
    public void OnButtonDrag()
    {
        Debug.Log("OnButtonDrag");
    }
    // 
    public void OnButtonDrop()
    {
        Debug.Log("OnButtonDrop");
    }
    // 
    public void OnButtonScroll()
    {
        Debug.Log("OnButtonScroll");
    }
    // 
    public void OnButtonUpdateSelected()
    {
        //Debug.Log("OnButtonUpdateSelected");
    }
    // 
    public void OnButtonSelect()
    {
        Debug.Log("OnButtonSelect");
    }
    // 
    public void OnButtonDeselect()
    {
        Debug.Log("OnButtonDeselect");
    }
    // 
    public void OnButtonMove()
    {
        Debug.Log("OnButtonMove");
    }
    // 
    public void OnButtonInitializePotentialDrag()
    {
        Debug.Log("OnButtonInitializePotentialDrag");
    }
    // 
    public void OnButtonBeginDrag()
    {
        Debug.Log("OnButtonBeginDrag");
    }
    // 
    public void OnButtonEndDrag()
    {
        Debug.Log("OnButtonEndDrag");
    }
    // 
    public void OnButtonSubmit()
    {
        Debug.Log("OnButtonSubmit");
    }
    // 
    public void OnButtonCancel()
    {
        Debug.Log("OnButtonCancel");
    }
}