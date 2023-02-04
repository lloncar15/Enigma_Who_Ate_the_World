using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : Singleton<AudioSystem>
{
    [SerializeField] private AudioSource m_musicSource;
    [SerializeField] private AudioSource m_soundsSource;

    public void PlayMusic(AudioClip clip)
    {
        m_musicSource.clip = clip;
        m_musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1.0f)
    {
        m_soundsSource.transform.position = position;
        PlaySound(clip, volume);
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        m_soundsSource.PlayOneShot(clip, volume);
    }
}
