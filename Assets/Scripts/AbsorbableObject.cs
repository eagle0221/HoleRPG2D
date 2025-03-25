using UnityEngine;

public class AbsorbableObject : MonoBehaviour
{
    public AbsorbableObjectData objectData; // オブジェクトのデータ
    private Transform player; // プレイヤーのTransform
    private bool isAbsorbing = false; // 吸収中かどうか

    public void Initialize()
    {
        // プレイヤーを検索して取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("Initialize called");
        if(player == null)
        {
            Debug.Log("Player not found");
        }
        // objectDataから情報を初期化
        GetComponent<SpriteRenderer>().sprite = objectData.objectSprite;
        gameObject.tag = "AbsorbableObject";
    }

    void Update()
    {
        if (isAbsorbing)
        {
            // プレイヤーに向かって移動
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * objectData.moveSpeed * Time.deltaTime;

            // 縮小
            transform.localScale -= Vector3.one * objectData.shrinkSpeed * Time.deltaTime;

            // 最小サイズ以下になったら、またはプレイヤーに近づいたら破壊
            if (transform.localScale.x <= objectData.minScale || Vector3.Distance(transform.position, player.position) <= objectData.destroyDistance)
            {
                Absorb();
            }
        }
    }

    // 吸収を開始するメソッド
    public void StartAbsorbing()
    {
        isAbsorbing = true;
    }

    // 吸収された時の処理
    private void Absorb()
    {
        // 経験値を加算
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.AddExp(objectData.exp);
        }
        Destroy(gameObject);
    }
}
