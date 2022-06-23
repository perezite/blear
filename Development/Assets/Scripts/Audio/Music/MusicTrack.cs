using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicTrack : MonoBehaviour
{
    // configuration
    private MusicConfiguration config;

    // available music clips 
    private List<AudioClip> clips;

    // active music layers
    private List<AudioSource> layers;

    // info about the active music layers
    private List<AudioLayerInfo> layerInfos = new List<AudioLayerInfo>();

    // playback duration for this track
    private float playbackDuration;

    // playback time elapsed
    private float playbackTimeElapsed = 0;

    // is the playback expired
    private bool isPlaybackExpired = false;

    // is the music track playing 
    private bool isPlaying;

    public bool IsPlaybackExpired()
    {
        return isPlaybackExpired;
    }

    public void SetConfig(MusicConfiguration config)
    {
        this.config = config;
    }

    public void SetLayers(List<AudioClip> layers)
    {
        clips = layers;
    }

    public bool IsEqual(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        var other = obj as MusicTrack;
        if (other == null)
        {
            return false;
        }

        bool isEqual = clips.SequenceEqual(other.clips);    // good enough for jazz...
        return isEqual;
    }

    // use all available clips and start them at the specified begin times
    public void Play(List<float> beginTimes)
    {
        Debug.Assert(beginTimes.Count == clips.Count, "Number of specified begin times must match number of specified audio clips");

        // configure playback duration
        playbackTimeElapsed = 0f;
        playbackDuration = Random.Range(config.MinTrackDuration, config.MaxTrackDuration);

        CreateLayers(clips);

        for (int i = 0; i < beginTimes.Count; i++)
        {
            var layer = layers[i];
            var beginTime = beginTimes[i];
            layerInfos.Single(x => x.AudioSource == layer).BeginTime = beginTime;
            StartCoroutine(FadeIn(layer, beginTime, config.FadeInDuration));
        }

        isPlaying = true;

        // Debug.Log(layers.Select(x => x.clip.name).Aggregate((current, next) => current + " " + next));
    }

    public void Play()
    {
        // configure playback duration
        playbackTimeElapsed = 0f;
        playbackDuration = Random.Range(config.MinTrackDuration, config.MaxTrackDuration);

        // pick clips 
        var pickedClips = clips.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)).Take(Random.Range(config.MinLayers, config.MaxLayers + 1)).ToList();

        // create layers and layer infos from clips 
        CreateLayers(pickedClips);

        // fade in layers
        foreach (var layer in layers)
        {
            var beginTime = Random.Range(0, layer.clip.length - 1);
            layerInfos.Single(x => x.AudioSource == layer).BeginTime = beginTime;
            StartCoroutine(FadeIn(layer, beginTime, config.FadeInDuration));
        }

        isPlaying = true;

        // Debug.Log(layers.Select(x => x.clip.name).Aggregate((current, next) => current + " " + next));
    }

    public void Suspend()
    {
        if (isPlaying)
        {
            StopAllCoroutines();
            layers.ForEach(x => StartCoroutine(FadeOut(x, config.FadeOutDuration)));
            layerInfos.ForEach(x => x.BeginTime = Mathf.Repeat(x.AudioSource.time + config.FadeOutDuration, x.AudioSource.clip.length));
        }
        
        isPlaying = false;
    }

    public void Resume()
    {
        if (!isPlaying)
        {
            StopAllCoroutines();
            layerInfos.ForEach(x => StartCoroutine(FadeIn(x.AudioSource, x.BeginTime, config.FadeInDuration)));
        }

        isPlaying = true;
    }

    public void StopAndDestroy()
    {
        layers.ForEach(x => StartCoroutine(FadeOut(x, config.FadeOutDuration)));

        StartCoroutine(WaitForDestroy());
    }

    public void Reset()
    {
        playbackTimeElapsed = 0f;

        foreach (var layer in layers)
        {
            var layerBeginTime = layerInfos.Single(x => x.AudioSource == layer).BeginTime;
            layer.time = layerBeginTime;
        }
    }

    private IEnumerator WaitForSuspension()
    {
        while (true)
        {
            if (!layers.Any(x => x.isPlaying))
            {
                yield break;
            }

            yield return null;
        }
    }

    // wait until all layers faded out, then destroy
    private IEnumerator WaitForDestroy()
    {
        while (true)
        {
            if (!layers.Any(x => x.isPlaying))
            {
                isPlaying = false;
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }
    }

    // create layers and layer infos from clips
    private void CreateLayers(List<AudioClip> clips)
    {
        clips.ForEach(x => CreateAudioSource(x));
        layers = GetComponents<AudioSource>().ToList();
        layerInfos = layers.Select(x => new AudioLayerInfo { AudioSource = x, BeginTime = 0f }).ToList();
    }

    private void Update()
    {
        if (isPlaying)
        {
            playbackTimeElapsed += Time.deltaTime;

            if (playbackTimeElapsed > playbackDuration)
            {
                isPlaybackExpired = true;
            }
        }
    }

    private IEnumerator FadeIn(AudioSource layer, float beginTime, float duration)
    {
        float delta = config.MasterVolume / duration;

        layer.time = beginTime;
        layer.volume = 0;
        layer.Play();

        yield return StartCoroutine(Fade(layer, duration, delta, true));
    }

    private IEnumerator FadeOut(AudioSource layer, float duration)
    {
        float delta = -config.MasterVolume / duration;

        yield return StartCoroutine(Fade(layer, duration, delta, false));

        layer.Stop();
    }

    private IEnumerator Fade(AudioSource layer, float duration, float delta, bool fadeIn)
    {
        // apply fading
        float lastTime = Time.time;
        float currentTime = Time.time;
        while ((fadeIn && layer.volume < config.MasterVolume) || (!fadeIn && layer.volume > 0f))
        {
            currentTime = Time.time;
            var deltaTime = currentTime - lastTime;

            var newLayerVolume = layer.volume + (deltaTime * delta);
            layer.volume = Mathf.Clamp(newLayerVolume, 0f, config.MasterVolume);

            lastTime = currentTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private void CreateAudioSource(AudioClip clip)
    {
        var audio = gameObject.AddComponent<AudioSource>();
        audio.loop = true;
        audio.clip = clip;
        audio.playOnAwake = false;
    }

    public struct MusicConfiguration
    {
        // the master volume
        public float MasterVolume;

        // minimum number of layers per track
        public int MinLayers;

        // maximum number of layers per track
        public int MaxLayers;

        // track fade in duration
        public float FadeInDuration;

        // track fade out duration
        public float FadeOutDuration;

        // minimum track duration
        public float MinTrackDuration;

        // maximum track duration
        public float MaxTrackDuration;
    }

    public class AudioLayerInfo
    {
        // the audio source
        public AudioSource AudioSource;

        // the position the layer started playing
        public float BeginTime;

        // suspended playback position
        public float SuspendedPlaybackPosition;
    }
}
