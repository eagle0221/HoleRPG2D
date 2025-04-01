using UnityEngine;
using System.Collections.Generic;

public class AbsorbableObject : MonoBehaviour
{
    public List<AbsorbableObjectData> objectDataList; // オブジェクトのデータ
    public AbsorbableObjectData objectData; // オブジェクトのデータ
    private Transform player; // プレイヤーのTransform
    private bool isAbsorbing = false; // 吸収中かどうか

    void Start()
    {
        // プレイヤーを検索して取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if(player == null)
        {
            Debug.Log("Player not found");
        }
        
        // objectDataから情報を初期化
        int rnd = Random.Range(0, objectDataList.Count);
        objectData = objectDataList[rnd];  // 登録したオブジェクトからランダム
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
            transform.localScale -= Vector3.one * 2f * Time.deltaTime; // 縮小速度は適宜調整

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
