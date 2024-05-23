using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonMonoBase<SoundManager>
{
    public float master
    {
        get => _master;
        set
        {
            _master = value;
            masterEvent?.Invoke(value);
        }
    }
    public float bgm
    {
        get => _bgm;
        set
        {
            _bgm = value;
            bgmEvent?.Invoke(value);
        }
    }

    public float sef
    {
        get => _sef;
        set
        {
            _sef = value;
            sefEvent?.Invoke(value);
        }
    }

    AudioSource _audioSources_BGM;
    AudioSource _audioSources_SEF;
    AudioMixer _mixer;

    float _master;
    float _bgm;
    float _sef;

    public event Action<float> masterEvent;
    public event Action<float> bgmEvent;
    public event Action<float> sefEvent;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        _audioSources_BGM = GetComponent<AudioSource>();
        _audioSources_SEF = gameObject.AddComponent<AudioSource>();
        _mixer = Resources.Load<AudioMixer>("Audio/SoundMixer");
        _audioSources_BGM.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master/BGM")[0];
        _audioSources_SEF.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master/SEF")[0];

        masterEvent += value =>
        {
            SetVolume("Master", value);
        };
        bgmEvent += value =>
        {
            SetVolume("BGM", value);
        };
        sefEvent += value =>
        {
            SetVolume("SEF", value);
        };
    }

    void SetVolume(string type, float volume)
    {
        _mixer.SetFloat(type, Mathf.Log10(volume) * 20);
    }


    public void BGMPlay(string clip)
    {
        AudioClip _bgm = Resources.Load<AudioClip>($"BGM/{clip}");
        _audioSources_BGM.clip = _bgm;
        _audioSources_BGM.Play();
        _audioSources_BGM.loop = true;
    }

    public void SEFPlay(string clip)
    {
        AudioClip _sef = Resources.Load<AudioClip>($"SEF/{clip}");
        _audioSources_SEF.PlayOneShot(_sef);
    }
}