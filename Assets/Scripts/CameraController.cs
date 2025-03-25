using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // プレイヤーのTransformをアタッチ
    public Vector3 offset; // カメラのオフセット
    private float cameraZ; // カメラのZ軸を保持

    void Start()
    {
        cameraZ = transform.position.z; // 初期Z軸を保存
    }

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogError("Playerがアタッチされていません!");
            return;
        }
        // プレイヤーの位置にオフセットを加えた位置を目標位置とする(Vector2で計算)
        Vector2 desiredPosition2D = (Vector2)player.position + (Vector2)offset;

        // カメラの位置を更新(Vector3でZ軸は固定)
        transform.position = new Vector3(desiredPosition2D.x, desiredPosition2D.y, cameraZ);
    }
}
