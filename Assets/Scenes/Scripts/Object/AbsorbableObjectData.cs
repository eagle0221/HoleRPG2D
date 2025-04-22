using UnityEngine;

[CreateAssetMenu(fileName = "New Absorbable Object Data", menuName = "Absorbable Object Data")]
public class AbsorbableObjectData : ScriptableObject
{
    public string objectName;
    public Sprite objectSprite;
    public float moveSpeed = 5f; // 引き寄せられる速度
    public float maxHp = 50f;
    public float hp = 50f;
    public float absorbPower = 3f; // 吸収力（攻撃力）
    public float strength = 0f; // 強度（防御力）
    public float speed = 0f; // スピード
    public float size = 1f; // サイズ
    public float attraction = 1f; // 引力
    public float attackSpeed = 1f; // 攻撃スピード
    public float shrinkSpeed = 2f; // 縮小する速度
    public float minScale = 0.1f; // 最小サイズ
    public float destroyDistance = 0.5f; // 破壊される距離
    public float exp = 10f; // 経験値
}
