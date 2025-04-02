using UnityEngine;
using System.Collections.Generic;

public class GameField : MonoBehaviour
{
    [Header("フィールド名")]
    [SerializeField] private string strStageName;
    public string stageName { get { return strStageName; } }

    [Header("フィールド境界")]
    public float minX = -13f;
    public float maxX = 13f;
    public float minY = -10f;
    public float maxY = 10f;

    [Header("敵の生成")]
    public List<GameObject> enemyPrefabs; // 敵のプレハブのリスト
    public List<EnemyData> enemyDatas; // 敵のデータのリスト
    public float enemySpawnInterval = 5f; // 敵の生成間隔
    private float enemySpawnTimer = 0f; // 敵の生成タイマー

    [Header("オブジェクトの生成")]
    public List<GameObject> objectPrefabs; // オブジェクトのプレハブのリスト
    public List<AbsorbableObjectData> objectDatas; // オブジェクトのデータのリスト
    public float objectSpawnInterval = 2f; // オブジェクトの生成間隔
    //private float objectSpawnTimer = 0f; // オブジェクトの生成タイマー

    private void OnEnable()
    {
        // Register this GameField with the GameManager
        GameManager.Instance.SetCurrentGameField(this);
    }

    private void Update()
    {
        // 敵の生成
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            SpawnEnemy();
            enemySpawnTimer = 0f;
        }

/*
        // オブジェクトの生成
        objectSpawnTimer += Time.deltaTime;
        if (objectSpawnTimer >= objectSpawnInterval)
        {
            SpawnObject();
            objectSpawnTimer = 0f;
        }
*/
    }

    // 敵を生成するメソッド
    void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0 || enemyDatas.Count == 0) return;

        // ランダムな敵のプレハブとデータを選択
        int enemyPrefabIndex = Random.Range(0, enemyPrefabs.Count);
        int enemyDataIndex = Random.Range(0, enemyDatas.Count);

        // ランダムな位置を生成
        Vector2 spawnPosition = GetRandomSpawnPosition();

        // 敵を生成
        GameObject enemyObject = Instantiate(enemyPrefabs[enemyPrefabIndex], spawnPosition, Quaternion.identity);
        EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
        enemyController.enemyData = enemyDatas[enemyDataIndex];
        SpriteRenderer enemySpriteRenderer = enemyObject.GetComponent<SpriteRenderer>();
        enemySpriteRenderer.sprite = enemyController.enemyData.enemySprite;
        enemyController.Initialize(); // Initializeメソッドを呼び出す
    }

/*
    // オブジェクトを生成するメソッド
    void SpawnObject()
    {
        if (objectPrefabs.Count == 0 || objectDatas.Count == 0) return;

        // ランダムなオブジェクトのプレハブとデータを選択
        int objectPrefabIndex = Random.Range(0, objectPrefabs.Count);
        int objectDataIndex = Random.Range(0, objectDatas.Count);

        // ランダムな位置を生成
        Vector2 spawnPosition = GetRandomSpawnPosition();

        // オブジェクトを生成
        GameObject objectObject = Instantiate(objectPrefabs[objectPrefabIndex], spawnPosition, Quaternion.identity);
        AbsorbableObject absorbableObject = objectObject.GetComponent<AbsorbableObject>();
        absorbableObject.objectData = objectDatas[objectDataIndex];
        absorbableObject.Initialize();
    }
*/

    // ランダムな生成位置を取得するメソッド
    Vector2 GetRandomSpawnPosition()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector2(x, y);
    }

    // 指定された位置がフィールド境界内にあるかどうかを確認
    public bool IsWithinBounds(Vector2 position)
    {
        return position.x >= minX && position.x <= maxX &&
               position.y >= minY && position.y <= maxY;
    }

    // 指定された位置をフィールド境界内にクランプする（範囲内に収める）
    public Vector2 ClampPosition(Vector2 position)
    {
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);
        return new Vector2(clampedX, clampedY);
    }

    // エディタ上で境界を可視化する（オプション）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 min = new Vector3(minX, minY, 0);
        Vector3 max = new Vector3(maxX, maxY, 0);
        Vector3 size = max - min;
        Vector3 center = (min + max) / 2f;
        Gizmos.DrawWireCube(center, size);
    }
}
