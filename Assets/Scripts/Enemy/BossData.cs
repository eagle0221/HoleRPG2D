using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BossData", menuName = "ScriptableObjects/BossData", order = 1)]
public class BossData : ScriptableObject
{
    public string bossName; // ボスの名前
    public Sprite bossSprite; // ボスのスプライト
    public EnemyStatus bossStatus; // ボスのステータス
    public List<DropItemData> dropItems; // ドロップアイテム
    public ulong money; // ドロップするお金
    public int exp; // ドロップする経験値
    public Vector3 position; // ボスの配置位置
    public float sizeMultiplier = 1.5f; // ボスのサイズ倍率
}
