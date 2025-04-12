using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    private Rigidbody2D rb;
    public EnemyData enemyData; // 敵のデータ
    public GameObject equipmentObjectPrefab; // 装備アイテムのプレハブ
    public EnemyStatus status; // 敵のステータス
    private float attackTimer = 0f; // 攻撃タイマー
    private float attackInterval = 0f; // 攻撃間隔
    public GameObject damageTextPrefab; // ダメージテキストのプレハブを追加
    public bool isAbsorbing = false; // 吸収中かどうか
    public ItemDropUIController itemDropUIController; // ItemDropUIControllerへの参照を追加
    private List<PlayerController> playersInRange = new List<PlayerController>(); // 攻撃範囲内のプレイヤーを管理するリスト
    private bool isAttacking = false; // 攻撃中かどうか
    public Slider hpBar;
    public TextMeshProUGUI enemyNameText;
    public Transform damageTextTransform;

    void Start()
    {
        gameObject.tag = "Enemy";
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        // EnemyDataからステータスを初期化
        if (enemyData != null)
        {
            status = new EnemyStatus(enemyData.enemyStatus.maxHp, enemyData.enemyStatus.maxHp, enemyData.enemyStatus.absorbPower, enemyData.enemyStatus.strength, enemyData.enemyStatus.speed, enemyData.enemyStatus.size, enemyData.enemyStatus.attackSpeed, enemyData.enemyStatus.minScale, enemyData.enemyStatus.destroyDistance, enemyData.enemyStatus.isBoss);
            UpdateEnemyStatus(); // 初期サイズを適用
            enemyNameText.text = enemyData.enemyName;
            hpBar.maxValue = status.maxHp;
            hpBar.value = status.maxHp;
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
        attackInterval = 1f / status.attackSpeed; // 攻撃間隔を攻撃スピードから計算
    }

    // 敵のステータスを更新するメソッド
    public void UpdateEnemyStatus()
    {
        // 敵のステータスを更新
        transform.localScale = Vector3.one * status.size;
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
            attackTimer += Time.deltaTime;
            // 攻撃範囲内にプレイヤーがいる場合、攻撃を行う
            if (playersInRange.Count > 0)
            {
                Attack();
            }
        }
    }

    void FixedUpdate()
    {
        if (!isAbsorbing && !enemyData.isBoss)
        {
            Move();
        }
        KeepEnemyInBounds(GameManager.Instance.CurrentGameField); // 敵を範囲内に保つ処理を追加
    }

    // 敵をフィールド範囲内に保つ
    void KeepEnemyInBounds(GameField gameField)
    {
        if(gameField == null) return;
        Vector2 clampedPosition = gameField.ClampPosition(rb.position);
        rb.position = clampedPosition;
    }

    public void UpdateHpBar(float currentHp)
    {
        hpBar.value = currentHp;
    }

    // 攻撃処理
    void Attack()
    {
        if (attackTimer >= attackInterval && !isAttacking)
        {
            isAttacking = true;
            // 攻撃範囲内のすべてのプレイヤーにダメージを与える
            foreach (PlayerController player in playersInRange)
            {
                if (player != null)
                {
                    player.TakeDamage(status.absorbPower);
                }
            }
            attackTimer = 0f; // タイマーをリセット
            isAttacking = false;
        }
    }

    // 攻撃範囲にプレイヤーが入った時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                playersInRange.Add(player); // 攻撃範囲内のプレイヤーリストに追加
            }
        }
    }

    // 攻撃範囲からプレイヤーが出た時の処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                playersInRange.Remove(player); // 攻撃範囲外に出たプレイヤーをリストから削除
            }
        }
    }

    // 移動処理
    void Move()
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
                    if (dropItemData.item == null)
                    {
                        Debug.Log("dropItemData.itemが設定されていません!");
                    }
                    if (transform.position == null)
                    {
                        Debug.Log("transform.positionが設定されていません!");
                    }
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
        //UpdateEnemyStatus(); // サイズ変更を反映
        status.hp -= actualDamage;
        UpdateHpBar(status.hp);
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
            GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, damageTextTransform);
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
        // 倒れたら吸収演出を開始
        StartAbsorbing();
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
        SoundManager.instance.PlaySE(SELineup.ABSORB_SE);
        DropItem();
        isAbsorbing = true;
        rb.linearVelocity = Vector2.zero; // 吸収開始時に移動を停止
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
        GameManager.Instance.trackRecord.EnemyAbsorbCount++;
        Destroy(gameObject);
    }

    public void Initialize()
    {
        // EnemyDataから情報を初期化
        GetComponent<SpriteRenderer>().sprite = enemyData.enemySprite;
        gameObject.tag = "Enemy";
        isAbsorbing = false; // 初期化時に吸収中でないことを確認
        if (enemyData.isBoss)
        {
            gameObject.tag = "Boss";
        }
    }
}
