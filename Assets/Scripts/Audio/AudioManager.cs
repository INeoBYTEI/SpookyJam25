using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using NUnit.Framework;

[Serializable]
public class AudioType
{
    public string title;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioSource source;
    [SerializeField] float minPitch = 1;
    [SerializeField] float maxPitch = 1;

    public void Play(float volume = 1, bool pitchVariance = true, float stereoPan = 0, bool interrupt = false)
    {
        if(!interrupt && source.isPlaying) { return; }
        source.clip = audioClips[Random.Range(0, audioClips.Length)];
        source.volume = volume;
        if (pitchVariance)
        {
            source.pitch = Random.Range(minPitch, maxPitch);
        }
        else
        {
            source.pitch = 1;
        }
        source.panStereo = stereoPan;
        source.Play();
    }

    public void SetPitchVariance(float min, float max)
    {
        minPitch = min;
        maxPitch = max;
    }
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioType[] audioTypes;
    public static AudioManager Instance = null;
    public Action<string, string, AudioClip> voicelineFinished;
    public Action<string> audioFinished;
    AudioSource ambiantSource;
    [SerializeField] AudioClip startAmbiance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        ambiantSource = GetComponent<AudioSource>();
        SetAmbiance(startAmbiance);
    }

    /// <summary>
    /// Plays the selected audio from the audioSource, it finds the audio by title. You can modify volume, panning and turn off pitch variance
    /// </summary>
    public void PlayAudio(string title, float volume = 1, bool pitchVariance = true, float stereoPan = 0, bool interrupt = false)
    {
        FindAudioType(title).Play(volume, pitchVariance, stereoPan, interrupt);
    }

    public void PlayAudioSimple(string title)
    {
        PlayAudio(title);
    }

    AudioType FindAudioType(string title)
    {
        foreach (var audioType in audioTypes)
        {
            if (audioType.title == title)
            {
                return audioType;
            }
        }
        Debug.LogError("AudioType \"" + title + "\" doesnt exist");
        return default;
    }

    public void SetAmbiance(AudioClip newClip, float newVolume = 1)
    {
        if(newClip == null) { return; }
        ambiantSource.clip = newClip;
        ambiantSource.volume = newVolume;
        ambiantSource.loop = true;
        ambiantSource.Play();
    }
}