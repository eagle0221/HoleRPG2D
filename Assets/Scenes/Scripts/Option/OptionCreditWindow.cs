using UnityEngine;
using UnityEngine.UI;

public class OptionCreditWindow : MonoBehaviour
{

    [SerializeField] private Button btnCreditClose;

    GameObject go;

    private void Start()
    {
        Debug.Log("Start");
        btnCreditClose.onClick.RemoveAllListeners();
        btnCreditClose.onClick.AddListener(OnClickCredit);
        Debug.Log("E n d");
    }

    public void OnClickCredit()
    {
        // クレジット表示
        SoundManager.instance.PlaySE(SELineup.SE_CANCEL);
        gameObject.GetComponent<ButtonControll>().CloseAction();
        Destroy(gameObject);
    }
}
