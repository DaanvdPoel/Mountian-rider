using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour //Daan
{
    static public AudioManager instance;

   [Header("Audio files")]
    public AudioClip[] audioClips;
    
    [Header("Audio Volume Mixers")]
    public AudioMixer masterMixer;

    [Header("Audio Source's")]
    public AudioSource soundEffectPlayer;
    public AudioSource backgroundMusicPlayer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(gameObject);
    }


    // ========== the SetVolumeLevel is not my own code. i took it from a youtube video https://www.youtube.com/watch?v=xNHSGMKtlv4&t=2s&ab_channel=JohnFrench  ======================
    /// <summary>
    /// changes the mastervolumemixers it value to the slider value
    /// </summary>
    /// <param name="sliderValue">the value of the volume slider</param>
    public void SetVolumeLevel(float sliderValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    // ========== My Code ======================

    /// <summary>
    /// plays a sound ones
    /// </summary>
    /// <param name="number">the number of the audioclip in the audioclips array</param>
    public void PlaySoundEffect(int number)
    {
        soundEffectPlayer.PlayOneShot(audioClips[number]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"></param>
    public void PlayBackGroundMusic(int number)
    {
        backgroundMusicPlayer.Stop(); //stop playing the song if one is playing
        backgroundMusicPlayer.clip = null; // to make sure the clip = empty
        backgroundMusicPlayer.loop = true; // to make sure its on loop;
        backgroundMusicPlayer.clip = audioClips[number]; //add the wanted music in clip
        backgroundMusicPlayer.Play(); //play

    }
}
