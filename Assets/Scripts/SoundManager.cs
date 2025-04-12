using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private List<AudioSource> audioSourceBGMList;
    [SerializeField] private AudioSource audioSourceSE;
    [SerializeField] private List<AudioClip> audioClipList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);     // シーンに切り替え時に破棄されない状態にする
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// BGMを再生させる
    /// </summary>
    public void PlayBGM(string clipName)
    {
        AudioClip clip = GetAudioClip(clipName);
        if (clip != null)
        {
            foreach (AudioSource audioSource in audioSourceBGMList)
            {
                if(audioSource.isPlaying == false)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                    return;
                }

            }
        }
    }

    /// <summary>
    /// 流れているBGMを全て止める
    /// </summary>
    public void StopBGMAll()
    {
        foreach (AudioSource audioSource in audioSourceBGMList)
        {
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }

        }

    }

    /// <summary>
    /// SEを再生させる
    /// </summary>
    public void PlaySE(string clipName)
    {
        AudioClip clip = GetAudioClip(clipName);
        if (clip != null)
        {
            audioSourceSE.PlayOneShot(clip);
        }
    }

    private AudioClip GetAudioClip(string clipName)
    {
        foreach(AudioClip clip in audioClipList)
        {
            if(clip.name == clipName)
            {
                return clip;
            }
        }
        return null;
    }
}

public class BGMLineup
{
    public const string HOLETOWN_BGM = "maou_game_village06";
    public const string SWEETSWORLD_BGM = "maou_game_dangeon21";
}

public class SELineup
{
    public const string ABSORB_SE = "maou_se_magical08";
    public const string LEVELUP_SE = "ラッパのファンファーレ";
}
