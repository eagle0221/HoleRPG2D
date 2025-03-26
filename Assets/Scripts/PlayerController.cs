using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PlayerStatus status = new PlayerStatus(); // ステータス管理クラスのインスタンス
    public Slider expSlider;
    public TextMeshProUGUI levelText; // レベル表示用テキスト
    public TextMeshProUGUI statusPointText; // ステータスポイント表示用テキスト
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    public int REBIRTH_LEVEL = 10;
    public GameObject statusPanel; // ステータス割り振りパネル
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI absorbPowerText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI sizeText;
    public TextMeshProUGUI attractionText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI moneyText; // 所持金表示用テキスト
    public EquipmentItem[] equipments = new EquipmentItem[2]; // 装備スロット（2つ）
    public TextMeshProUGUI[] equipmentsName = new TextMeshProUGUI[2]; // 装備スロット（2つ）
    public TextMeshProUGUI[] equipmentsText = new TextMeshProUGUI[2]; // 装備スロット（2つ）
    public Inventory inventory; // インベントリへの参照を追加
    public int money = 0; // 所持金
    public float invincibilityTime = 1f; // 無敵時間
    private float invincibilityTimer = 0f; // 無敵タイマー
    private bool isInvincible = false; // 無敵状態かどうか
    public Slider attackSpeedSlider; // 攻撃スピード表示用Sliderを追加
    private List<EnemyController> enemiesInRange = new List<EnemyController>(); // 攻撃範囲内の敵を管理するリスト
    private EnemyController targetEnemy; // 攻撃対象の敵
    public Canvas canvas; // Canvasへの参照を追加
    public Button rebirthButton; // 転生ボタンへの参照を追加

    void Start()
    {
        // Canvasを取得
        canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvasが見つかりません!");
        }
        rb = GetComponent<Rigidbody2D>();
        expSlider.maxValue = status.maxExp;
        expSlider.value = status.currentExp;
        UpdateUI(); // UIを初期化
        UpdateStatusText(); // 初期ステータスを表示するために追加
        UpdateRebirthButtonInteractable(); // 初期状態でボタンの状態を更新
        inventory = GetComponent<Inventory>(); // インベントリを取得
        UpdatePlayerStatus(); // 初期サイズを適用
        attackSpeedSlider.maxValue = 1f; // 最大値を1に設定
        attackSpeedSlider.value = 0f; // 初期値を0に設定
    }

    void Update()
    {
        HandleInput();
        UpdateExpSlider();
        // 攻撃スピードSliderの更新
        if (enemiesInRange.Count > 0) // 攻撃範囲内に敵がいる場合のみ
        {
            attackSpeedSlider.value += Time.deltaTime * status.attackSpeed; // 攻撃スピードに応じて増加
            attackSpeedSlider.value = Mathf.Clamp(attackSpeedSlider.value, 0f, 1f); // 0～1の範囲に制限

            // Sliderが100%に達したら攻撃
            if (attackSpeedSlider.value >= 1f)
            {
                AttackNearestEnemy(); // 最も近い敵を攻撃
                attackSpeedSlider.value = 0f; // 攻撃後にSliderをリセット
            }
        }
        else
        {
            attackSpeedSlider.value = 0f; // 攻撃範囲内に敵がいない場合はSliderをリセット
        }

        // 無敵時間の処理
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityTime)
            {
                isInvincible = false;
                invincibilityTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        KeepPlayerInBounds(); // プレイヤーを範囲内に保つ処理を追加
    }

    void HandleInput()
    {
        // キーボード入力
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        // スマホ入力
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;
                Vector2 swipeDirection = (touchEndPos - touchStartPos).normalized;
                moveDirection = swipeDirection;
            }
        }
    }

    void MovePlayer()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    // プレイヤーをフィールド範囲内に保つ
    void KeepPlayerInBounds()
    {
        Vector2 clampedPosition = GameField.Instance.ClampPosition(rb.position);
        rb.position = clampedPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AbsorbableObject"))
        {
            // 吸収可能なオブジェクトかどうか確認
            AbsorbableObject absorbable = other.GetComponent<AbsorbableObject>();
            if (absorbable != null)
            {
                // 吸収開始
                absorbable.StartAbsorbing();
            }
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            // 吸収可能なオブジェクトかどうか確認
            if(status.size > enemy.enemyData.enemyStatus.size)
            {
                enemy.StartAbsorbing();
            }
            else if (enemy != null)
            {
                enemiesInRange.Add(enemy); // 攻撃範囲内の敵リストに追加
            }
        }

        if (other.CompareTag("Equipment"))
        {
            Debug.Log("Tag:Equipment");
            EquipmentItem item = other.GetComponent<EquipmentObject>().item;
            inventory.AddItem(item); // インベントリにアイテムを追加
            // 吸収可能なオブジェクトかどうか確認
            AbsorbableObject absorbable = other.GetComponent<AbsorbableObject>();
            if (absorbable != null)
            {
                // 吸収開始
                absorbable.StartAbsorbing();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemiesInRange.Remove(enemy); // 攻撃範囲外に出た敵をリストから削除
            }
        }
    }

    // 最も近い敵を攻撃するメソッド
    void AttackNearestEnemy()
    {
        if (enemiesInRange.Count > 0)
        {
            // 最も近い敵を検索
            targetEnemy = GetNearestEnemy();
            if (targetEnemy != null)
            {
                targetEnemy.TakeDamage(status.absorbPower); // 攻撃
            }
        }
    }

    // 最も近い敵を取得するメソッド
    EnemyController GetNearestEnemy()
    {
        EnemyController nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (EnemyController enemy in enemiesInRange)
        {
            if (enemy == null) continue; // 敵がすでに削除されている場合はスキップ

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    // プレイヤーのステータスを更新するメソッド
    public void UpdatePlayerStatus()
    {
        // プレイヤーのサイズを更新
        transform.localScale = Vector3.one * status.size;
        // プレイヤーの移動速度を更新
        moveSpeed = status.speed;
    }

    public void AddExp(float exp)
    {
        status.currentExp += exp;
        if (status.currentExp >= status.maxExp)
        {
            LevelUp();
        }
    }
    void LevelUp()
    {
        status.level++;
        status.currentExp -= status.maxExp;
        status.maxExp *= 1.2f; // 次のレベルに必要な経験値を増加
        status.statusPoint += 3; // レベルアップ時にステータスポイントを付与
        UpdateUI();
        UpdateRebirthButtonInteractable(); // レベルアップ時にボタンの状態を更新
    }

    // 転生処理
    public void Rebirth()
    {
        if (status.level >= REBIRTH_LEVEL) // 例：レベル10以上で転生可能
        {
            status.ResetForRebirth();
            status.rebirthPoint++;
            status.statusPoint += status.rebirthPoint; // 転生ポイントをステータスポイントに加算
            UpdateStatusText();
            UpdateUI();
            UpdateRebirthButtonInteractable(); // 転生後にボタンの状態を更新
        }
    }

    // 転生ボタンのインタラクションを更新するメソッド
    void UpdateRebirthButtonInteractable()
    {
        if (rebirthButton != null)
        {
            rebirthButton.interactable = status.level >= REBIRTH_LEVEL;
        }
    }

    // ステータス割り振りパネルを開く
    public void OpenStatusPanel()
    {
        statusPanel.SetActive(true);
        UpdateStatusText(); // ステータス割り振りパネルを開いたときにステータスを表示するために追加
    }

    // ステータス割り振りパネルを閉じる
    public void CloseStatusPanel()
    {
        statusPanel.SetActive(false);
    }

    // ステータス割り振り処理
    public void AddStatusPoint(string statusName)
    {
        if (status.statusPoint > 0)
        {
            switch (statusName)
            {
                case "HP":
                    status.maxHp += 10;
                    status.hp = status.maxHp;
                    break;
                case "AbsorbPower":
                    status.absorbPower += 0.1f;
                    break;
                case "Strength":
                    status.strength += 0.1f;
                    break;
                case "Speed":
                    status.speed += 0.1f;
                    break;
                case "Size":
                    status.size += 0.1f; // 1ずつ増加
                    status.size = Mathf.Clamp(status.size, 1f, 500f); // 最大値を500に制限
                    UpdatePlayerStatus(); // サイズ変更を反映
                    break;
                case "Attraction":
                    status.attraction += 0.1f;
                    break;
                case "AttackSpeed":
                    status.attackSpeed += 0.1f;
                    break;
            }
            status.statusPoint--;
            UpdateStatusText(); // ステータスを上げた後にUIを更新するために追加
            UpdateUI();
        }
    }

    // ステータス表示を更新
    public void UpdateStatusText()
    {
        hpText.text = "HP: " + status.hp.ToString() + " / " + status.maxHp.ToString();
        absorbPowerText.text = "吸収力: " + status.absorbPower.ToString("F1");
        strengthText.text = "強度: " + status.strength.ToString("F1");
        speedText.text = "スピード: " + status.speed.ToString("F1");
        sizeText.text = "サイズ: " + status.size.ToString("F1");
        attractionText.text = "引力: " + status.attraction.ToString("F1");
        attackSpeedText.text = "攻撃スピード: " + status.attackSpeed.ToString("F1");
        statusPointText.text = "SP: " + status.statusPoint.ToString(); //ステータスポイントの表示を更新
    }

    // UIの更新
    void UpdateUI()
    {
        expSlider.maxValue = status.maxExp;
        expSlider.value = status.currentExp;
        levelText.text = "Lv: " + status.level.ToString();
        statusPointText.text = "SP: " + status.statusPoint.ToString(); //ステータスポイントの表示を更新
        UpdateMoneyUI();
        //rebirthPointText.text = "RP: " + status.rebirthPoint.ToString(); // 転生ポイントの表示を削除
    }

    public void UpdateMoneyUI()
    {
        moneyText.text = "Money: " + money.ToString();
    }

    void UpdateExpSlider()
    {
        expSlider.value = status.currentExp;
    }

    // 装備処理
    public bool Equip(EquipmentItem item)
    {
        for (int i = 0; i < equipments.Length; i++)
        {
            if (equipments[i] == null)
            {
                equipments[i] = item;
                ApplyEquipmentEffect(item);
                // 装備名と効果の説明をUIに表示
                equipmentsName[i].text = item.itemName;
                equipmentsText[i].text = item.effectDescription;
                return true;
            }
        }
        Debug.Log("装備スロットがいっぱいです");
        return false;
    }

    // 装備効果を適用
    void ApplyEquipmentEffect(EquipmentItem item)
    {
        switch (item.equipmentType)
        {
            case EquipmentItem.EquipmentType.AbsorbPower:
                status.absorbPower += item.value;
                break;
            case EquipmentItem.EquipmentType.Strength:
                status.strength += item.value;
                break;
            case EquipmentItem.EquipmentType.Speed:
                status.speed += item.value;
                break;
            case EquipmentItem.EquipmentType.Size:
                status.size += item.value;
                status.size = Mathf.Clamp(status.size, 1f, 500f); // 最大値を500に制限
                UpdatePlayerStatus(); // サイズ変更を反映
                break;
            case EquipmentItem.EquipmentType.Attraction:
                status.attraction += item.value;
                break;
            case EquipmentItem.EquipmentType.AttackSpeed:
                status.attackSpeed += item.value;
                break;
        }
        UpdateStatusText();
        UpdatePlayerStatus(); // 装備効果を適用後にプレイヤーのステータスを更新
    }
    
    // 装備解除処理
    public void UnEquip(EquipmentItem item)
    {
        switch (item.equipmentType)
        {
            case EquipmentItem.EquipmentType.AbsorbPower:
                status.absorbPower -= item.value;
                break;
            case EquipmentItem.EquipmentType.Strength:
                status.strength -= item.value;
                break;
            case EquipmentItem.EquipmentType.Speed:
                status.speed -= item.value;
                break;
            case EquipmentItem.EquipmentType.Size:
                status.size -= item.value;
                status.size = Mathf.Clamp(status.size, 1f, 500f); // 最大値を500に制限
                UpdatePlayerStatus(); // サイズ変更を反映
                break;
            case EquipmentItem.EquipmentType.Attraction:
                status.attraction -= item.value;
                break;
            case EquipmentItem.EquipmentType.AttackSpeed:
                status.attackSpeed -= item.value;
                break;
        }
        UpdateStatusText();
        UpdatePlayerStatus(); // 装備効果を適用後にプレイヤーのステータスを更新
        for (int i = 0; i < equipments.Length; i++)
        {
            if (equipments[i] == item)
            {
                // 装備名と効果の説明をUIから削除
                equipmentsName[i].text = "";
                equipmentsText[i].text = "";
                break;
            }
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI(); // UIを更新
    }

    // ダメージを受ける処理
    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            status.hp -= damage;
            UpdateStatusText(); // UIを更新
            if (status.hp <= 0)
            {
                // プレイヤーが倒れた時の処理
                Debug.Log("プレイヤーが倒れました");
                // ここにゲームオーバー処理などを追加
            }
            // 無敵状態にする
            isInvincible = true;
        }
    }
}
