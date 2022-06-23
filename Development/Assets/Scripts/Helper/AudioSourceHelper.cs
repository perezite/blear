using UnityEngine;

public static class AudioSourceHelper
{
    // copy the values of the audio source component
    private static void CopyValues(AudioSource from, AudioSource to)
    {
        to.clip = from.clip;
        to.outputAudioMixerGroup = from.outputAudioMixerGroup;
        to.mute = from.mute;
        to.bypassEffects = from.bypassEffects;
        to.bypassListenerEffects = from.bypassListenerEffects;
        to.bypassReverbZones = from.bypassReverbZones;
        to.playOnAwake = from.playOnAwake;
        to.loop = from.loop;
        to.priority = from.priority;
        to.volume = from.volume;
        to.pitch = from.pitch;
        to.panStereo = from.panStereo;
        to.spatialBlend = from.spatialBlend;
        to.reverbZoneMix = from.reverbZoneMix;
        to.dopplerLevel = from.dopplerLevel;
        to.rolloffMode = from.rolloffMode;
        to.minDistance = from.minDistance;
        to.spread = from.spread;
        to.maxDistance = from.maxDistance;
    }
}