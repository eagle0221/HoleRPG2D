using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class NotificationWindow : MonoBehaviour
{
    private readonly string JSON_NOTIFICATION = Application.dataPath + "/Scenes/Data/Notification.json";
    [SerializeField] private GameObject areaNotificationOne;
    [SerializeField] private Button btnClose;

    private NotificationList notificationList;
    private GameObject go;
    private Button[] buttons;
    private List<Button> parentButtons = new List<Button>();

    private void Start()
    {
        NotificationTitleList();
        btnClose.onClick.AddListener(CloseAction);
    }

    /// <summary>
    /// お知らせ一覧表示処理
    /// </summary>
    public void NotificationTitleList()
    {
        Debug.Log("Start");
        string jsonData = "";
        StreamReader streamReader;

        // Jsonデータ取得
        streamReader = new StreamReader(JSON_NOTIFICATION, false);
        jsonData = streamReader.ReadToEnd();
        streamReader.Close();
        notificationList = JsonUtility.FromJson<NotificationList>(jsonData);

        // 販売スキルを全表示する
        foreach (Notification n in notificationList.list.OrderByDescending(n => n.notificationNo))
        {
            go = Instantiate(areaNotificationOne, GameObject.Find("areaNotificationContent").transform, false);
            go.GetComponent<NotificationOne>().NotificationDisp(n);
        }

        Debug.Log("E n d");
    }


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
        Debug.Log("Start");
        SoundManager.instance.PlaySE(SELineup.SE_CANCEL);
        foreach (Button btn in parentButtons)
        {
            btn.interactable = true; // ボタンを再活性化
        }
        //DestroyChildren.GetChildren(gameObject, "areaMain"); // 画面を閉じる
        Destroy(gameObject);
        Debug.Log("E n d");
    }

}
