using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Equipment")]
public class EquipmentItem : ScriptableObject
{
    public string itemName;
    public enum EquipmentType { AbsorbPower, Strength, Speed, Size, Attraction, AttackSpeed }
    public EquipmentType equipmentType;
    public float value;
    public Sprite itemIcon; // アイテムのアイコンを追加
    [TextArea(3, 10)] //複数行のテキストエリアを表示
    public string effectDescription; // 装備の効果を説明するテキストを追加
}