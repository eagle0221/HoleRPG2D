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
    public GameObject damageTextPrefab; // ダメージテキストのプレハブを追加
    private bool isAbsorbing = false; // 吸収中かどうか
    public ItemDropUIController itemDropUIController; // ItemDropUIControllerへの参照を追加

    void Start()
    {
        gameObject.tag = "Enemy";
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        // EnemyDataからステータスを初期化
        if (enemyData != null)
        {
            status = new EnemyStatus(enemyData.enemyStatus.maxHp, enemyData.enemyStatus.maxHp, enemyData.enemyStatus.absorbPower, enemyData.enemyStatus.strength, enemyData.enemyStatus.speed, enemyData.enemyStatus.size, enemyData.enemyStatus.attackSpeed, enemyData.enemyStatus.minScale, enemyData.enemyStatus.destroyDistance);
            UpdateEnemyStatus(); // 初期サイズを適用
        }
        else
        {
            Debug.LogError("EnemyDataが設定されていません!");
        }
        attackInterval = 1f / status.attackSpeed;
        // ItemDropUIControllerを取得
        itemDropUIController = FindAnyObjectByType<ItemDropUIController>();
        if (itemDropUIController == null)
        {
            Debug.LogError("ItemDropUIControllerが見つかりません!");
        }
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
         if (isAbsorbing)
        {
            // プレイヤーに向かって移動
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * status.speed * Time.deltaTime;

            // 縮小
            transform.localScale -= Vector3.one * 2f * Time.deltaTime; // 縮小速度は適宜調整

            // 最小サイズ以下になったら、またはプレイヤーに近づいたら破壊
            if (transform.localScale.x <= status.minScale || Vector3.Distance(transform.position, player.position) <= status.destroyDistance)
            {
                Absorb();
            }
        }
        else
        {
            MoveTowardsPlayer();
            attackTimer += Time.deltaTime;
        }
   }

    void FixedUpdate()
    {
        // プレイヤーに向かって移動
        if (!isAbsorbing)
        {
            MoveTowardsPlayer();
        }
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
                GameObject dropItemObject = Instantiate(equipmentObjectPrefab, transform.position, Quaternion.identity);
                EquipmentObject equipmentObject = dropItemObject.GetComponent<EquipmentObject>();
                equipmentObject.item = dropItemData.item;
                // ここでアイテムオブジェクトにタグを設定
                dropItemObject.tag = "Equipment";

                // ドロップアイテムを表示
                if (itemDropUIController != null)
                {
                    if(dropItemData.item == null)
                    {
                        Debug.Log("dropItemData.itemが設定されていません!");
                    }
                    if(transform.position == null)
                    {
                        Debug.Log("transform.positionが設定されていません!");
                    }
                    Debug.Log("DropItem called with item: " + (dropItemData.item != null ? dropItemData.item.itemName : "null") + ", position: " + transform.position);
                    itemDropUIController.ShowItemDrop(dropItemData.item, transform.position);
                }
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
            GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, this.transform);
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
            playerController.AddExp(enemyData.exp);
        }
        Destroy(gameObject);
    }

    public void Initialize()
    {
        // EnemyDataから情報を初期化
        GetComponent<SpriteRenderer>().sprite = enemyData.enemySprite;
        gameObject.tag = "Enemy";
    }
}
