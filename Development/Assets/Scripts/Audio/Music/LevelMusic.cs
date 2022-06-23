using System.Collections.Generic;
using UnityEngine;

// Plays randomly generated everloop tracks 
public class LevelMusic : Music
{
    [Tooltip("The music clips")]
    public List<AudioClip> Layers;

    [Tooltip("The master volume")]
    public float MasterVolume = 0.8f;

    [Tooltip("Minimum number of music layers per track")]
    [Range(1, 20)]
    public int MinTrackLayers = 1;

    [Tooltip("Maximum number of music layers per track")]
    [Range(1, 20)]
    public int MaxTrackLayers = 1;

    [Tooltip("Track fade in duration in seconds")]
    [Range(1f, 60f)]
    public float FadeInDuration = 3f;

    [Tooltip("Track fade out duration in seconds")]
    [Range(1f, 60f)]
    public float FadeOutDuration = 3f;

    [Tooltip("Minimal track duration in seconds")]
    [Range(1f, 300f)]
    public float MinTrackDuration = 30f;

    [Tooltip("Maximal track duration in seconds")]
    [Range(1f, 300f)]
    public float MaxTrackDuration = 40f;

    private void Start()
    {
        var config = new MusicTrack.MusicConfiguration();
        config.MasterVolume = MasterVolume;
        config.MinLayers = MinTrackLayers;
        config.MaxLayers = MaxTrackLayers;
        config.FadeInDuration = FadeInDuration;
        config.FadeOutDuration = FadeOutDuration;
        config.MinTrackDuration = MinTrackDuration;
        config.MaxTrackDuration = MaxTrackDuration;

        Init(config, Layers, null);
    }
}
