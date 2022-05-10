using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GameManager<SoundManager>
{
    public void PlaySoundEffect(
        string nameEffect, 
        AudioClipData audioClipData, 
        AudioSource audioSource
    ) {
        var audioClip = SoundManager.Instance.GetAudioEffect(
            nameEffect,
            audioClipData.name,
            audioClipData.audioClip
        );
        SoundManager.Instance.PlaySound(audioClip, audioSource);
    }

    void PlaySound(AudioClip audioClip, AudioSource audioSource)
    {
        audioSource.PlayOneShot(audioClip, 1);
    }

    AudioClip GetAudioEffect(
        string attackName, 
        string[] names, 
        AudioClip[] audioClips
    ) {
        for (int i = 0; i < name.Length; i++)
        {
            if (names[i] == attackName)
            {
                return audioClips[i];
            }
        }

        throw new Exception("N�o foi poss�vel achar o �udio");
        
        return null;
    }
}
