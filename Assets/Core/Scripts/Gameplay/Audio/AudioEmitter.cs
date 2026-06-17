using UnityEngine;
using UnityEngine.Audio;

public class AudioEmitter : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioMixerGroup mixerGroup;

    [Header("Timing")]
    [SerializeField] private float delay = 0f;

    [Header("Si es -1, se ignora la duración")]
    [SerializeField] private float duration = -1f;

    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool loop = false;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    [SerializeField] private float pitch = 1f;
    [SerializeField] private bool spatial = false;

    private void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play()
    {
        AudioManager.Instance.PlayFromEmitter(
            clip,
            transform.position,
            mixerGroup,
            loop,
            volume,
            pitch,
            spatial,
            duration,
            delay
        );
    }
}