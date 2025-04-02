using UnityEngine;
using TMPro;

[System.Serializable]
public class TrackRecordUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPlayerDieCount;     // プレイヤー○亡回数
    [SerializeField] private TextMeshProUGUI txtEnemyDieCount;      // 敵の討伐数
    [SerializeField] private TextMeshProUGUI txtRebirthCount;       // 転生回数
    [SerializeField] private TextMeshProUGUI txtObjectAbsorbCount;  // オブジェクト回数

    void Update()
    {
        txtPlayerDieCount.text = GameManager.Instance.trackRecord.PlayerDieCount.ToString();
        txtEnemyDieCount.text = GameManager.Instance.trackRecord.EnemyAbsorbCount.ToString();
        txtRebirthCount.text = GameManager.Instance.trackRecord.RebirthCount.ToString();
        txtObjectAbsorbCount.text = GameManager.Instance.trackRecord.ObjectAbsorbCount.ToString();
    }
}
