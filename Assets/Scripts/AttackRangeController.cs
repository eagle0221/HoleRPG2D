using UnityEngine;

public class AttackRangeController : MonoBehaviour
{
    public GameObject attackRangePrefab; // 攻撃範囲プレハブ
    private GameObject attackRangeInstance; // 生成された攻撃範囲オブジェクト
    public float attackRange = 1.0f; // 攻撃範囲の半径（または矩形のサイズ）
    public float attackDuration = 0.5f; // 攻撃範囲を表示する時間

    public void ShowAttackRange()
    {
        // 攻撃範囲オブジェクトが既に存在する場合は削除
        if (attackRangeInstance != null)
        {
            Destroy(attackRangeInstance);
        }

        // 攻撃範囲オブジェクトを生成
        attackRangeInstance = Instantiate(attackRangePrefab, transform.position, Quaternion.identity);

        // 攻撃範囲オブジェクトの大きさを設定
        attackRangeInstance.transform.localScale = new Vector3(attackRange * 2, attackRange * 2, 1); // 円形の場合
        // attackRangeInstance.transform.localScale = new Vector3(attackRange, attackRange, 1); // 矩形の場合

        // 攻撃範囲オブジェクトの位置を調整
        attackRangeInstance.transform.position = transform.position;
        // 2Dゲームでは、Z軸を0に固定する方が良い場合が多いです。
        attackRangeInstance.transform.position = new Vector3(attackRangeInstance.transform.position.x, attackRangeInstance.transform.position.y, 0);

        // 攻撃範囲オブジェクトを非表示にするコルーチンを開始
        //StartCoroutine(HideAttackRange());
    }

    private System.Collections.IEnumerator HideAttackRange()
    {
        // 指定時間待機
        yield return new WaitForSeconds(attackDuration);

        // 攻撃範囲オブジェクトを削除
        if (attackRangeInstance != null)
        {
            Destroy(attackRangeInstance);
        }
    }
}
