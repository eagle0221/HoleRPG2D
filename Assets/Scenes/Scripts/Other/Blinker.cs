using UnityEngine;
using TMPro;

// 文字点滅用スクリプト
public class Blinker : MonoBehaviour
{

    public float speed = 1.0f;
    private TextMeshProUGUI tMPro;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        tMPro = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tMPro.color = GetAlphaColor(tMPro.color);
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 2.0f;
        return color;
    }
}
