#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;

namespace i5.Editor.Versioning
{
    /// <summary>
    /// Editor script which increases the build version every time the application is built
    /// </summary>
    public class BuildIncrement
    {
#if UNITY_EDITOR
        /// <summary>
        /// Called by Unity after a build was completed
        /// Increments the version info
        /// </summary>
        /// <param name="target">The target of the build</param>
        /// <param name="pathToBuiltProject">The path where the project was built</param>
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