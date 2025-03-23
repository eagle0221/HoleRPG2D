using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 2f;
    private float timer = 0f;
    public Canvas canvas; // Canvasへの参照を追加

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        // フィールド範囲内でランダムな位置を生成
        float randomX = Random.Range(GameField.Instance.minX, GameField.Instance.maxX);
        float randomY = Random.Range(GameField.Instance.minY, GameField.Instance.maxY);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // オブジェクトを生成
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, canvas.transform);
    }
}
