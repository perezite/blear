using System;
using System.Collections.Generic;
using UnityEngine;

// This mono singleton manages the persistent elements of the music
public class MusicManager : MonoBehaviour
{
    private const string MusicTrackGameObjectLabel = "MusicTrack";

    // the singleton instance
    private static MusicManager instance = null;

    // the music configuration
    private MusicTrack.MusicConfiguration config = new MusicTrack.MusicConfiguration();

    // the available audio clips
    private List<AudioClip> layers = new List<AudioClip>();

    // the active track
    private MusicTrack musicTrack;

    // playback time elapsed
    private float timeElapsed;

    // singleton constructor
    private MusicManager()
    {
    }

    // get singleton instance
    public static MusicManager GetInstance()
    {
        if (instance == null)
        {
            var newGameObject = new GameObject();
            newGameObject.name = typeof(MusicManager).Name;
            instance = newGameObject.AddComponent<MusicManager>();
            DontDestroyOnLoad(newGameObject);
        }

        return instance;
    }

    public void SetLayers(List<AudioClip> newLayers)
    {
        layers = newLayers;
    }

    public void SetConfiguration(MusicTrack.MusicConfiguration newConfiguration)
    {
        config = newConfiguration;   
    }

    public void Play(List<float> beginTimes = null)
    {
        var newMusicTrack = CreateMusicTrack();

        // if the new music track is different, start the new one
        if (!newMusicTrack.IsEqual(musicTrack))
        {
            if (musicTrack)
            {
                musicTrack.StopAndDestroy();
            }

            musicTrack = newMusicTrack;
            if (beginTimes != null)
            {
                musicTrack.Play(beginTimes);
            }
            else
            {
                musicTrack.Play();
            }
        }
        else
        {
            Destroy(newMusicTrack.gameObject);
        }
    }

    public void Suspend()
    {
        musicTrack.Suspend();
    }

    public void Resume()
    {
        musicTrack.Resume();
    }

    public void Stop()
    {
        musicTrack.StopAndDestroy();
    }

    public void Next()
    {
        musicTrack.StopAndDestroy();

        musicTrack = CreateMusicTrack();
        musicTrack.Play();
    }

    public void Reset()
    {
        musicTrack.Reset();
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (musicTrack.IsPlaybackExpired())
        {
            Next();
        }
    }

    private MusicTrack CreateMusicTrack()
    {
        var musicTrackGameObject = new GameObject();
        musicTrackGameObject.name = MusicTrackGameObjectLabel;
        var newMusicTrack = musicTrackGameObject.AddComponent<MusicTrack>();
        newMusicTrack.SetConfig(config);
        newMusicTrack.SetLayers(layers);
        newMusicTrack.gameObject.transform.parent = transform;
        return newMusicTrack;
    }
}
