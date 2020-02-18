using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace i5.Editor.Versioning
{
    /// <summary>
    /// Object which administers and applies version data for the project
    /// </summary>
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

        /// <summary>
        /// The save path of the serialized information
        /// </summary>
        public const string infoPath = "Assets/versionConfig.json";

        /// <summary>
        /// The major version number which is increased if major breaking changes are made
        /// </summary>
        public int MajorVersion { get => majorVersion; }

        /// <summary>
        /// The minor version number which is increased if minor breaking changes are made
        /// </summary>
        public int MinorVersion { get => minorVersion; }

        /// <summary>
        /// The patch version number which is increased if small backwards compatible changes are made
        /// </summary>
        public int PatchVersion { get => patchVersion; }

        /// <summary>
        /// The stage of the software
        /// </summary>
        public VersionStage Stage { get => (VersionStage)stage; private set => stage = (int)value; }

        /// <summary>
        /// The build version which is increased if the application is built
        /// </summary>
        public int BuildVersion { get => buildVersion; }

        /// <summary>
        /// Gets a version string of the format [major].[minor].[patch][stageLetter][build]
        /// </summary>
        public string VersionString
        {
            get
            {
                return MajorVersion + "." + MinorVersion + "." + PatchVersion + StageAbbreviation + BuildVersion;
            }
        }

        /// <summary>
        /// Gets a numeric representation of the version by concatenating the numbers of major, minor and patch versions
        /// </summary>
        public int VersionNumeric
        {
            get
            {
                return MajorVersion * 10000 + MinorVersion * 100 + PatchVersion;
                // 1.1.1 = 10101
            }
        }

        /// <summary>
        /// Gets a short representation of the version string of the format [major].[minor].[patch]
        /// If the patch version is 0, it is ommited
        /// </summary>
        public string ShortVersionString
        {
            get
            {
                string versionString = MajorVersion + "." + MinorVersion;
                if (PatchVersion != 0)
                {
                    versionString += "." + PatchVersion;
                }
                return versionString;
            }
        }

        /// <summary>
        /// Gets an abbreviation for the stage
        /// </summary>
        public string StageAbbreviation
        {
            get
            {
                switch (Stage)
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

        /// <summary>
        /// Empty constructor for serialization
        /// </summary>
        public VersionInfo()
        {
        }

        /// <summary>
        /// Creates a new version info object with the given version number
        /// </summary>
        /// <param name="major">The major version number</param>
        /// <param name="minor">The minor version number</param>
        /// <param name="patch">The patch version number</param>
        /// <param name="stage">The stage of the application</param>
        /// <param name="build">The build number</param>
        public VersionInfo(int major, int minor, int patch, VersionStage stage, int build = 0)
        {
            SetVersion(major, minor, patch, stage, build);
        }

        /// <summary>
        /// Updates the version and applies it to the project settings
        /// </summary>
        /// <param name="major">The major version number</param>
        /// <param name="minor">The minor version number</param>
        /// <param name="patch">The patch version number</param>
        /// <param name="stage">The stage of the application</param>
        /// <param name="build">The build number</param>
        public void SetVersion(int major, int minor, int patch, VersionStage stage, int build = 0)
        {
            majorVersion = major;
            minorVersion = minor;
            patchVersion = patch;
            Stage = stage;
            buildVersion = build;

            ApplyVersion();
        }

        /// <summary>
        /// Applies the set version to Unity's project settings
        /// Currently supported: standalone, UWP, Android, iOS versions
        /// </summary>
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