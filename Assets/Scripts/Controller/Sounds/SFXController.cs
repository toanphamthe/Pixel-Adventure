using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [Header("Audio Stats")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        _audioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFXVolume, 0.5f);
    }

    public void PlaySFX(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }

    public void PlayButtonSFX()
    {
        _audioSource.PlayOneShot(_buttonClip);
    }
}
