using UnityEngine;
using UnityEditor;

//Give a possibility to test the AudioEvent.Play() in Editor w/o Play mode
//In Play mode this possibility not active "!EditorApplication.isPlayingOrWillChangePlaymode"
[CustomEditor(typeof(AudioEvent),true)]
public class AudioEventEditor : Editor
{
    AudioSource _previewAudioSource;
    GameObject _tempGameObject;
    bool initButton;

    private void OnDisable()
    {
        //To exclude the attemp to delete the temp GameObject in the process of stop the Play mode
        if (_tempGameObject)
        {
            //Debug.Log($"DestroyImmediate {_tempGameObject.name} {_tempGameObject.activeInHierarchy}");
            if (_previewAudioSource)
            {
                _previewAudioSource.Stop();
                _previewAudioSource = null;
                //Debug.Log($"OnDisable() _previewAudioSource was != null");
            }
            DestroyImmediate(_tempGameObject);

        }
    }

    private void OnEnable()
    {
        initButton = true;
    }

    private void CreateTempAudioSource()
    {
		_tempGameObject = EditorUtility.CreateGameObjectWithHideFlags("Preview" + target.name, HideFlags.HideInHierarchy, typeof(AudioSource));
		_previewAudioSource = _tempGameObject.GetComponent<AudioSource>();
		//Debug.Log($"{target.name} {_previewAudioSource!=null}");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
		//To exclude the possibility to create the temp GameObject in the process of run the Play mode
        EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
        if (GUILayout.Button("Test Audio Event"))
        {
            if (!_tempGameObject) CreateTempAudioSource();
            ((AudioEvent)target).PlayClipNext(_previewAudioSource);
        }
        EditorGUI.EndDisabledGroup();
    }
}
