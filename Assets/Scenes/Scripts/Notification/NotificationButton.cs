using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NotificationButton : MonoBehaviour
{
    [SerializeField] private GameObject areaNotification;
    [SerializeField] private Button btnNotification;

    GameObject go;

    private void Start()
    {
        btnNotification.onClick.AddListener(OnClickNotificationGet);
    }

    /// <summary>
    /// お知らせ押下処理
    /// </summary>
    public void OnClickNotificationGet()
    {
        Debug.Log("Start");

        List<Button> buttonsToDisable = GameObject.Find("TitleScreenCanvas").GetComponentsInChildren<Button>().ToList();
        SoundManager.instance.PlaySE(SELineup.SE_SELECT);

        // お知らせ表示
        go = Instantiate(areaNotification, GameObject.Find("TitleScreenCanvas").transform, false);
        go.GetComponent<NotificationWindow>().RegisterParentButtons(buttonsToDisable);

        Debug.Log("E n d");
    }

}