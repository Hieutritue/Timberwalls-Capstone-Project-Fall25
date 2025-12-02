using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        //Rooms 
        Caniso,
        
        //Personal Actions
        Pooping,
        Eating,
        Washing,
        Sleeping,
        
        //Colonist Action
        Build,
        Destroy,
        BreakingHardMaterial, //Copper mine, Stone mine
        WoodChopping,
        
        
        //Common
        Steam, //(copper furnace and refinery)
        MachineMoving, //Battery factory & cryo plant
        Fire, //Cooking furnitures
        MachinePumping, //Fish Tank
        Heating, // 
        
        //Defense
        FlameThrowerActive,
        LongShotCannonActive,
        RailGunActive,
        RocketLauncherActive,
        StoneCannonActive,
        
        //Entertament
        
        //Living
        BathtubActive,
        SpeakerActive,
        WashingMachineActive,
        WaterTapActive,
        //Medical
        
        //Resource Gathering
        ChickenCoopIdle, //Clucking
        
        
        
        //Science
        
        
        
        
    }
    
    private static Dictionary<Sound, float> soundTimerDictionary;
    
    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.Build] = 0f;
    }
    
    public static void PlayOnce(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(GetAudioClip(sound));
        }
    }
    
    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.Build:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float buildTimerMax = 0.5f;
                    if (lastTimePlayed + buildTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
                break;
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound: " + sound + " not found!");
        return null;
    }
    
    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
        }
    }
}
