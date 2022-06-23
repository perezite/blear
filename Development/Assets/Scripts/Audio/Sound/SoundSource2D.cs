using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource2D : MonoBehaviour
{
    [Tooltip("The regular audio clip to be played. This clip is affected by the pitch property")]
    public AudioClip AudioClip;

    [Tooltip("The path the sound file to be played on Android. The path is relative to the StreamingAssets folder. This sound is not affected by the pitch property. The file must therefore be baked with the desired pitch")]
    public string AndroidAudioClipPath;

    [Tooltip("The playback volume")]
    [Range(0f, 1f)]
    public float Volume = 1f;

    [Tooltip("The playback pitch")]
    [Range(-3f, 3f)]
    public float Pitch = 1f;

    // the trailing path for streaming assets
    private const string StreamingAssetsTrailingPath = "Assets/StreamingAssets/";

    // android fileIds by ClipPath
    private static Dictionary<string, int> androidFileIds = new Dictionary<string, int>();

    // is there alread an android sound pool
    private static bool hasPool;

    // is sound enabled
    private static bool isSoundEnabled;

    // the unity audio source
    private AudioSource audioSource;

    // last time the source was played
    private float lastPlaybackStartTime = float.MinValue;

    // volume in previous frame
    private float previousVolume = 1f;

    // pitch in previous frame
    private float previousPitch = 1f;

    public void ForcePlay()
    {
        PlaybackSound();
    }

    public void Play()
    {
        if (isSoundEnabled)
        {
            PlaybackSound();
        }
    }

    public void PlayDelayed(float seconds)
    {
        if (isSoundEnabled)
        {
            StartCoroutine(PlayAfter(seconds));
        }
    }

    /// <summary>
    /// Tells whether the SoundSource is currently playing
    /// Note: This function uses the length of AudioClip as a reference. This means, that results will be wrong on Android, if NativeAudioClip has a different baked pitch than the Pitch set on the component.
    /// </summary>
    public bool IsPlaying()
    {
        var soundLength = AudioClip.length * (1f / Pitch);
        return Time.time < lastPlaybackStartTime + soundLength;
    }

    private void PlaybackSound()
    {
        if (IsNativeAndroid() || IsAndroidInEditor())
        {
            // AndroidNativeAudio.play(androidFileIds[AndroidAudioClipPath], leftVolume: Volume);
        }

        if (IsNotAndroid() || IsAndroidInEditor())
        {
            audioSource.Play();
        }

        lastPlaybackStartTime = Time.time;
    }

    private void Start()
    {
        if (IsNativeAndroid() || IsAndroidInEditor())
        {
            if (!hasPool)
            {
                // AndroidNativeAudio.makePool();
                hasPool = true;
            }

            LoadAndroidAudioFile();
        }

        if (IsNotAndroid() || IsAndroidInEditor())
        {
            audioSource = CreateUnityAudioSource();
        }

        UpdateVolume();
        UpdatePitch();

        isSoundEnabled = PlayerProfile.GetIsSoundEnabled();
        PlayerProfile.IsSoundEnabledChanged += OnIsSoundEnabledChanged;
    }

    private void Update()
    {
        if (previousVolume != Volume)
        {
            UpdateVolume();
        }

        if (previousPitch != Pitch)
        {
            UpdatePitch();
        }
    }

    private void UpdateVolume()
    {
        previousVolume = Volume;
        if (audioSource)
        {
            audioSource.volume = Volume;
        }
    }

    private void UpdatePitch()
    {
        previousPitch = Pitch;
        if (audioSource)
        {
            audioSource.pitch = Pitch;
        }
    }

    private AudioSource CreateUnityAudioSource()
    {
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = AudioClip;
        return audioSource;
    }

    private void LoadAndroidAudioFile()
    {
        #if UNITY_EDITOR
            var assetPath = StreamingAssetsTrailingPath + AndroidAudioClipPath;
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.DefaultAsset>(assetPath);
            var displayAssetPath = string.IsNullOrEmpty(AndroidAudioClipPath) ? "null" : assetPath;
            if (asset == null)
            { 
                Debug.LogWarning("The asset at AndroidAudioClipPath = " + displayAssetPath + " does not exist. Playback will fail on Android");
            }
        #endif

        if (IsNativeAndroid() || IsEditor())
        {
            if (!androidFileIds.ContainsKey(AndroidAudioClipPath))
            {
                // var fileId = AndroidNativeAudio.load(AndroidAudioClipPath);
                // androidFileIds.Add(AndroidAudioClipPath, fileId);
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (IsNativeAndroid() || IsEditor())
        {
            if (androidFileIds.ContainsKey(AndroidAudioClipPath))
            {
                // AndroidNativeAudio.unload(androidFileIds[AndroidAudioClipPath]);
                // androidFileIds.Remove(AndroidAudioClipPath);
            }

            if (hasPool)
            {
                // AndroidNativeAudio.releasePool();
                hasPool = false;
            }
        }
    }

    private IEnumerator PlayAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Play();
    }

    private void OnIsSoundEnabledChanged()
    {
        isSoundEnabled = PlayerProfile.GetIsSoundEnabled();
    }

    #region Platform

    private bool IsNativeAndroid()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            return true;
        #else
            return false;
        #endif
    }

    private bool IsAndroidInEditor()
    {
        #if UNITY_ANDROID && UNITY_EDITOR
            return true;
        #else
            return false;
        #endif
    }

    private bool IsNotAndroid()
    {
        #if !UNITY_ANDROID
            return true;
        #else
            return false;
        #endif
    }

    private bool IsEditor()
    {
        #if UNITY_EDITOR
            return true;
        #else
            return false;
        #endif
    }

    #endregion
}