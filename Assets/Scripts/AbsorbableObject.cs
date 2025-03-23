using UnityEngine;

public class AbsorbableObject : MonoBehaviour
{
    public float moveSpeed = 5f; // 引き寄せられる速度
    public float shrinkSpeed = 2f; // 縮小する速度
    public float minScale = 0.1f; // 最小サイズ
    public float destroyDistance = 0.5f; // 破壊される距離
    private Transform player; // プレイヤーのTransform
    private bool isAbsorbing = false; // 吸収中かどうか

    void Start()
    {
        // プレイヤーを検索して取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isAbsorbing)
        {
            // プレイヤーに向かって移動
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // 縮小
            transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

            // 最小サイズ以下になったら、またはプレイヤーに近づいたら破壊
            if (transform.localScale.x <= minScale || Vector3.Distance(transform.position, player.position) <= destroyDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    // 吸収を開始するメソッド
    public void StartAbsorbing()
    {
        isAbsorbing = true;
    }
}
