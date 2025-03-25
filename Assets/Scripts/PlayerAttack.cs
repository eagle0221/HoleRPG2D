using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AttackRangeController attackRangeController; // AttackRangeControllerスクリプトへの参照

    void Update()
    {
        // 攻撃処理...

        // 攻撃範囲を表示
        attackRangeController.ShowAttackRange();
    }
}
