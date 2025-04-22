using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // ボスのプレハブ
    public BossData bossData; // ボスのデータ

    void OnEnable()
    {
        SpawnBoss();
    }

    void SpawnBoss()
    {
        // ボスのプレハブを生成
        GameObject bossObject = Instantiate(bossPrefab, bossData.position, Quaternion.identity);

        // ボスのEnemyControllerを取得
        EnemyController bossController = bossObject.GetComponent<EnemyController>();

        // ボスのデータを設定
        bossController.enemyData = ScriptableObject.CreateInstance<EnemyData>();
        bossController.enemyData.enemyName = bossData.bossName;
        bossController.enemyData.enemySprite = bossData.bossSprite;
        bossController.enemyData.enemyStatus = bossData.bossStatus;
        bossController.enemyData.dropItems = bossData.dropItems;
        bossController.enemyData.money = bossData.money;
        bossController.enemyData.exp = bossData.exp;
        bossController.enemyData.isBoss = true;

        // ボスのサイズを設定
        bossController.status.size *= bossData.sizeMultiplier;
        bossController.UpdateEnemyStatus();

        // ボスの移動速度を0に設定
        bossController.status.speed = 0f;
        bossController.Initialize();
    }
}
