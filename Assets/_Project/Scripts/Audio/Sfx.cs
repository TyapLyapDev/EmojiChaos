using System;
using UnityEngine;
using UnityEngine.Audio;

public class Sfx : InitializingBehaviour
{
    private const float SlightPitchDeviation = 0.1f;

    [SerializeField] private AudioMixer _mixer;

    [Header("UI")]
    [SerializeField] private AudioClip _pointerDownButton;
    [SerializeField] private AudioClip _pointerUpButton;
    [SerializeField] private AudioClip _pause;
    [SerializeField] private AudioClip _resume;
    [SerializeField] private AudioClip _panelClosed;
    [SerializeField] private AudioClip _panelShowed;
    [SerializeField] private AudioClip _levelSelected;
    [SerializeField] private AudioClip _levelClosed;
    [SerializeField] private AudioClip _scoreIncreased;
    [SerializeField] private AudioClip _starCollected;
    [SerializeField] private AudioClip _fail;
    [SerializeField] private AudioClip _victory;

    [Header("Star")]
    [SerializeField] private AudioClip _starDeath;
    [SerializeField] private AudioClip _starPoof;
    [SerializeField] private AudioClip _starRelax;
    [SerializeField] private AudioClip[] _starFears;

    [Header("Car")]
    [SerializeField] private AudioClip[] _carCantDrive;
    [SerializeField] private AudioClip[] _carRoars;
    [SerializeField] private AudioClip[] _carAccidents;

    [Header("Gun")]
    [SerializeField] private AudioClip _gunShot;
    [SerializeField] private AudioClip _gunInstalled;
    [SerializeField] private AudioClip _gunDisappearance;

    [Header("Enemy")]
    [SerializeField] private AudioClip[] _enemyHits;

    private AudioSourcePool _pool;
    private bool _isMute;

    public bool IsMute => _isMute;

    public void SetMute() =>
        _isMute = true;

    public void ResetMute() => 
        _isMute = false;

    public void PlayPointerDownButton() =>
        PlayOneShot(_pointerDownButton);

    public void PlayPointerUpButton() =>
        PlayOneShot(_pointerUpButton);

    public void PlayPause() =>
        PlayOneShot(_pause);

    public void PlayResume() =>
        PlayOneShot(_resume);

    public void PlayPanelClosed() =>
        PlayOneShot(_panelClosed);

    public void PlayPanelShowed() =>
        PlayOneShot(_panelShowed);

    public void PlayLevelSelected() =>
        PlayOneShot(_levelSelected);

    public void PlayLevelClosed() =>
        PlayOneShot(_levelClosed);

    public void PlayScoreIncreased() =>
        PlayOneShot(_scoreIncreased);

    public void PlayStarCollected() =>
        PlayOneShot(_starCollected);

    public void PlayFail() =>
        PlayOneShot(_fail);

    public void PlayVictory() =>
        PlayOneShot(_victory);

    public void PlayStarByeBye() =>
        PlayOneShot(_starDeath, SlightPitchDeviation);

    public void PlayStarPoof() =>
        PlayOneShot(_starPoof, SlightPitchDeviation);

    public void PlayStarFear() =>
        PlayOneShot(_starFears, SlightPitchDeviation);

    public void PlayStarRelax() =>
        PlayOneShot(_starRelax, SlightPitchDeviation);

    public void PlayCarCantDrive() =>
        PlayOneShot(_carCantDrive, SlightPitchDeviation);

    public AudioSource PlayCarRoar() =>
        PlayOneShot(_carRoars, SlightPitchDeviation);

    public void PlayCarAccident() =>
        PlayOneShot(_carAccidents, SlightPitchDeviation);

    public void PlayGunShot() =>
        PlayOneShot(_gunShot, SlightPitchDeviation);

    public void PlayGunInstalled() =>
        PlayOneShot(_gunInstalled, SlightPitchDeviation);

    public void PlayGunDisapperence() =>
        PlayOneShot(_gunDisappearance, SlightPitchDeviation);

    public void PlayEnemyHit() =>
        PlayOneShot(_enemyHits, SlightPitchDeviation);

    protected override void OnInitialize()
    {
        AudioMixerGroup[] audioMixerGroups = _mixer.FindMatchingGroups(Constants.SfxGroup);

        if (audioMixerGroups.Length == 0)
            throw new InvalidOperationException($"AudioMixerGroup '{Constants.SfxGroup}' not found!");

        _pool = new(this, audioMixerGroups[0]);
    }

    private AudioSource PlayOneShot(AudioClip clip, float deviationPitch = 0f)
    {
        if (_isMute)
            return null;

        AudioSource source = _pool.Get();
        source.pitch = Mathf.Approximately(deviationPitch, 0) 
            ? 1 
            : UnityEngine.Random.Range(1 - deviationPitch, 1 + deviationPitch);

        source.PlayOneShot(clip);

        return source;
    }

    private AudioSource PlayOneShot(AudioClip[] clips, float deviationPitch = 0f)
    {
        if (_isMute)
            return null;

        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        AudioClip clip = clips[randomIndex];

        return PlayOneShot(clip, deviationPitch);
    }
}