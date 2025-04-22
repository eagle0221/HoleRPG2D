using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Button btnTapToStart;
    [SerializeField] private TextMeshProUGUI txtPlayerID;
    [SerializeField] private GameObject areaField;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private GameObject saveManager;
    [SerializeField] private GameObject titleScreenCanvas;

    // Start is called before the first frame update
    void Start()
    {
        btnTapToStart.onClick.AddListener(TapToStart);
        Debug.Log(PlayerPrefs.HasKey("txtPlayerID"));
        if(PlayerPrefs.HasKey("txtPlayerID"))
        {
            txtPlayerID.text = $"ID: {PlayerPrefs.GetString("txtPlayerID")}";
        }
        else
        {
            txtPlayerID.text = $"ID: - ";
        }
        SoundManager.instance.StopBGMAll();
        SoundManager.instance.PlayBGM(BGMLineup.OPENING);
    }

    /// <summary>
    /// Tap To Startボタン押下
    /// </summary>
    public void TapToStart()
    {
        Debug.Log("Start");
        SoundManager.instance.PlaySE(SELineup.SE_TAP_TO_START);
        ActiveGUI();
        InactiveGUI();
        btnTapToStart.onClick.RemoveListener(TapToStart);
        Debug.Log("E n d");
    }

    private void ActiveGUI()
    {
        areaField.SetActive(true);
        player.SetActive(true);
        canvasUI.SetActive(true);
        saveManager.SetActive(true);;
    }

    private void InactiveGUI()
    {
        titleScreenCanvas.SetActive(false);
    }
}
