using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameField CurrentGameField { get; private set; }
    public TrackRecord trackRecord = new();
    public ResourceInfo resourceInfo = new();

    public float gameSpeed = 1f; // ゲームスピード
    
    public void SetGameSpeed(float speed)
    {
        gameSpeed = speed;
    }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep GameManager across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to set the current GameField
    public void SetCurrentGameField(GameField gameField)
    {
        CurrentGameField = gameField;
    }
}
