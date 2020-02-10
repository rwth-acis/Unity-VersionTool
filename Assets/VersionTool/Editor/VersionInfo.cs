using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace i5.Editor.Versioning
{
    [Serializable]
    public class VersionInfo
    {
        [SerializeField]
        private int majorVersion;
        [SerializeField]
        private int minorVersion;
        [SerializeField]
        private int patchVersion;
        [SerializeField]
        private int stage;
        [SerializeField]
        private int buildVersion;

        private const string infoPath = "Assets/versionConfig.json";

        public int MajorVersion { get => majorVersion; }

        public int MinorVersion { get => minorVersion; }

        public int PatchVersion { get => patchVersion; }

        public VersionStage Stage { get => (VersionStage)stage; private set => stage = (int)value; }

        public int BuildVersion { get => buildVersion; }

        public string VersionString
        {
            get
            {
                return MajorVersion + "." + MinorVersion + "." + PatchVersion + StageAbbreviation + BuildVersion;
            }
        }

        public int VersionNumeric
        {
            get
            {
                return MajorVersion * 10000 + MinorVersion * 100 + PatchVersion;
                // 1.1.1 = 10101
            }
        }

        public string StageAbbreviation
        {
            get
            {
                switch(Stage)
                {
                    case VersionStage.Alpha:
                        return "a";
                    case VersionStage.Beta:
                        return "b";
                    case VersionStage.RC:
                        return "rc";
                    case VersionStage.Release:
                        return "f";
                    default:
                        Debug.LogError("[Version Window] Unrecognized stage abbreviation");
                        return "-";
                }
            }
        }

        public VersionInfo()
        {
        }

        public VersionInfo(int major, int minor, int patch, VersionStage stage, int build)
        {
            SetVersion(major, minor, patch, stage);
        }

        public void SetVersion(int major, int minor, int patch, VersionStage stage)
        {
            majorVersion = major;
            minorVersion = minor;
            patchVersion = patch;
            Stage = stage;
            buildVersion = 0;

            ApplyVersion();
        }

        private void ApplyVersion()
        {
#if UNITY_EDITOR
            PlayerSettings.bundleVersion = VersionString;
            PlayerSettings.WSA.packageVersion = new Version(MajorVersion, MinorVersion, PatchVersion, BuildVersion);
            PlayerSettings.Android.bundleVersionCode = VersionNumeric;
            PlayerSettings.iOS.buildNumber = VersionString;
#endif
        }

        /// <summary>
        /// Increments the major version and resets the minor, patch and build versions
        /// </summary>
        public void IncrementMajorVersion()
        {
            majorVersion++;
            minorVersion = 0;
            patchVersion = 0;
            buildVersion = 0;

            ApplyVersion();
        }

        /// <summary>
        /// Increments the minor version and resets the patch and build versions
        /// </summary>
        public void IncrementMinorVersion()
        {
            minorVersion++;
            patchVersion = 0;
            buildVersion = 0;

            ApplyVersion();
        }

        /// <summary>
        /// Increments the patch version and resets the build version
        /// </summary>
        public void IncrementPatchVersion()
        {
            patchVersion++;
            buildVersion = 0;

            ApplyVersion();
        }

        /// <summary>
        /// Increments the build version
        /// </summary>
        public void IncrementBuildVersion()
        {
            buildVersion++;

            ApplyVersion();
        }

        /// <summary>
        /// Saves the current version info in the assets file
        /// </summary>
        public void Save()
        {
#if UNITY_EDITOR
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(infoPath, json);
            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// Loads the saved version configuration or returns a default version value if the configuration file does not exist
        /// </summary>
        /// <returns>The saved version configuration or the default version 0.1.0a0 if the configuration file does not exist</returns>
        public static VersionInfo TryLoad()
        {   
            if (File.Exists(infoPath))
            {
                string json = File.ReadAllText(infoPath);
                VersionInfo info = JsonUtility.FromJson<VersionInfo>(json);
                return info;
            }
            else
            {
                // if no settings exist: load a default version info number
                return new VersionInfo(0, 1, 0, VersionStage.Alpha, 0);
            }
        }
    }
}