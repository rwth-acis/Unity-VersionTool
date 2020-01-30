using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace i5.Editor.Versioning
{
    public class BuildIncrement
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            Debug.Log("Post Build");
            if (VersionWindow.VersionInfo == null)
            {
                VersionWindow.VersionInfo = VersionInfo.TryLoad();
            }

            VersionWindow.VersionInfo.IncrementBuildVersion();
            VersionWindow.VersionInfo.Save();
        }
    }
}