using UnityEngine;
using UnityEngine.UI;

public class PlayerLose : MonoBehaviour
{
    public GameObject transitionPanel;
    public Button yesButton;
    public Button noButton;
    private bool isWindowOpen = false; // ステータス画面が開いているかどうか
    public AdmobUnitReward admobUnitReward;
    public PlayerController playerController; // PlayerControllerへの参照を追加

    void OnEnable()
    {
        isWindowOpen = true; // ステータス画面が開いているかどうか
    }

    void OnDisable()
    {
        isWindowOpen = false; // ステータス画面が開いているかどうか
    }

    void Start()
    {
        yesButton.interactable = false;
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    void Update()
    {
        // ウィンドウが開いている間、ゲームを一時停止
        if (isWindowOpen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        yesButton.interactable = admobUnitReward.IsReady;
    }

    public void ShowTransitionPanel()
    {
        transitionPanel.SetActive(true);
    }

    public void HideTransitionPanel()
    {
        transitionPanel.SetActive(false);
    }

    private void OnYesButtonClicked()
    {
        //HideTransitionPanel();
        admobUnitReward.ShowRewardAd((reward) =>
        {
            if (reward != null)
            {
                Debug.Log("Reward type: " + reward.Type);
                Debug.Log("Reward received: " + reward.Amount);
                // 報酬を受け取ったので、プレイヤーのHPを回復
                playerController.RestoreHP();
            }
        });
        yesButton.interactable = false;
        //isWindowOpen = false; // ステータス画面が開いているかどうか
    }

    private void OnNoButtonClicked()
    {
        HideTransitionPanel();
    }
}
