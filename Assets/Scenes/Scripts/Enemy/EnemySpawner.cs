using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("敵の生成")]
    public List<GameObject> enemyPrefabs; // 敵のプレハブのリスト
    public List<EnemyData> enemyDatas; // 敵のデータのリスト
    public float enemySpawnInterval = 5f; // 敵の生成間隔
    private float enemySpawnTimer = 0f; // 敵の生成タイマー

    public float minX = -13f;
    public float maxX = 13f;
    public float minY = -10f;
    public float maxY = 10f;

    private void Update()
    {
        // 敵の生成
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            SpawnEnemy();
            enemySpawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0 || enemyDatas.Count == 0) return;

        // ランダムな敵のプレハブとデータを選択
        int enemyPrefabIndex = Random.Range(0, enemyPrefabs.Count);
        int enemyDataIndex = Random.Range(0, enemyDatas.Count);

        // ランダムな位置を生成
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // 敵を生成
        GameObject enemyObject = Instantiate(enemyPrefabs[enemyPrefabIndex], spawnPosition, Quaternion.identity);
        EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
        enemyController.enemyData = enemyDatas[enemyDataIndex];
        SpriteRenderer enemySpriteRenderer = enemyObject.GetComponent<SpriteRenderer>();
        enemySpriteRenderer.sprite = enemyController.enemyData.enemySprite;
        enemyController.Initialize(); // Initializeメソッドを呼び出す
    }

    // ランダムな生成位置を取得するメソッド
    Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector3(x, y, -3);
    }

}
