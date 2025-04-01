using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public GameObject transitionPanel;
    public Button yesButton;
    public Button noButton;
    public GameObject mainField; // MainFieldのGameObject
    public GameObject anotherWorld; // AnotherWorldのGameObject
    private Vector3 returnPosition = new Vector3(0, -9, -1); // 戻る位置
    private bool isWindowOpen = false; // ステータス画面が開いているかどうか

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        isWindowOpen = true; // ステータス画面が開いているかどうか
    }

    void Start()
    {
        transitionPanel.SetActive(false);
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
        LoadAnotherWorld();
        HideTransitionPanel();
        isWindowOpen = false; // ステータス画面が開いているかどうか
    }

    private void OnNoButtonClicked()
    {
        HideTransitionPanel();
        isWindowOpen = false; // ステータス画面が開いているかどうか
    }

    public void LoadAnotherWorld()
    {
        mainField.SetActive(false);
        anotherWorld.SetActive(true);
    }

    public void LoadMainField()
    {
        anotherWorld.SetActive(false);
        mainField.SetActive(true);
        // プレイヤーを初期位置に戻す
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = returnPosition;
        }
    }
}
