using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public EnemyStatus enemyStatus;
    public List<DropItemData> dropItems;
    public float exp;
    public int money;
    public Sprite enemySprite;
}

[System.Serializable]
public class EnemyStatus
{
    public float maxHp;
    public float hp;
    public float absorbPower;
    public float strength;
    public float speed;
    public float size;
    public float attackSpeed;
    public float minScale; // 最小サイズ
    public float destroyDistance; // 破壊される距離

    public EnemyStatus(float maxHp, float hp, float absorbPower, float strength, float speed, float size, float attackSpeed, float minScale, float destroyDistance)
    {
        this.maxHp = maxHp;
        this.hp = hp;
        this.absorbPower = absorbPower;
        this.strength = strength;
        this.speed = speed;
        this.size = size;
        this.attackSpeed = attackSpeed;
        this.minScale = minScale;
        this.destroyDistance = destroyDistance;
    }
}

[System.Serializable]
public class DropItemData
{
    public EquipmentItem item;
    public float dropRate;
}
