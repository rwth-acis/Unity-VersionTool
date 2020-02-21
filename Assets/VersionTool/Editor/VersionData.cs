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
    public class VersionData
    {
        [SerializeField]
        private int packageVersion;
        [SerializeField]
        private int majorVersion;
        [SerializeField]
        private int minorVersion;
        [SerializeField]
        private int patchVersion;
        [SerializeField]
        private int stageNumber;
        [SerializeField]
        private int buildVersion;

        public int PackageVersion { get => packageVersion; }

        /// <summary>
        /// The major version number which is increased if major breaking changes are made
        /// </summary>
        public int MajorVersion { get => majorVersion; set => majorVersion = value; }

        /// <summary>
        /// The minor version number which is increased if minor breaking changes are made
        /// </summary>
        public int MinorVersion { get => minorVersion; set => minorVersion = value; }

        /// <summary>
        /// The patch version number which is increased if small backwards compatible changes are made
        /// </summary>
        public int PatchVersion { get => patchVersion; set => patchVersion = value; }

        /// <summary>
        /// The stage of the software
        /// </summary>
        public VersionStage Stage { get => (VersionStage)stageNumber; set => stageNumber = (int)value; }

        /// <summary>
        /// The build version which is increased if the application is built
        /// </summary>
        public int BuildVersion { get => buildVersion; set => buildVersion = value; }

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

        public int StageNumeric
        {
            get
            {
                return stageNumber;
            }
            set
            {
                stageNumber = value;
            }
        }

        /// <summary>
        /// Empty constructor for serialization
        /// </summary>
        public VersionData()
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
        public VersionData(int packageVersion, int major, int minor, int patch, VersionStage stage, int build = 0)
        {
            this.packageVersion = packageVersion;
            this.majorVersion = major;
            this.minorVersion = minor;
            this.patchVersion = patch;
            this.Stage = stage;
            this.buildVersion = build;
        }
    }
}