using UnityEngine.Audio;
using UnityEngine;

public enum SFXAudioGroup
{
    General,
}
[System.Serializable]
public class Sound
{
    public string name;
    public SFXAudioGroup category = SFXAudioGroup.General;
    public AudioClip clip;

    [Range(0.0f, 2.0f)]
    public float volume = 1.0f;
    [Range(0.0f, 2.0f)]
    public float pitch = 1.0f;
}
