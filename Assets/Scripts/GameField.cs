using UnityEngine;

public class GameField : MonoBehaviour
{
    public static GameField Instance; // シングルトンパターンで簡単にアクセスできるようにする

    [Header("フィールド境界")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    private void Awake()
    {
        // シングルトンパターン
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
