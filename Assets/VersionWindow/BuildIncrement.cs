using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace i5.Editor.Versioning
{
    public class BuildIncrement : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (VersionWindow.VersionInfo == null)
            {
                VersionWindow.VersionInfo = VersionInfo.TryLoad();
            }

            VersionWindow.VersionInfo.IncrementBuildVersion();
        }
    }
}