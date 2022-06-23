using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public void Init(MusicTrack.MusicConfiguration config, List<AudioClip> layers, List<float> layerBeginTimes)
    {
        // configure music manager
        var musicManager = MusicManager.GetInstance();
        musicManager.SetConfiguration(config);
        musicManager.SetLayers(layers);
        if (layerBeginTimes != null)
        {
            musicManager.Play(layerBeginTimes);
        }
        else
        {
            musicManager.Play();
        }

        // listen for music setting changes
        PlayerProfile.IsMusicEnabledChanged += OnIsMusicEnabledChanged;
        OnIsMusicEnabledChanged();
    }

    public void Stop()
    {
        MusicManager.GetInstance().Stop();
    }

    public void Play()
    {
        MusicManager.GetInstance().Play();
    }

    public void Suspend()
    {
        MusicManager.GetInstance().Suspend();
    }

    public void Resume()
    {
        MusicManager.GetInstance().Resume();
    }

    private void OnIsMusicEnabledChanged()
    {
        var isMusicEnabled = PlayerProfile.GetIsMusicEnabled();

        if (isMusicEnabled)
        {
            Resume();
        }
        else
        {
            Suspend();
        }
    }
}
