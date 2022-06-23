using System;
using System.Collections.Generic;
using UnityEngine;

// Plays a predefined Everloop track (fixed clips and fixed begin time)
public class MenuMusic : Music
{
    [Tooltip("The music clips")]
    public List<AudioClip> Layers;

    [Tooltip("Begin time for each music clip")]
    public List<float> LayerBeginTimes;

    [Tooltip("The master volume")]
    [Range(0f, 1f)]
    public float MasterVolume = 0.8f;

    [Tooltip("Track fade in duration in seconds")]
    [Range(1f, 60f)]
    public float FadeInDuration = 3f;

    [Tooltip("Track fade out duration in seconds")]
    [Range(1f, 60f)]
    public float FadeOutDuration = 3f;

    public void Start()
    {
        // get configuration
        var config = new MusicTrack.MusicConfiguration();
        config.MasterVolume = MasterVolume;
        config.MinLayers = Layers.Count;
        config.MaxLayers = Layers.Count;
        config.FadeInDuration = FadeInDuration;
        config.FadeOutDuration = FadeOutDuration;
        config.MinTrackDuration = float.MaxValue;
        config.MaxTrackDuration = float.MaxValue;

        Init(config, Layers, LayerBeginTimes);
    }
}
