using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationOne : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtNotificationTitle;
    [SerializeField] private Button btnNotificationTitle;
    [SerializeField] private GameObject areaNotificationDetail;
    private Notification notification;
    private Button[] buttons;
    GameObject go;

    private void Start()
    {
        btnNotificationTitle.onClick.AddListener(OnClickNotificationTitle);
    }

    public void OnClickNotificationTitle()
    {
        Debug.Log("Start");
        //List<Button> buttonsToDisable = areaParentButtons.GetComponentsInChildren<Button>().ToList();
        SoundManager.instance.PlaySE(SELineup.SE_SELECT);

        // お知らせ詳細表示
        go = Instantiate(areaNotificationDetail, GameObject.Find("TitleScreenCanvas").transform, false);
        go.GetComponent<NotificationDetail>().SetNotificationDetail(notification);
        //go.GetComponent<NotificationWindow>().RegisterParentButtons(buttonsToDisable);
        Debug.Log("E n d");
    }

    /// <summary>
    /// タイトル表示
    /// </summary>
    public void NotificationDisp(Notification n)
    {
        notification = n;
        txtNotificationTitle.text = n.notificationTitle;
    }

    /// <summary>
    /// 親画面の制御
    /// </summary>
    public void ParentControl(bool control)
    {
        Debug.Log("Start");
        // 親画面のボタンを非活性にする
        go = GameObject.Find("areaCharacterTrainingPrefab(Clone)");
        buttons = go.GetComponentsInChildren<Button>();
        if (control)
        {
            // 親画面のボタンを有効な状態に戻す
            foreach (Button btn in buttons)
            {
                btn.interactable = true;
            }
        }
        else
        {
            foreach (Button btn in buttons)
            {
                btn.interactable = false;
            }
        }
        Debug.Log("E n d");
    }
}