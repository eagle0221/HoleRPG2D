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
    public UIController uiController; // UIControllerへの参照を追加
    public SpawnObject spawnObject; // SpawnObjectへの参照を追加
    public Transform spawnTransform; // オブジェクトを配置する親オブジェクトのTransform
    public GameObject bossSpawner;   // ボス再配置のオブジェクト

    public static bool deathFlg = false;

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
    }

    private void OnNoButtonClicked()
    {
        deathFlg = true;
        // フィールドを基本フィールドに移動
        uiController.LoadMainField();

        // プレイヤーの位置を初期化
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0, -9, -1);
        }

        // プレイヤーのHPを全回復
        playerController.RestoreHP();

        // プレイヤーステータスの初期化
        playerController.status.StatusInitialize();

        // 既に出現していたオブジェクトを削除
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AbsorbableObject"))
        {
            Destroy(obj);
        }
        
        // オブジェクトを再配置
        spawnObject.Start();

        // 既に出現していた敵/ボスを削除
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(obj);
        }
        bossSpawner.SetActive(false);

        // ボスを再配置（ボス再配置の処理は別途実装が必要です）
        bossSpawner.SetActive(true);

        HideTransitionPanel();
        deathFlg = false;
    }
}
