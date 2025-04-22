using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NotificationDetail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtNotificationTitle;
    [SerializeField] private TextMeshProUGUI txtNotificationDetail;
    [SerializeField] private Button btnClose;

    private Notification notificationDetail;
    private GameObject go;
    private List<Button> parentButtons = new List<Button>();

    private void Start()
    {
        NotificationDetailDisp();
        btnClose.onClick.AddListener(CloseAction);
    }

    /// <summary>
    /// お知らせ詳細表示
    /// </summary>
    public void NotificationDetailDisp()
    {
        Debug.Log("Start");
        txtNotificationTitle.text = notificationDetail.notificationTitle;
        txtNotificationDetail.text = notificationDetail.notification;
        Debug.Log("E n d");
    }

    public void SetNotificationDetail(Notification nd)
    {
        notificationDetail = nd;
    }

    /// <summary>
    /// 閉じる押下処理
    /// </summary>
    public void CloseAction()
    {
        Debug.Log("Start");
        SoundManager.instance.PlaySE(SELineup.SE_CANCEL);
        //DestroyChildren.GetChildren(gameObject, "areaMain"); // 画面を閉じる
        Destroy(gameObject);
        Debug.Log("E n d");
    }

}
