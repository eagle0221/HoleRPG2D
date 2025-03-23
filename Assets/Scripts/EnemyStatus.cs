using UnityEngine;

[System.Serializable]
public class EnemyStatus
{
    public float maxHp = 10f;
    public float hp = 10f;
    public float absorbPower = 1f; // 吸収力（攻撃力）
    public float strength = 0f; // 強度（防御力）
    public float speed = 2f; // スピード
    public float size = 9f; // サイズ
    public float attackSpeed = 1f; // 攻撃スピード

    // コンストラクタ
    public EnemyStatus(float maxHp, float absorbPower, float strength, float speed, float size, float attackSpeed)
    {
        this.maxHp = maxHp;
        this.hp = maxHp;
        this.absorbPower = absorbPower;
        this.strength = strength;
        this.speed = speed;
        this.size = size; // 初期値を1に設定
        this.attackSpeed = attackSpeed;
    }
}
