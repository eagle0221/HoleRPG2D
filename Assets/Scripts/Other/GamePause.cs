using UnityEngine;

public class GamePause : MonoBehaviour
{
    private bool isPause = false;
    public bool Ispause { get { return isPause; } }

    void OnEnable()
    {
        isPause = true;
    }

    void Update()
    {
        // ウィンドウが開いている間、ゲームを一時停止
        if (isPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = GameManager.Instance.gameSpeed;
        }
    }

    void OnDisable()
    {
        isPause = false;
    }

}