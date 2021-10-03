using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

[System.Serializable]
public class Sound : MonoBehaviour
{
    public SoundEffects SoundName;
    public AudioClip Clip;

    [Range(0, 1f)]
    public float Volume;
    [Range(0.1f, 2f)]
    public float Pitch;

    [HideInInspector]
    public AudioSource source;
}
