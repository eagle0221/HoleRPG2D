using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{

    [SerializeField] private Button btnOptionClose;

    private void Start()
    {
        Debug.Log("Start");
        btnOptionClose.onClick.RemoveAllListeners();
        btnOptionClose.onClick.AddListener(OnClickOptionClose);
        Debug.Log("E n d");
    }

    public void OnClickOptionClose()
    {
        SoundManager.instance.PlaySE(SELineup.SE_CANCEL);
        Destroy(gameObject);
        gameObject.GetComponent<ButtonControll>().CloseAction();
    }
}
