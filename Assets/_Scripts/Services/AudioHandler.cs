using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler
{
    private const int OffVolumeSaveKey = -1;
    private const int OnVolumeSaveKey = 1;

    private const string MusicKey = "MusicGroup";
    private const string VFXKey = "VFXGroup";

    private const float OffVolume = -80;
    private const float OnVolume = 0;

    private AudioMixerGroup _masterGroup;

    public AudioHandler(AudioMixerGroup audioMixer)
    {
        _masterGroup = audioMixer;
    }

    public void Initialize()
    {
        int musicSaveValue = PlayerPrefs.GetInt(MusicKey);

        Debug.Log(musicSaveValue);

        if (musicSaveValue == 0 || musicSaveValue == 1)
        {
            _masterGroup.audioMixer.SetFloat(MusicKey, OnVolume);
        }
        else
        {
            _masterGroup.audioMixer.SetFloat(MusicKey, OffVolume);
        }

        int vfxSaveValue = PlayerPrefs.GetInt(VFXKey);
        Debug.Log(vfxSaveValue);
        
        if (vfxSaveValue == 0 || vfxSaveValue == 1)
        {
            _masterGroup.audioMixer.SetFloat(VFXKey, OnVolume);
        }
        else
        {
            _masterGroup.audioMixer.SetFloat(VFXKey, OffVolume);
        }
    }

    public bool IsMusicOn() => GetIntFrom(MusicKey) >= OnVolumeSaveKey;

    public bool ISVFXOn() => GetIntFrom(VFXKey) >= OnVolumeSaveKey;

    public void OffMusic()
    {
        _masterGroup.audioMixer.SetFloat(MusicKey, OffVolume);
        PlayerPrefs.SetInt(MusicKey, OffVolumeSaveKey);
    }

    public void OnMusic()
    {
        _masterGroup.audioMixer.SetFloat(MusicKey, OnVolume);
        PlayerPrefs.SetInt(MusicKey, OnVolumeSaveKey);
    }

    public void OffVFX()
    {
        _masterGroup.audioMixer.SetFloat(VFXKey, OffVolume);
        PlayerPrefs.SetInt(VFXKey, OffVolumeSaveKey);
    }

    public void OnVFX()
    {
        _masterGroup.audioMixer.SetFloat(VFXKey, OnVolume);
        PlayerPrefs.SetInt(VFXKey, OnVolumeSaveKey);
    }

    private void DetermineSound(int condition, string soundKey)
    {
        if (condition == -1)
            _masterGroup.audioMixer.SetFloat(soundKey, OffVolume);
        else
            _masterGroup.audioMixer.SetFloat(soundKey, OnVolume);
    }

    private int GetIntFrom(string soundKey) => PlayerPrefs.GetInt(soundKey);
}
