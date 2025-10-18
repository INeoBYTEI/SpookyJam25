using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using NUnit.Framework;
using UnityEditor.EditorTools;

[Serializable]
public class AudioType
{
    public string title;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField, Tooltip("Not Required")] AudioSource prePickedSource;
    [SerializeField] float minPitch = 1;
    [SerializeField] float maxPitch = 1;

    public void Play(float volume = 1, bool pitchVariance = true, float stereoPan = 0, bool interrupt = false, AudioSource dynamicSource = null, bool loop = false)
    {
        AudioSource actualSource;
        if (dynamicSource)
        {
            actualSource = dynamicSource;
        }
        else
        {
            if (prePickedSource == null) { Debug.LogError("There is no prepicked source!"); }
            actualSource = prePickedSource;
        }

        if (!interrupt && actualSource.isPlaying) { return; }
        actualSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        actualSource.volume = volume;
        if (pitchVariance)
        {
            actualSource.pitch = Random.Range(minPitch, maxPitch);
        }
        else
        {
            actualSource.pitch = 1;
        }
        actualSource.loop = loop;
        actualSource.panStereo = stereoPan;
        actualSource.Play();
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
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        ambiantSource = GetComponent<AudioSource>();
        SetAmbiance(startAmbiance);
    }

    /// <summary>
    /// Plays the selected audio from the audioSource, it finds the audio by title. You can modify volume, panning and turn off pitch variance
    /// </summary>
    public void PlayAudio(string title, AudioSource source = null, bool loop = false, float volume = 1, bool pitchVariance = true, float stereoPan = 0, bool interrupt = false)
    {
        FindAudioType(title).Play(volume, pitchVariance, stereoPan, interrupt, source, loop);
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
        if (newClip == null) { return; }
        ambiantSource.clip = newClip;
        ambiantSource.volume = newVolume;
        ambiantSource.loop = true;
        ambiantSource.Play();
    }
}