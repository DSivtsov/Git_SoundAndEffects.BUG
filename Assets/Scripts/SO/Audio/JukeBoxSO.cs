using UnityEngine;
using GMTools;

public enum JukeBoxMode
{
    Full,
    Begin,
    End
}
/// <summary>
/// Implementing the playback of audio clips on a randomized schedule
/// </summary>
[CreateAssetMenu(fileName = "JukeBox", menuName = "AudioEvent/JukeBox")]
public class JukeBoxSO : AudioEvent
{
    #region SerializedFields
    [SerializeField] private AudioClip[] audioClips;
    [Tooltip("Internal time for prepare to loading next audioclip")]
    [Range(0, 3), SerializeField] private float LoadDelayTime = 3;
    [Tooltip("The time of mixing the end of the current with begin of the next audioclip")]
    [Range(0, 3), SerializeField] private float CrossDeltaTime = 0;
    [Tooltip("Delay before start music for loading first audioclip")]
    [Range(0, 5), SerializeField] private float StartDelay = 2;
    [Header("JukeBoxMode")]
    [Tooltip("Begin - play only begin of clip, End - play only end of clip")]
    [SerializeField] private JukeBoxMode PlayMode = JukeBoxMode.Full;
    [Tooltip("Begin mode only - Playback time is measured from the begin of the clip.")]
    [Range(0, 20), SerializeField] private int TimeFromBegin = 15;
    [Tooltip("End mode only - Play time is measured from the end of the clip.")]
    [Range(0, 20), SerializeField] private int TimeFromEnd = 15;
    #endregion

    #region NonSerializedFields
    public bool IsTimeStartPreparing() => AudioSettings.dspTime > beginPrepareTime;

    //private System.Random rndIdxClip = new System.Random();
    private double beginPrepareTime;
    private double timeScheduled;
    private int flip;
    private AudioSource[] audioSources;
    private AudioSource nextAudioSource;
    //private SequenceIterationManager iterationManager;
    private ISequenceIteration iterationManager;
    #endregion

    /// <summary>
    /// Initialize the JukeBox
    /// </summary>
    /// <param name="audioSources"></param>
    /// <param name="sequenceType"></param>
    /// <param name="initialValue"> Set not zerro value as the special initial Value or set zerro in other cases. For RND Sequence use the Value as initial seed.
    /// For InSequence use the Value as the start number</param>
    public void InitJukeBox(AudioSource[] audioSources, SequenceType sequenceType, int? initialValue)
    {
        iterationManager = SequenceIterationManager.CreateSequenceIteration(audioClips.Length, sequenceType, initialValue);

        this.audioSources = audioSources;

        flip = 0;
        nextAudioSource = audioSources[flip];
    }
    /// <summary>
    /// USED ONLY for PlayOneClip in Editor
    /// </summary>
    private int nextClipIdx = 0;
    /// <summary>
    /// Base Method to test setting of SO AudioEvent USED ONLY in Editor. Use the sequenceType "in sequence" begin from 0 and JukeBoxMode "Full"
    /// </summary>
    /// <param name="audioSource">used the temporary audioSource for AudioEvent in Editor</param>
    public override void PlayClipNext(AudioSource audioSource)
    {
        //nextAudioSource = audioSource;
        //if (ClipsArrayEmpty())
        //{
        //    Debug.LogError($"[{this}] SO with Audio clips is Empty");
        //    return;
        //}
        //audioSource.clip = audioClips[nextClipIdx];
        ////audioSource.Play();
        //nextClipIdx = (nextClipIdx + 1) % audioClips.Length;
    }
    /// <summary>
    /// Init the playing audio clip on schedule
    /// </summary>
    public void PlayClipNextInit()
    {
        if (ClipsArrayEmpty())
        {
            Debug.LogError($"[{this}] SO with Audio clips is Empty");
            return;
        }
        timeScheduled = AudioSettings.dspTime + StartDelay;

        float lenght = PrepareStartNextClip();

        UpdatePrepareStartTimers(lenght);
    }
    /// <summary>
    /// Start next audio clip on schedule
    /// </summary>
    public void PlayScheduledNextClip()
    {
        //Prepare the next audioclip to start it in the timeScheduled
        float lenght = PrepareStartNextClip();

        // Schedule the stop of current according to the PlayMode and other parameters. 
        PrepareStopCurrentAudioSource();
        // Update the the timeScheduled and other parameters to the start of next audioclip
        UpdatePrepareStartTimers(lenght);
    }

    /// <summary>
    /// Skip the current audio clip and go to the next scheduled one
    /// </summary>
    public void SwitchToNextClip()
    {
        timeScheduled = AudioSettings.dspTime + StartDelay;
        PlayScheduledNextClip();
    }

