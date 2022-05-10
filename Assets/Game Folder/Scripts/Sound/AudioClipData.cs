using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "audios_effect", menuName = "ScriptableObjects/AudioEffect", order = 1)]
public class AudioClipData: ScriptableObject
{
    public string[] name;
    public AudioClip[] audioClip;
}
