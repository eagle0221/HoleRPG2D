using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public Vector3 position;
    public float maxHp = 50f;
    public float hp = 50f;
    public float absorbPower = 3f; // 吸収力（攻撃力）
    public float strength = 0f; // 強度（防御力）
    public float speed = 1f; // スピード
    public float size = 1f; // サイズ
    public float attraction = 1f; // 引力
    public float attackSpeed = 1f; // 攻撃スピード
    public float currentExp = 0f;
    public float maxExp = 100f;
    public int level = 1;
    public int statusPoint = 0;
    public int rebirthPoint = 0;

    // 転生時のリセット処理
    public void StatusInitialize()
    {
        level = 1;
        currentExp = 0f;
        maxExp = 100f;
        statusPoint = 0;
        maxHp = 100f;
        hp = maxHp;
        absorbPower = 3f;
        strength = 0f;
        speed = 1f;
        size = 1f; // 初期値を1に設定
        attraction = 1f;
        attackSpeed = 1f;
    }
    // 転生時のリセット処理
    public void ResetForRebirth()
    {
        StatusInitialize();
        GameManager.Instance.trackRecord.RebirthCount++;
    }
}
