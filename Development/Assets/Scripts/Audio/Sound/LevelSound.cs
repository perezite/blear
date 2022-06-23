using System.Collections;
using UnityEngine;

public class LevelSound : MonoBehaviour
{
    [Tooltip("Level start sound")]
    public SoundSource2D StartSound;

    [Tooltip("Start sound delay")]
    public float StartSoundDelay;

    [Tooltip("Level lose sound")]
    public SoundSource2D LoseSound;

    [Tooltip("Level win sound")]
    public SoundSource2D WinSound;

    public void PlayStartSound()
    {
        StartSound.PlayDelayed(StartSoundDelay);
    }

    public IEnumerator PlayLoseSound()
    {
        LoseSound.Play();

        while (LoseSound.IsPlaying())
        {
            yield return null;
        }
    }

    public IEnumerator PlayWinSound()
    {
        WinSound.Play();

        while (WinSound.IsPlaying())
        {
            yield return null;
        }
    }
}
