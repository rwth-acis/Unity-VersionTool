using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace i5.Editor.Versioning
{
    public class VersionWindow : EditorWindow
    {
        private static int major = 1;
        private static int minor = 0;
        private static int patch = 0;
        private static VersionStage selectedStage = VersionStage.Alpha;

        public static VersionInfo VersionInfo { get; set; }

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

        private void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            major = EditorGUILayout.IntField("Major Version", major);
            minor = EditorGUILayout.IntField("Minor Version", minor);
            patch = EditorGUILayout.IntField("Patch Version", patch);
            selectedStage = (VersionStage)EditorGUILayout.EnumPopup("Version status", selectedStage);
            GUI.enabled = false;
            EditorGUILayout.IntField(new GUIContent("Build Version", "The build version is automatically incremented each time you build the project."), VersionInfo.BuildVersion);
            GUI.enabled = true;

            if (GUILayout.Button("Save Version"))
            {
                VersionInfo.SetVersion(major, minor, patch, selectedStage);
                Save();
            }

            GUILayout.Label("Current version: " + VersionInfo.VersionString, EditorStyles.boldLabel);

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

        private void Save()
        {
            VersionInfo.Save();
            SyncWindowWithStored();
        }

        private static void SyncWindowWithStored()
        {
            major = VersionInfo.MajorVersion;
            minor = VersionInfo.MinorVersion;
            patch = VersionInfo.PatchVersion;
            selectedStage = VersionInfo.Stage;
        }
    }

    public enum VersionStage
    {
        Alpha, Beta, RC, Release
    }
}