using UnityEngine;

[System.Serializable]
public class DropItemData
{
    public EquipmentItem item;
    [Range(0f, 1f)] public float dropRate; // ドロップ率（0.0〜1.0）
}
