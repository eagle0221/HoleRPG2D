using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    private Rigidbody2D rb;
    public EnemyData enemyData; // 敵のデータ
    public GameObject equipmentObjectPrefab; // 装備アイテムのプレハブ
    private EnemyStatus status; // 敵のステータス
    private float attackTimer = 0f; // 攻撃タイマー
    private float attackInterval = 0f; // 攻撃間隔

    public Canvas canvas; // Canvasへの参照を追加

    public GameObject damageTextPrefab; // ダメージテキストのプレハブを追加

    void Start()
    {
        gameObject.tag = "Enemy";
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        // Canvasを取得
        canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvasが見つかりません!");
        }

        // EnemyDataからステータスを初期化
        if (enemyData != null)
        {
            status = new EnemyStatus(enemyData.enemyStatus.maxHp, enemyData.enemyStatus.absorbPower, enemyData.enemyStatus.strength, enemyData.enemyStatus.speed, enemyData.enemyStatus.size, enemyData.enemyStatus.attackSpeed);
            UpdateEnemyStatus(); // 初期サイズを適用
        }
        else
        {
            Debug.LogError("EnemyDataが設定されていません!");
        }
        attackInterval = 1f / status.attackSpeed;
    }

    // 敵のステータスを更新するメソッド
    void UpdateEnemyStatus()
    {
        // 敵のサイズを更新
        transform.localScale = Vector3.one * status.size;
        // 敵の移動速度を更新
        //moveSpeed = status.speed; //moveSpeedは使わないので削除
    }

    void Update()
    {
        MoveTowardsPlayer();
        attackTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        // プレイヤーに向かって移動
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * status.speed;
        }
    }

    void DropItem()
    {
        if (enemyData == null) return;

        // ドロップアイテムのリストをループ
        foreach (DropItemData dropItemData in enemyData.dropItems)
        {
            // ランダムな値を生成
            float randomValue = Random.value;

            // ドロップ率と比較
            if (randomValue <= dropItemData.dropRate)
            {
                // ドロップする
                GameObject dropItemObject = Instantiate(equipmentObjectPrefab, transform.position, Quaternion.identity, canvas.transform);
                EquipmentObject equipmentObject = dropItemObject.GetComponent<EquipmentObject>();
                equipmentObject.item = dropItemData.item;
            }
        }
        // お金をドロップ
        if (enemyData.money > 0)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            player.AddMoney(enemyData.money);
        }
    }

    // ダメージを受ける処理
    public void TakeDamage(float damage)
    {
        // 防御力を考慮してダメージを計算
        float actualDamage = Mathf.Max(0, damage - status.strength);
        UpdateEnemyStatus(); // サイズ変更を反映
        status.hp -= actualDamage;
        // ダメージテキストを表示
        ShowDamageText(actualDamage);
        // HPが0以下になったら倒れる
        if (status.hp <= 0)
        {
            Die();
        }
    }

    void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            // ダメージテキストを生成
            GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, canvas.transform); // Canvasを親オブジェクトに設定
            // ダメージテキストのスクリプトを取得
            DamageText damageText = damageTextObject.GetComponent<DamageText>();
            // ダメージテキストにダメージ量を設定
            damageText.SetDamage(damage);
        }
    }

    // 敵が倒れた時の処理
    void Die()
    {
        DropItem();
        // 経験値加算処理を追加
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.AddExp(enemyData.exp); // 敵のデータから経験値を取得して加算
        }
        Destroy(gameObject);
    }

    // プレイヤーとの衝突判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーにダメージを与える
            if (attackTimer >= attackInterval)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(status.absorbPower);
                }
                attackTimer = 0f;
            }
        }
    }
}
