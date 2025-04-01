using UnityEngine;

[CreateAssetMenu(fileName = "New Absorbable Object Data", menuName = "Absorbable Object Data")]
public class AbsorbableObjectData : ScriptableObject
{
    public string objectName;
    public Sprite objectSprite;
    public float moveSpeed = 5f; // 引き寄せられる速度
    public float shrinkSpeed = 2f; // 縮小する速度
    public float minScale = 0.1f; // 最小サイズ
    public float destroyDistance = 0.5f; // 破壊される距離
    public float exp = 10f; // 経験値
}
