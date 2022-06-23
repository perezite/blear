using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickSound : MonoBehaviour
{
    // the last played pitch
    private static float lastPitch = float.MinValue;

    // the sounds
    private List<SoundSource2D> sounds;

    public void Play()
    {
        var sound = sounds.Where(x => !Mathf.Approximately(x.Pitch, lastPitch)).OrderBy(n => Random.value).FirstOrDefault();
        lastPitch = sound.Pitch;

        sound.Play();
    }

    public bool IsPlaying()
    {
        return sounds.Any(x => x.IsPlaying());
    }

    private void Start()
    {
        sounds = GetComponents<SoundSource2D>().ToList();
    }
}
