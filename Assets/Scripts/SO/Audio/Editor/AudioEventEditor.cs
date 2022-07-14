using UnityEngine;
using UnityEditor;

//Give a possibility to test the AudioEvent.Play() in Editor w/o Play mode
//In Play mode this possibility not active "!EditorApplication.isPlayingOrWillChangePlaymode"
[CustomEditor(typeof(AudioEvent),true)]
public class AudioEventEditor : Editor
{
    AudioSource _previewAudioSource;
    GameObject _tempCreated;

    private void OnDisable()
    {
        //To exclude the attemp to delete the temp GameObject in the process of stop the Play mode
        if (_tempCreated)
        {
            //Debug.Log($"DestroyImmediate {_tempCreated.activeInHierarchy}");
            DestroyImmediate(_tempCreated); 
        }
    }

    private void OnEnable()
    {
        //To exclude the possibility to create the temp GameObject in the process of run the Play mode
        if (!EditorApplication.isPlayingOrWillChangePlaymode && !_tempCreated)
        {
            _tempCreated = EditorUtility.CreateGameObjectWithHideFlags("Preview"+ target.name, HideFlags.HideInHierarchy, typeof(AudioSource));
            _previewAudioSource = _tempCreated.GetComponent<AudioSource>();
            //Debug.Log($"{target.name} {_previewAudioSource!=null}");
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(!_previewAudioSource);
        if (GUILayout.Button("Test Audio Event"))
        {
            ((AudioEvent)target).PlayClipNext(_previewAudioSource);
        }
        EditorGUI.EndDisabledGroup();
    }
}