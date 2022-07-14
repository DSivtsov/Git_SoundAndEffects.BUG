using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SetAudioClips
{
    public EnumAudioClip enumClip;
    [SerializeField] public  AudioClip audioClip;
    [Range(0, 2f)] [SerializeField] public float pitch = 1f;
    [Range(0, 1f)] [SerializeField] public float volume = 1f;

}

public enum MyEnum
{
    Walk,
    Run
}

[CreateAssetMenu(fileName = "SetSounds", menuName = "AudioEvent/SetSounds")]
public class SetAudioClipsSO : AudioEvent
{
    [SerializeField] private SetAudioClips[] audioClips;

    private int nextClipIdx = 0;

    public override void PlayClipNext(AudioSource audioSource)
    {
        CheckClipsArray(audioSource);
        Debug.Log($"nextClipIdx[{nextClipIdx}] enumClip[{audioClips[nextClipIdx].enumClip}] clipName[{audioClips[nextClipIdx].audioClip.name}]" +
            $" pitch={audioClips[nextClipIdx].pitch:f2} volume={audioClips[nextClipIdx].volume:f2}");
        //audioSource.clip = audioClips[nextClipIdx].audioClip;
        //audioSource.Play();
        PlayClip(audioSource, audioClips[nextClipIdx].enumClip);
        nextClipIdx = (nextClipIdx + 1) % audioClips.Length;
    }

    public void PlayClip(AudioSource audioSource, EnumAudioClip enumClip)
    {
        CheckClipsArray(audioSource);
        foreach (var item in audioClips)
        {
            if (item.enumClip == enumClip)
            {
                Debug.Log(item.audioClip.name);
                current = audioSource;
                audioSource.pitch = item.pitch;
                audioSource.volume = item.volume;
                audioSource.clip = item.audioClip;
                audioSource.PlayDelayed(.25f);
                return;
            }
        }
        Debug.LogError($"[{audioSource.gameObject.name}] AudioEvent disabled because SO with Audio clips doesn't have the demanded clip [{enumClip}]");
    }


    private void CheckClipsArray(AudioSource audioSource)
    {
        if (audioClips.Length == 0)
        {
            Debug.LogError($"[{audioSource.gameObject.name}] AudioEvent disabled because SO with Audio clips is Emtpty");
            return;
        }
    }

    AudioSource current;

    private void OnValidate()
    {
        if (current)
        {
            current.PlayDelayed(1f);
        }
    }
}
