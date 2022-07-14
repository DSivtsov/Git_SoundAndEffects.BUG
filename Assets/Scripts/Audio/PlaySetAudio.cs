using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MyClass
{
    public PlayerState myEnum;
    public EnumAudioClip enumAudio;
}

[RequireComponent(typeof(AudioSource))]
public class PlaySetAudio : MonoBehaviour
{
    [SerializeField] private AudioEvent audioEvent;

    public MyClass[] my;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(PlayerState currentState)
    {
        //if (Mouse.current.rightButton.wasPressedThisFrame)
        //{
        //    audioEvent.PlayClip(audioSource);
        //}
        foreach (var item in my)
        {
            if (item.myEnum == currentState)
            {
                ((SetAudioClipsSO)audioEvent).PlayClip(audioSource, item.enumAudio);
            }
        }

        
    }
}
