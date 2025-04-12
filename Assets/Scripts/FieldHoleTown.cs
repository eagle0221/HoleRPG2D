using UnityEngine;

public class FieldHoleTown : MonoBehaviour
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
        SoundManager.instance.PlayBGM(BGMLineup.HOLETOWN_BGM);
    }
}
