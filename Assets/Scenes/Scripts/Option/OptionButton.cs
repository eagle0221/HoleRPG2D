using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{

    [SerializeField] private Button btnOption;
    [SerializeField] private GameObject areaOptionPrefab;

    public GameObject go;

    private void Start()
    {
        Debug.Log("Start");
        btnOption.onClick.RemoveAllListeners();
        btnOption.onClick.AddListener(OnClickOpenOption);
        Debug.Log("E n d");
    }

    public void OnClickOpenOption()
    {
        List<Button> buttonsToDisable = GameObject.Find("TitleScreenCanvas").GetComponentsInChildren<Button>().ToList();
        SoundManager.instance.PlaySE(SELineup.SE_SELECT);

        go = Instantiate(areaOptionPrefab, GameObject.Find("areaMain").transform, false);
        go.GetComponent<ButtonControll>().RegisterParentButtons(buttonsToDisable);
    }
}
