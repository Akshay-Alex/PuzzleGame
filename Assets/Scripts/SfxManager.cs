using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager sfxManager;
    public AudioSource _audioSource;
    public AudioClip _sfxClick;
    public AudioClip _sfxWin;
    // Start is called before the first frame update
    void Start()
    {
        sfxManager = this;
        _audioSource = this.gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }
    public void PlayClickAudio()
    {
        _audioSource.clip = _sfxClick;
        _audioSource.Play();
    }
    public void PlayWinAudio()
    {
        _audioSource.clip = _sfxWin;
        _audioSource.Play();
    }


}
