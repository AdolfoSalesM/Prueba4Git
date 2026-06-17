using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Pool")]
    [SerializeField] private int initialPoolSize = 10;

    private readonly List<AudioSource> pool = new();

    private Transform audioPoolContainer;

    // MUSIC
    private AudioSource musicSource;
    private AudioSource overrideMusicSource;

    private CancellationTokenSource overrideCTS;

    // 🔥 estado REAL de música base
    private AudioClip baseMusicClip;
    private float baseMusicTime;
    private float baseMusicVolume;
    private bool baseMusicWasPlaying;
    private AudioMixerGroup baseMusicGroup;
    private bool baseMusicLoop;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CreatePoolContainer();
        CreatePool(initialPoolSize);
    }

    #region POOL

    private void CreatePoolContainer()
    {
        GameObject container = new GameObject("AudioPool");
        container.transform.SetParent(transform);
        audioPoolContainer = container.transform;
    }

    private void CreatePool(int amount)
    {
        for (int i = 0; i < amount; i++)
            CreateAudioSource();
    }

    private AudioSource CreateAudioSource()
    {
        GameObject go = new GameObject("PooledAudioSource");
        go.transform.SetParent(audioPoolContainer);

        AudioSource source = go.AddComponent<AudioSource>();
        pool.Add(source);

        return source;
    }

    private AudioSource GetAvailableSource()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].isPlaying)
                return pool[i];
        }

        return CreateAudioSource();
    }

    #endregion

    #region ENTRY

    public async void PlayFromEmitter(
        AudioClip clip,
        Vector3 position,
        AudioMixerGroup mixerGroup,
        bool loop,
        float volume,
        float pitch,
        bool spatial,
        float duration,
        float delay)
    {
        if (clip == null) return;

        if (delay > 0f)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        if (mixerGroup != null && mixerGroup.name == "Music")
        {
            PlayMusicFromEmitter(clip, mixerGroup, volume, loop, duration);
            return;
        }

        PlaySFX(clip, position, mixerGroup, loop, volume, pitch, spatial, duration);
    }

    #endregion

    #region SFX

    private void PlaySFX(
        AudioClip clip,
        Vector3 position,
        AudioMixerGroup mixerGroup,
        bool loop,
        float volume,
        float pitch,
        bool spatial,
        float duration)
    {
        AudioSource source = GetAvailableSource();

        source.transform.position = position;

        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = spatial ? 1f : 0f;
        source.outputAudioMixerGroup = mixerGroup;

        source.Play();

        if (duration >= 0f)
            RunStopAfterTime(source, duration).Forget();
    }

    private async UniTaskVoid RunStopAfterTime(AudioSource source, float duration)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        if (source != null)
        {
            source.Stop();
            source.clip = null;
        }
    }

    #endregion

    #region MUSIC

    private void PlayMusicFromEmitter(
        AudioClip clip,
        AudioMixerGroup musicGroup,
        float volume,
        bool loop,
        float duration)
    {
        if (musicSource == null)
            musicSource = CreateMusicSource();

        // 🔥 GUARDAR ESTADO REAL COMPLETO
        baseMusicClip = musicSource.clip;
        baseMusicTime = musicSource.time;
        baseMusicVolume = musicSource.volume;
        baseMusicGroup = musicSource.outputAudioMixerGroup;
        baseMusicLoop = musicSource.loop;
        baseMusicWasPlaying = musicSource.isPlaying;

        if (baseMusicWasPlaying)
            musicSource.Pause();

        // 🔥 override
        musicSource.clip = clip;
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.volume = volume;
        musicSource.loop = loop;

        musicSource.Play();

        if (duration >= 0f)
            RunMusicRestore(duration).Forget();
    }

    private async UniTaskVoid RunMusicRestore(float duration)
    {
        overrideCTS?.Cancel();
        overrideCTS = new CancellationTokenSource();

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: overrideCTS.Token);
        }
        catch
        {
            return;
        }

        if (musicSource == null) return;

        // 🔥 parar override
        musicSource.Stop();

        // 🔥 restaurar SI había música previa real
        if (baseMusicWasPlaying && baseMusicClip != null)
        {
            musicSource.clip = baseMusicClip;
            musicSource.outputAudioMixerGroup = baseMusicGroup;
            musicSource.volume = baseMusicVolume;
            musicSource.loop = baseMusicLoop;

            musicSource.time = baseMusicTime;
            musicSource.Play();
        }
    }

    #endregion

    #region MUSIC SOURCE

    private AudioSource CreateMusicSource()
    {
        GameObject go = new GameObject("MusicSource");
        go.transform.SetParent(transform);

        return go.AddComponent<AudioSource>();
    }

    #endregion
}