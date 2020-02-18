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
        private static int major = 1;
        private static int minor = 0;
        private static int patch = 0;
        private static VersionStage selectedStage = VersionStage.Alpha;

        public static VersionInfo VersionInfo { get; set; }

#if UNITY_EDITOR
        /// <summary>
        /// Called if the window is requested by the editor so that it can be shown
        /// </summary>
        [MenuItem("Window/Version Settings")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(VersionWindow));
            // load the VersionInfo object if it was not yet initialized
            if (VersionInfo == null)
            {
                VersionInfo = VersionInfo.TryLoad();
                SyncWindowWithStored();
            }
        }

        /// <summary>
        /// Called if the editor requests the GUI of the window
        /// Defines the UI elements of the window
        /// </summary>
        private void OnGUI()
        {
            // create the fields where the version numbers can be entered
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            major = EditorGUILayout.IntField("Major Version", major);
            minor = EditorGUILayout.IntField("Minor Version", minor);
            patch = EditorGUILayout.IntField("Patch Version", patch);
            selectedStage = (VersionStage)EditorGUILayout.EnumPopup("Version status", selectedStage);
            GUI.enabled = false;
            EditorGUILayout.IntField(new GUIContent("Build Version", "The build version is automatically incremented each time you build the project."), VersionInfo.BuildVersion);
            GUI.enabled = true;

            // button for saving the version
            if (GUILayout.Button("Save Version"))
            {
                VersionInfo.SetVersion(major, minor, patch, selectedStage);
                Save();
            }

            // preview label which shows the version string
            GUILayout.Label("Current version: " + VersionInfo.VersionString, EditorStyles.boldLabel);

            // quick buttons for incrementing version parts
            if (GUILayout.Button("Increment Major Version"))
            {
                VersionInfo.IncrementMajorVersion();
                Save();
            }

            if (GUILayout.Button("Increment Minor Version"))
            {
                VersionInfo.IncrementMinorVersion();
                Save();
            }

            if (GUILayout.Button("Increment Patch Version"))
            {
                VersionInfo.IncrementPatchVersion();
                Save();
            }
        }

        /// <summary>
        /// Saves the version info
        /// </summary>
        private void Save()
        {
            VersionInfo.Save();
            SyncWindowWithStored();
        }

        /// <summary>
        /// Synchronizes the window's displayed data with the data stored in the VersionInfo object
        /// </summary>
        private static void SyncWindowWithStored()
        {
            major = VersionInfo.MajorVersion;
            minor = VersionInfo.MinorVersion;
            patch = VersionInfo.PatchVersion;
            selectedStage = VersionInfo.Stage;
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