    /// <summary>
    /// Update the timeScheduled and beginPrepareTime and other parameters to the start the next audioclip
    /// </summary>
    /// <param name="lenght">lenght of the next audioclip</param>
    private void UpdatePrepareStartTimers(float lenght)
    {
        SwitchNextAudioSource();
        timeScheduled += lenght - CrossDeltaTime;
        beginPrepareTime = timeScheduled - LoadDelayTime;
        //Debug.Log($"flip[{flip}] [{audioSources[flip].GetInstanceID()}]: Planned timeScheduled={timeScheduled:f3}]");
    }

    /// <summary>
    /// Schedule the stop of current according to the PlayMode and other parameters
    /// </summary>
    private void PrepareStopCurrentAudioSource()
    {
        //if (PlayMode == JukeBoxMode.Begin || PlayMode == JukeBoxMode.Full)
        //{
            audioSources[1 - flip].SetScheduledEndTime(timeScheduled + CrossDeltaTime);
        //}
    }

    private void SwitchNextAudioSource()
    {
        flip = 1 - flip;
        nextAudioSource = audioSources[flip];
    }

    /// <summary>
    /// Prepare and Start the next audioclip to start it in the timeScheduled on AudioSource - audioSources[flip]
    /// </summary>
    /// <returns>the lenght of this audioclip</returns>
    private float PrepareStartNextClip()
    {
        nextAudioSource.clip = audioClips[iterationManager.Next()];
        if (PlayMode == JukeBoxMode.End)
        {
            ShiftStartPosition(nextAudioSource, TimeFromEnd);
        }
        nextAudioSource.PlayScheduled(timeScheduled);
        //Debug.Log($"flip[{flip}] [{audioSources[flip].GetInstanceID()}]:PlayScheduled clipName[{audioSources[flip].clip.name} at dspTime{AudioSettings.dspTime:f3}]");
        return ClipLenghtJukeBoxMode(nextAudioSource.clip);
    }

    private float ClipLenghtJukeBoxMode(AudioClip audioClip)
    {
        return PlayMode switch
        {
            JukeBoxMode.Full => GetLenghtClip(audioClip),
            JukeBoxMode.Begin => (TimeFromBegin > 0) ? TimeFromBegin : GetLenghtClip(audioClip),
            JukeBoxMode.End => (TimeFromEnd > 0) ? TimeFromEnd : GetLenghtClip(audioClip),
            _ => throw new System.NotImplementedException($"Absent [{PlayMode}] JukeBoxMode value")
        };
    }

    private void ShiftStartPosition(AudioSource audioSource, float timeBeforeEnd)
    {
        if (timeBeforeEnd < 0) return;
        audioSource.timeSamples = audioSource.clip.samples - GetDeltaSample(audioSource.clip, timeBeforeEnd);
    }

    public bool ClipsArrayEmpty() => audioClips.Length == 0;

    private float GetLenghtClip(AudioClip clip) => clip.samples * 1f / clip.frequency;

    private int GetDeltaSample(AudioClip clip, float timeDelta) => (int)(timeDelta * clip.frequency) ;

    #region Editor code only for checking the set values which related to Playmode parameters
#if UNITY_EDITOR
    private JukeBoxMode oldValue;


    string WarningMessage => $"Used the JukeBoxSO in demo mode [{PlayMode}]";

    private void OnEnable()
    {
        oldValue = JukeBoxMode.Full;
        if (PlayMode != JukeBoxMode.Full)
        {

            DetectValueChangeMessage(WarningMessage);
        }
    }

    private void OnValidate()
    {
        switch (PlayMode)
        {
            case JukeBoxMode.Full:
                TimeFromEnd = 0;
                TimeFromBegin = 0;
                break;
            case JukeBoxMode.Begin:
                TimeFromEnd = 0;
                if (TimeFromBegin < CrossDeltaTime + LoadDelayTime)
                {
                    //Debug.LogWarning("Corrected the TimeFromBegin must be more in twice time than CrossDeltaTime + LoadDelayTime ");
                    TimeFromBegin = SetMinTimeSwitch();
                }
                DetectValueChangeMessage(WarningMessage);
                break;
            case JukeBoxMode.End:
                TimeFromBegin = 0;
                if (TimeFromEnd < CrossDeltaTime + LoadDelayTime)
                {
                    //Debug.LogWarning("Corrected the TimeFromEnd must be more in twice time than CrossDeltaTime + LoadDelayTime");
                    TimeFromEnd = SetMinTimeSwitch();
                }
                DetectValueChangeMessage(WarningMessage);
                break;
        }
    }

    // Used only for JukeBoxMode.Begin and JukeBoxMode.End mode
    private int SetMinTimeSwitch()
    {
        float min = 2 * (CrossDeltaTime + LoadDelayTime);
        int roundedMinTimeSwitch = Mathf.RoundToInt(min);
        return roundedMinTimeSwitch - min > 0 ? roundedMinTimeSwitch : roundedMinTimeSwitch + 2;
    }

    private void DetectValueChangeMessage(string message)
    {
        if (oldValue != PlayMode)
        {
            oldValue = PlayMode;
            Debug.LogWarning(message);
        }
    }
#endif 
    #endregion
}