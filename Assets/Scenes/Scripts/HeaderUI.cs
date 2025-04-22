using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeaderUI : MonoBehaviour
{
    public static HeaderUI Instance { get; private set; }
    [SerializeField] private Slider sliHP;
    [SerializeField] private TextMeshProUGUI txtHP;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText; // レベル表示用テキスト
    [SerializeField] private TextMeshProUGUI txtStageName;
    [SerializeField] private TextMeshProUGUI txtMoney;
    [SerializeField] private TextMeshProUGUI txtChargeCrystal;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep GameManager across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateHeaderUI();
    }

    public void UpdateHeaderUI()
    {
        txtStageName.text = GameManager.Instance.CurrentGameField.stageName;
        txtMoney.text = GameManager.Instance.resourceInfo.Money.ToString("N0");
        txtChargeCrystal.text = GameManager.Instance.resourceInfo.ChargeCrystal.ToString("N0");
    }

}
