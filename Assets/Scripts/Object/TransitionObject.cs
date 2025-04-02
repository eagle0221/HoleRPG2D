using UnityEngine;

public class TransitionObject : MonoBehaviour
{
    public float respawnTime = 10f; // 再生成までの時間
    private bool isAbsorbed = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAbsorbed)
        {
            isAbsorbed = true;
            UIController.Instance.ShowTransitionPanel();
            gameObject.SetActive(false);
            Invoke("Respawn", respawnTime);
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        isAbsorbed = false;
    }
}
