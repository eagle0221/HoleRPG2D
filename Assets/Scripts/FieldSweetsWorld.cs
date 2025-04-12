using UnityEngine;

public class FieldSweetsWorld : MonoBehaviour
{
/*
    private void Awake()
    {
    }
*/
    private void OnEnable()
    {
        // BGMを再生
        SoundManager.instance.StopBGMAll();
        SoundManager.instance.PlayBGM(BGMLineup.SWEETSWORLD_BGM);
    }
}
