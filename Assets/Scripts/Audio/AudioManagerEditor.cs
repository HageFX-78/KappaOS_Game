using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;

[System.Serializable]
public class AudioGroupWrapper
{
    public string groupName;
    [NonReorderable]
    public List<Sound> groupAudio = new List<Sound>();
}
#if UNITY_EDITOR
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AudioManager am = (AudioManager)target;
        if (GUILayout.Button("GetAudioFiles"))
        {
            if(!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                Debug.Log("<color=##ffc31f>AudioManager - </color>No Resources Folder found ... Creating Resources Folder");
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            //Create directory if there is no SFX or BGM folder
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Audio"))
            {
                Debug.Log("<color=##ffc31f>AudioManager - </color>No Audio Folder found ... Creating Audio Folder");
                AssetDatabase.CreateFolder("Assets/Resources", "Audio");
                return;
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Audio/BGM"))
            {
                Debug.Log("<color=##ffc31f>AudioManager - </color>No BGM Folder found ... Creating BGM Folder");
                AssetDatabase.CreateFolder("Assets/Resources/Audio", "BGM");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Audio/SFX"))
            {
                Debug.Log("<color=##ffc31f>AudioManager - </color>No SFX Folder found ... Creating SFX Folder");
                AssetDatabase.CreateFolder("Assets/Resources/Audio", "SFX");
            }


            //Clear all current audio list
            am.BGM.Clear();
            am.SFX.Clear();

            //Get all audio files from the directory
            AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");
            foreach(AudioClip audioClip in bgmClips)
            {
                Sound s = new Sound();
                s.name = audioClip.name;
                s.clip = audioClip;
                am.BGM.Add(s);
            }
                // Get all audio files from the directory with category grouping
            string sfxFolderPath = "Assets/Resources/Audio/SFX";
            string[] sfxSubfolders = AssetDatabase.GetSubFolders(sfxFolderPath);
            
            foreach (string subfolder in sfxSubfolders)
            {
                string categoryName = Path.GetFileName(subfolder);
                
                // Create a new audio group for each category
                AudioGroupWrapper audioGroup = new AudioGroupWrapper();
                audioGroup.groupName = categoryName;
                am.SFX.Add(audioGroup);

                // Load audio clips from the subfolder
                string relativeFolderPath = "Audio/SFX/" + categoryName;
                AudioClip[] sfxClips = Resources.LoadAll<AudioClip>(relativeFolderPath);

                foreach (AudioClip audioClip in sfxClips)
                {
                    Sound s = new Sound();
                    s.name = audioClip.name;
                    s.clip = audioClip;
                    audioGroup.groupAudio.Add(s);
                }
            }

            // Add audio clips directly in the SFX folder to "General" group
            // Get all audio files from the SFX folder (including subfolders)
            string[] allSfxClipPaths = AssetDatabase.FindAssets("t:AudioClip", new[] { sfxFolderPath });
            HashSet<string> subfolderPaths = new HashSet<string>(sfxSubfolders.Select(f => "Assets/Resources/Audio/SFX/" + Path.GetFileName(f)));

            // Check for general group, create if not found
            AudioGroupWrapper generalGroup;
            if (am.SFX.Find(g => g.groupName == "General") == null)
            {
                generalGroup = new AudioGroupWrapper();
                generalGroup.groupName = "General";
                am.SFX.Add(generalGroup);
            }
            generalGroup = am.SFX.Find(g => g.groupName == "General");

            // Add audio clips to the general group
            foreach (string clipPath in allSfxClipPaths)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(clipPath);
                if (!subfolderPaths.Any(f => assetPath.StartsWith(f)))
                {
                    AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
                    if (audioClip != null)
                    {
                        Sound s = new Sound();
                        s.name = audioClip.name;
                        s.clip = audioClip;
                        generalGroup.groupAudio.Add(s);
                    }
                }
            }
        }
    }
}
#endif
