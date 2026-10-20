using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Sound")]
public class SoundData : ScriptableObject
{
    public AudioClip[] clips;                 // several variations
    [Range(0f, 1f)] public float volume = 1f;
    public Vector2 pitchRange = new(0.95f, 1.05f);
    [Range(0f, 1f)] public float spatialBlend = 1f;
    public AudioMixerGroup output;
    public float cooldown = 0.03f;            // no "machine-gun" repeats

    [System.NonSerialized] public float lastPlayTime = -1f;

    public AudioClip PickClip() => clips[Random.Range(0, clips.Length)];
}
