#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;

namespace i5.Editor.Versioning
{
    public class BuildIncrement
    {
#if UNITY_EDITOR
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (VersionWindow.VersionInfo == null)
            {
                VersionWindow.VersionInfo = VersionInfo.TryLoad();
            }

            string previousVersion = VersionWindow.VersionInfo.VersionString;

            VersionWindow.VersionInfo.IncrementBuildVersion();
            VersionWindow.VersionInfo.Save();

            Debug.Log("Incremented Build Number: " + previousVersion + " -> " + VersionWindow.VersionInfo.VersionString);
        }
#endif
    }
}