using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource SFX;

    public AudioClip gameClear;
    public AudioClip gameOver;
    public AudioClip dateWarning;

    private static SoundManager m_instance;

    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
            }
            return m_instance;
        }
    }

    public void SetBGMVolume(float volume)
    {
        BGM.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFX.volume = volume;
    }

    public void PlayWarning()
    {
        SFX.PlayOneShot(dateWarning);
    }

    public void PlayClear()
    {
        SFX.PlayOneShot(gameClear);
    }

    public void PlayOver()
    {
        SFX.PlayOneShot(gameOver);
    }
}
