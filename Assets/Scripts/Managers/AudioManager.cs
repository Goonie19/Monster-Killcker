using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    public FMODUnity.EventReference hitSoundPath;
    public FMODUnity.EventReference UnlockSoundPath;
    public FMODUnity.EventReference BuySoundPath;

    private FMOD.Studio.EventInstance _hitInstance;
    private FMOD.Studio.EventInstance _unlockInstance;
    private FMOD.Studio.EventInstance _buyInstance;

    void Awake()
    {
        Instance = this;
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
}
