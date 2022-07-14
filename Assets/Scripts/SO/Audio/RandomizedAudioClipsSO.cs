using System;
using System.Collections.Generic;
using UnityEngine;
using GMTools;

[CreateAssetMenu(fileName = "RandomizeSounds", menuName = "AudioEvent/RandomizedSounds")]
public class RandomizedAudioClipsSO : AudioEvent
{
    [SerializeField] private AudioClip[] audioClips;

    [Range(0, 2f)] [SerializeField] private float minPitch;
    [Range(0, 2f)] [SerializeField] private float maxPitch;
    [Range(0, 1f)] [SerializeField] private float minVolume;
    [Range(0, 1f)] [SerializeField] private float maxVolume;

    System.Random rndIdxClip = new System.Random();
    System.Random rndPitch = new System.Random();
    System.Random rndVolume = new System.Random();

    public void InitRandom(int seed)
    {
        rndIdxClip = new System.Random(seed);
        seed = rndIdxClip.Next(int.MinValue, int.MaxValue);
        rndPitch = new System.Random(seed);
        seed = rndPitch.Next(int.MinValue, int.MaxValue);
        rndVolume = new System.Random(seed);
    }

    public override void PlayClipNext(AudioSource audioSource)
    {
        if (audioClips.Length == 0)
        {
            Debug.LogError($"[{audioSource.gameObject.name}] AudioEvent disabled because SO have the Audio clips array is Emtpty");
            return;
        }
        int nextIdxClip = rndIdxClip.Next(audioClips.Length);
        audioSource.pitch = rndIdxClip.NextFloat(minPitch, maxPitch);
        audioSource.volume = rndVolume.NextFloat(minVolume, maxVolume);
        audioSource.clip = audioClips[nextIdxClip];
        audioSource.Play();
        Debug.Log($"clipIdx[{nextIdxClip}] clipName[{audioSource.clip.name}] pitch={audioSource.pitch:f2} volume={audioSource.volume:f2}");
    }

    private void OnValidate()
    {
        CorrectMinMax(ref minPitch, ref maxPitch);
        CorrectMinMax(ref minVolume, ref maxVolume);
    }

    private void CorrectMinMax(ref float min, ref float max)
    {
        if (min > max)
        {
            float temp = max;
            max = min;
            min = temp;
        }
    }
}