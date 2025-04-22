using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionCredit : MonoBehaviour
{

    [SerializeField] private Button btnCredit;
    [SerializeField] private GameObject areaOptionCredit;

    GameObject go;

    private void Start()
    {
        Debug.Log("Start");
        btnCredit.onClick.RemoveAllListeners();
        btnCredit.onClick.AddListener(OnClickCredit);
        Debug.Log("E n d");
    }

    public void OnClickCredit()
    {
        List<Button> buttonsToDisable = GameObject.Find("areaOptionPrefab(Clone)").GetComponentsInChildren<Button>().ToList();
        SoundManager.instance.PlaySE(SELineup.SE_SELECT);

        // クレジット表示
        go = Instantiate(areaOptionCredit, GameObject.Find("areaMain").transform, false);
        go.GetComponent<ButtonControll>().RegisterParentButtons(buttonsToDisable);
    }
}
