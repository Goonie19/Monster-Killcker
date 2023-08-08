using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    [Title("Music")]
    public FMODUnity.EventReference AmbientMusic;
    public FMODUnity.EventReference EndingWindMusic;
    public FMODUnity.EventReference BossFightMusic;
    public FMODUnity.EventReference MainMenuMusic;

    [Title("MenuSFX")]
    public FMODUnity.EventReference ClickButton;
    public FMODUnity.EventReference ClickPlayButton;

    [Title("Game SFX")]
    public FMODUnity.EventReference hitSoundPath;
    public FMODUnity.EventReference killAllySoundPath;
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
    private FMOD.Studio.EventInstance _killAllyInstance;
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

        if(PlayerPrefs.HasKey("MasterVolume"))
            _masterVCA.setVolume(PlayerPrefs.GetFloat("MasterVolume"));
        else
            _masterVCA.setVolume(0.5f);

        if(PlayerPrefs.HasKey("MusicVolume"))
            _masterVCA.setVolume(PlayerPrefs.GetFloat("MusicVolume"));
        else
            _musicVCA.setVolume(0.5f);

        if (PlayerPrefs.HasKey("SFXVolume"))
            _sfxVCA.setVolume(PlayerPrefs.GetFloat("SFXVolume"));
        else
            _sfxVCA.setVolume(0.5f);

    }

    public void PlayAmbientMusic()
    {
        _musicInstance = FMODUnity.RuntimeManager.CreateInstance(AmbientMusic);
        _musicInstance.start();
    }

    public void PlayBossMusic()
    {
        _musicInstance = FMODUnity.RuntimeManager.CreateInstance(BossFightMusic);
        _musicInstance.start();
    }

    public void PlayEndingWind()
    {
        _musicInstance = FMODUnity.RuntimeManager.CreateInstance(EndingWindMusic);
        _musicInstance.start();
    }

    public void PlayMainMenuMusic()
    {
        _musicInstance = FMODUnity.RuntimeManager.CreateInstance(MainMenuMusic);
        _musicInstance.start();
    }

    public void StopMusic()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicInstance.release();
    }

    public void PlayClickButtonSound()
    {
        _clickButtonInstance = FMODUnity.RuntimeManager.CreateInstance(ClickButton);
        _clickButtonInstance.start();
        _clickButtonInstance.release();
    }

    public void PlayClickPlayButtonSound()
    {
        _clickPlayButtonInstance = FMODUnity.RuntimeManager.CreateInstance(ClickPlayButton);
        _clickPlayButtonInstance.start();
        _clickPlayButtonInstance.release();
    }

    public void PlayHitSound()
    {
        _hitInstance = FMODUnity.RuntimeManager.CreateInstance(hitSoundPath);
        _hitInstance.start();
        _hitInstance.release();
    }

    public void PlayKillAllySound()
    {
        _killAllyInstance = FMODUnity.RuntimeManager.CreateInstance(killAllySoundPath);
        _killAllyInstance.start();
        _killAllyInstance.release();
    }

    public void PlayUnlockedSound()
    {
        _unlockInstance = FMODUnity.RuntimeManager.CreateInstance(UnlockSoundPath);
        _unlockInstance.start();
        _unlockInstance.release();
    }

    public void PlayBuySound()
    {
        _buyInstance = FMODUnity.RuntimeManager.CreateInstance(BuySoundPath);
        _buyInstance.start();
        _buyInstance.release();
    }

    #region VOLUME SLIDERS

    public void SetMasterVolume(float volume)
    {
        _masterVCA.setVolume(volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        _musicVCA.setVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVCA.setVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public float GetMasterVolume()
    {
        float volume = 0;
        _masterVCA.getVolume(out volume);
        return volume;
    }

    public float GetMusicVolume()
    {
        float volume = 0;
        _musicVCA.getVolume(out volume);
        return volume;
    }

    public float GetSFXVolume()
    {
        float volume = 0;
        _sfxVCA.getVolume(out volume);
        return volume;
    }
    #endregion

    public void SetBossLifeParameter(float lifePercentage)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("BossLife", lifePercentage);
    }
}
