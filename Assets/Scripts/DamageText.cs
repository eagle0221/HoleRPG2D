using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    public float destroyDelay = 1f;
    private TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void SetDamage(float damage)
    {
        textMesh.text = damage.ToString();
        StartCoroutine(AnimateDamageText());
    }

    IEnumerator AnimateDamageText()
    {
        float timer = 0f;
        Color textColor = textMesh.color;

        while (timer < destroyDelay)
        {
            timer += Time.deltaTime;
            // 上に移動
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            // フェードアウト
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;
            yield return null;
        }

        Destroy(gameObject);
    }
}
