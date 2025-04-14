using UnityEngine;
using UnityEngine.UI;

public class PlayerLose : MonoBehaviour
{
    public GameObject transitionPanel;
    public Button yesButton;
    public Button noButton;
    private bool isWindowOpen = false; // ウィンドウが開いているかどうか
    public AdmobUnitReward admobUnitReward;
    public PlayerController playerController; // PlayerControllerへの参照を追加
    public ObjectSpawn objectSpawn; // objectSpawnへの参照を追加
    public Transform spawnTransform; // オブジェクトを配置する親オブジェクトのTransform
    public GameObject bossSpawner;   // ボス再配置のオブジェクト
    public GameObject mainField;     // リセット時に戻るフィールド
    private GameObject currentField; // 異世界のGameObject
    private Vector3 returnPosition = new Vector3(0, -9, -1); // リセット時に戻る位置

    public static bool deathFlg = false;

    void OnEnable()
    {
        isWindowOpen = true; // ウィンドウが開いているかどうか
    }

    void OnDisable()
    {
        isWindowOpen = false; // ウィンドウが開いているかどうか
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
        LoadMainField();

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
        playerController.status.statusPoint += playerController.status.rebirthPoint; // 転生ポイントをステータスポイントに加算

        // 既に出現していたオブジェクトを削除
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AbsorbableObject"))
        {
            Destroy(obj);
        }
        // オブジェクトを再配置
        objectSpawn.OnEnable();

        // 既に出現していた敵/ボスを削除
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Boss"))
        {
            Destroy(obj);
        }
        bossSpawner.SetActive(false);

        // ボスを再配置（ボス再配置の処理は別途実装が必要です）
        bossSpawner.SetActive(true);

        HideTransitionPanel();
        deathFlg = false;
    }

    public void LoadMainField()
    {
        currentField = GameObject.FindGameObjectWithTag("Field");
        if(currentField.name != mainField.name)
        {
            currentField.SetActive(false);
            mainField.SetActive(true);
        }
        // プレイヤーを初期位置に戻す
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = returnPosition;
        }
    }

}
