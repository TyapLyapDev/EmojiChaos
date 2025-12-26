using UnityEngine;

public class Music : InitializingBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _levelMusic;

    private bool _isMute = false;
    private bool _isMenu = true;
    private bool _isPause = false;

    public void SetMute()
    {
        _isMute = true;
        _source.Pause();
    }

    public void ResetMute()
    {
        _isMute = false;

        if (_isPause)
            return;

        if (_source.clip != null)
        {
            UnPause();
            return;
        }

        if (_isMenu)
            PlayMenu();
        else
            PlayLevel();
    }

    public void PlayMenu()
    {
        _isMenu = true;
        Play(_menuMusic);
    }

    public void PlayLevel()
    {
        _isMenu = false;
        Play(_levelMusic);
    }

    public void Pause()
    {
        _isPause = true;
        _source.Pause();
    }

    public void UnPause()
    {
        _isPause = false;
        _source.UnPause();

        if (_source.isPlaying == false)
            _source.Play();
    }

    protected override void OnInitialize()
    {
        _source.loop = true;
        _source.playOnAwake = false;
    }

    private void Play(AudioClip clip)
    {
        _isPause = false;
        _source.clip = clip;
        _source.time = 0f;

        if (_isMute)
            return;

        _source.Play();
    }
}