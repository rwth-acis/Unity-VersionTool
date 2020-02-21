#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace i5.Editor.Versioning
{
    /// <summary>
    /// Editor window for setting the version
    /// </summary>
    public class VersionWindow
#if UNITY_EDITOR
        : EditorWindow
#endif
    {
#if UNITY_EDITOR
        /// <summary>
        /// Called if the window is requested by the editor so that it can be shown
        /// </summary>
        [MenuItem("Window/Version Settings")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(VersionWindow));
        }

        /// <summary>
        /// Called if the editor requests the GUI of the window
        /// Defines the UI elements of the window
        /// </summary>
        private void OnGUI()
        {
            // create the fields where the version numbers can be entered
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            VersionManager.DataInstance.MajorVersion = EditorGUILayout.IntField("Major Version", VersionManager.DataInstance.MajorVersion);
            VersionManager.DataInstance.MinorVersion = EditorGUILayout.IntField("Minor Version", VersionManager.DataInstance.MinorVersion);
            VersionManager.DataInstance.PatchVersion = EditorGUILayout.IntField("Patch Version", VersionManager.DataInstance.PatchVersion);
            VersionManager.DataInstance.Stage = (VersionStage)EditorGUILayout.EnumPopup("Version status", VersionManager.DataInstance.Stage);
            GUI.enabled = false;
            EditorGUILayout.IntField(new GUIContent("Build Version", "The build version is automatically incremented each time you build the project."), VersionManager.DataInstance.BuildVersion);
            GUI.enabled = true;

            // button for saving the version
            if (GUILayout.Button("Save Version"))
            {
                VersionManager.Save();
            }

            // preview label which shows the version string
            GUILayout.Label("Current version: " + VersionManager.DataInstance.VersionString, EditorStyles.boldLabel);

            // quick buttons for incrementing version parts
            if (GUILayout.Button("Increment Major Version"))
            {
                VersionManager.IncrementMajorVersion();
                VersionManager.Save();
            }

            if (GUILayout.Button("Increment Minor Version"))
            {
                VersionManager.IncrementMinorVersion();
                VersionManager.Save();
            }

            if (GUILayout.Button("Increment Patch Version"))
            {
                VersionManager.IncrementPatchVersion();
                VersionManager.Save();
            }
        }
#endif
    }

    /// <summary>
    /// The stages that the application can have
    /// </summary>
    public enum VersionStage
    {
        Alpha, Beta, RC, Release
    }
}