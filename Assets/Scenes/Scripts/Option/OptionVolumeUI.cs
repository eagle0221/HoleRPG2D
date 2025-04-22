using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.Audio;
using TMPro;
using Unity.VisualScripting;

public class OptionVolumeUI : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sldBGMVolume;
    [SerializeField] private TextMeshProUGUI txtBGMVolume;

    [SerializeField] private Slider sldSEVolume;
    [SerializeField] private TextMeshProUGUI txtSEVolume;

    private GameObject go;

    private void Start()
    {
        sldBGMVolume.onValueChanged.AddListener((value) =>
        {
            // -80～20の間
            value = Mathf.Clamp01(value);
            float decibel = 20f * Mathf.Log10(value);
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            audioMixer.SetFloat("BGM_Volume", decibel);
            txtBGMVolume.text = (Mathf.Round(value * 100)).ToString();
        });
        sldSEVolume.onValueChanged.AddListener((value) =>
        {
            // -80～20の間
            value = Mathf.Clamp01(value);
            float decibel = 20f * Mathf.Log10(value);
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            audioMixer.SetFloat("SE_Volume", decibel);
            txtSEVolume.text = (Mathf.Round(value * 100)).ToString();
        });
    }
}
