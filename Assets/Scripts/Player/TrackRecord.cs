using UnityEngine;

[System.Serializable]
public class TrackRecord
{
    [SerializeField] private ulong playerDieCount;     // プレイヤー○亡回数
    [SerializeField] private ulong enemyAbsorbCount;   // 敵吸収数
    [SerializeField] private ulong rebirthCount;       // 転生回数
    [SerializeField] private ulong objectAbsorbCount;  // オブジェクト吸収回数

    // playerDieCount のプロパティ
    public ulong PlayerDieCount
    {
        get { return playerDieCount; }
        set { playerDieCount = value; }
    }

    // enemyDieCount のプロパティ
    public ulong EnemyAbsorbCount
    {
        get { return enemyAbsorbCount; }
        set { enemyAbsorbCount = value; }
    }

    // rebirthCount のプロパティ
    public ulong RebirthCount
    {
        get { return rebirthCount; }
        set { rebirthCount = value; }
    }
   
    // rebirthCount のプロパティ
    public ulong ObjectAbsorbCount
    {
        get { return objectAbsorbCount; }
        set { objectAbsorbCount = value; }
    }

    public TrackRecord(ulong playerDieCount, ulong enemyAbsorbCount, ulong rebirthCount, ulong objectAbsorbCount)
    {
        this.playerDieCount = playerDieCount;
        this.enemyAbsorbCount = enemyAbsorbCount;
        this.rebirthCount = rebirthCount;
        this.objectAbsorbCount = objectAbsorbCount;
    }
    public TrackRecord()
    {
    }

}
