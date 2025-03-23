using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public List<DropItemData> dropItems;
    public float exp; // 敵を倒した時に得られる経験値
    public int money; // 敵を倒したときに得られるお金
    public EnemyStatus enemyStatus; // 敵のステータスを追加
}
