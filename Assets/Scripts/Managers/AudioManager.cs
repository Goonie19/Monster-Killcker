using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    [Title("Music")]
    public FMODUnity.EventReference AmbientMusic;

    [Title("MenuSFX")]
    public FMODUnity.EventReference ClickButton;
    public FMODUnity.EventReference ClickPlayButton;

    [Title("Game SFX")]
    public FMODUnity.EventReference hitSoundPath;
    public FMODUnity.EventReference UnlockSoundPath;
    public FMODUnity.EventReference BuySoundPath;

    [Title("Volume Sliders Parameters")]
    public string MasterVCAName;
    public string MusicVCAName;
    public string SFXVCAName;

    private FMOD.Studio.EventInstance _musicInstance;

    private FMOD.Studio.EventInstance _clickButtonInstance;
    private FMOD.Studio.EventInstance _clickPlayButtonInstance;

    private FMOD.Studio.EventInstance _hitInstance;
    private FMOD.Studio.EventInstance _unlockInstance;
    private FMOD.Studio.EventInstance _buyInstance;

    //Volume VCAs
    private FMOD.Studio.VCA _masterVCA;
    private FMOD.Studio.VCA _musicVCA;
    private FMOD.Studio.VCA _sfxVCA;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _masterVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + MasterVCAName);
        _musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + MusicVCAName);
        _sfxVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + SFXVCAName);

    }

    [ContextMenu("PlayAmbientMusic1")]
    public void PlayAmbientMusic()
    {
        _musicInstance = FMODUnity.RuntimeManager.CreateInstance(AmbientMusic);
        _musicInstance.start();
    }

    public void StopAmbientMusic()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayClickButtonSound()
    {
        _clickButtonInstance = FMODUnity.RuntimeManager.CreateInstance(ClickButton);
        _clickButtonInstance.start();
    }

    public void PlayClickPlayButtonSound()
    {
        _clickPlayButtonInstance = FMODUnity.RuntimeManager.CreateInstance(ClickPlayButton);
        _clickPlayButtonInstance.start();
    }

    public void PlayHitSound()
    {
        _hitInstance = FMODUnity.RuntimeManager.CreateInstance(hitSoundPath);
        _hitInstance.start();
    }

    public void PlayUnlockedSound()
    {
        _unlockInstance = FMODUnity.RuntimeManager.CreateInstance(UnlockSoundPath);
        _unlockInstance.start();
    }

    public void PlayBuySound()
    {
        _buyInstance = FMODUnity.RuntimeManager.CreateInstance(BuySoundPath);
        _buyInstance.start();
    }

    #region VOLUME SLIDERS

    public void SetMasterVolume(float volume)
    {
        _masterVCA.setVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        _musicVCA.setVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVCA.setVolume(volume);
    }
    #endregion
}
