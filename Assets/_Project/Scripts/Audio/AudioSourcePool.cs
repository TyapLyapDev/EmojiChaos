using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePool
{
    private readonly MonoBehaviour _mono;
    private readonly AudioMixerGroup _audioMixerGroup;
    private readonly GameObject _gameObject;
    private readonly List<AudioSource> _audioSourcePool = new();

    public AudioSourcePool(MonoBehaviour mono, AudioMixerGroup audioMixerGroup)
    {
        _mono = mono != null ? mono : throw new ArgumentNullException(nameof(mono));
        _audioMixerGroup = audioMixerGroup != null ? audioMixerGroup : throw new ArgumentNullException(nameof(audioMixerGroup));
        _gameObject = _mono.gameObject;
    }

    public AudioSource Get()
    {
        foreach (AudioSource source in _audioSourcePool)
            if (source.isPlaying == false)
                return source;

        AudioSource newSource = CreateNewSource();
        _audioSourcePool.Add(newSource);

        return newSource;
    }

    private AudioSource CreateNewSource()
    {
        AudioSource newSource = _gameObject.AddComponent<AudioSource>();
        newSource.outputAudioMixerGroup = _audioMixerGroup;
        newSource.loop = false;
        newSource.playOnAwake = false;

        return newSource;
    }
}