using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private Slider SliderMasterVol;
    [SerializeField] private Slider SliderMusicVol;
    [SerializeField] private Slider SliderSFXVol;

    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        // on set le volume avec les valeur du player pref ou 0.5f si ils ne sont pas rempli
        audioMixer.SetFloat("MasterVol", Mathf.Log10(PlayerPrefs.GetFloat("MasterVol", 0.5f) * 20));
        audioMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 0.5f) * 20));
        audioMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVol", 0.5f) * 20));

        SliderMasterVol.value = PlayerPrefs.GetFloat("MasterVol", 0.5f);
        SliderMusicVol.value = PlayerPrefs.GetFloat("MusicVol", 0.5f);
        SliderSFXVol.value = PlayerPrefs.GetFloat("SFXVol", 0.5f);


    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVol", volume);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVol", volume);
        PlayerPrefs.Save();
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVol", volume);
        PlayerPrefs.Save();
    }
}
