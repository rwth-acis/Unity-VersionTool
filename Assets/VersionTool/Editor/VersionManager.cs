using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace i5.Editor.Versioning
{
    public static class VersionManager
    {
        /// <summary>
        /// The save path of the serialized information
        /// </summary>
        public const string savePath = "Assets/versionConfig.json";

        public const int minimumPackageVersion = 00200; // compatible to version 0.2.0 and upwards

        private static VersionData dataInstance;

        public static VersionData DataInstance
        {
            get
            {
                if (dataInstance == null)
                {
                    TryLoad();
                }
                return dataInstance;
            }
        }

        /// <summary>
        /// Updates the version and applies it to the project settings
        /// </summary>
        /// <param name="major">The major version number</param>
        /// <param name="minor">The minor version number</param>
        /// <param name="patch">The patch version number</param>
        /// <param name="stage">The stage of the application</param>
        /// <param name="build">The build number</param>
        public static void SetVersion(int major, int minor, int patch, VersionStage stage, int build = 0)
        {
            DataInstance.MajorVersion = major;
            DataInstance.MinorVersion = minor;
            DataInstance.PatchVersion = patch;
            DataInstance.Stage = stage;
            DataInstance.BuildVersion = build;

            ApplyVersion();
        }

        /// <summary>
        /// Applies the set version to Unity's project settings
        /// Currently supported: standalone, UWP, Android, iOS versions
        /// </summary>
        private static void ApplyVersion()
        {
#if UNITY_EDITOR
            PlayerSettings.bundleVersion = DataInstance.VersionString;
            PlayerSettings.WSA.packageVersion = new Version(DataInstance.MajorVersion, DataInstance.MinorVersion, DataInstance.PatchVersion, DataInstance.BuildVersion);
            PlayerSettings.Android.bundleVersionCode = DataInstance.VersionNumeric;
            PlayerSettings.iOS.buildNumber = DataInstance.VersionString;
#endif
        }

        /// <summary>
        /// Increments the major version and resets the minor, patch and build versions
        /// </summary>
        public static void IncrementMajorVersion()
        {
            DataInstance.MajorVersion++;
            DataInstance.MinorVersion = 0;
            DataInstance.PatchVersion = 0;
            DataInstance.BuildVersion = 0;

            ApplyVersion();
        }

        /// <summary>
        /// Increments the minor version and resets the patch and build versions
        /// </summary>
        public static void IncrementMinorVersion()
        {
            DataInstance.MinorVersion++;
            DataInstance.PatchVersion = 0;
            DataInstance.BuildVersion = 0;

            ApplyVersion();
        }

        /// <summary>
        /// Increments the patch version and resets the build version
        /// </summary>
        public static void IncrementPatchVersion()
        {
            DataInstance.PatchVersion++;
            DataInstance.BuildVersion = 0;

            ApplyVersion();
        }

        /// <summary>
        /// Increments the build version
        /// </summary>
        public static void IncrementBuildVersion()
        {
            DataInstance.BuildVersion++;

            ApplyVersion();
        }

        /// <summary>
        /// Saves the current version info in the assets file
        /// </summary>
        public static void Save()
        {
#if UNITY_EDITOR
            string json = JsonUtility.ToJson(DataInstance);
            File.WriteAllText(savePath, json);
            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// Loads the saved version configuration or returns a default version value if the configuration file does not exist
        /// </summary>
        /// <returns>The saved version configuration or the default version 0.1.0a0 if the configuration file does not exist</returns>
        public static bool TryLoad()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                VersionData loadedData = JsonUtility.FromJson<VersionData>(json);
                if (loadedData.PackageVersion >= minimumPackageVersion)
                {
                    dataInstance = loadedData;
                    return true;
                }
                else
                {
                    Debug.LogError("Incompatible save data of version manager. Revert to a previous version of the VersionTool package.");
                    return false;
                }
            }
            else
            {
                // if no settings exist: load a default version info number
                dataInstance = new VersionData(minimumPackageVersion, 0, 1, 0, VersionStage.Alpha, 0);
                return false;
            }
        }
    }
}