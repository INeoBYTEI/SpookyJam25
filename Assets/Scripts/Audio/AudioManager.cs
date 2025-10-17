using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class AudioType
{
    public string title;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] float minPitch = 1;
    [SerializeField] float maxPitch = 1;

    public void Play(AudioSource source, float volume = 1, bool pitchVariance = true, float stereoPan = 0)
    {
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
    List<AudioSource> characterSources = new();
    public static AudioManager Instance;
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
    }

    /// <summary>
    /// Plays the selected audio from the audioSource, it finds the audio by title. You can modify volume, panning and turn off pitch variance
    /// </summary>
    public void PlayAudio(AudioSource source, string title, float volume = 1, bool pitchVariance = true, float stereoPan = 0)
    {
        FindAudioType(title).Play(source, volume, pitchVariance, stereoPan);
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

    public void SetAmbiance(AudioClip clip, float volume)
    {
        ambiantSource.clip = clip;
        ambiantSource.volume = volume;
        ambiantSource.loop = true;
    }
}