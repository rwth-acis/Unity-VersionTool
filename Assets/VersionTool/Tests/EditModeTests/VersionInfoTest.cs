using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using i5.Editor.Versioning;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /// <summary>
    /// Tests the features of the version info class
    /// </summary>
    public class VersionInfoTest
    {
        private string origBundleVersion;
        private Version origWsaVersion;
        private int origAndroidVersion;
        private string origiOSVersion;

        private string saveData;

        private VersionInfo versionInfo;

        /// <summary>
        /// Store all original data so that they can be restored later
        /// </summary>
        [SetUp]
        public void Setup()
        {
            origBundleVersion = PlayerSettings.bundleVersion;
            origWsaVersion = PlayerSettings.WSA.packageVersion;
            origAndroidVersion = PlayerSettings.Android.bundleVersionCode;
            origiOSVersion = PlayerSettings.iOS.buildNumber;

            if (File.Exists(VersionInfo.infoPath))
            {
                saveData = File.ReadAllText(VersionInfo.infoPath);
            }

            versionInfo = new VersionInfo(1, 2, 3, VersionStage.Alpha, 4);
        }

        /// <summary>
        /// Checks that the version properties are updated if a new version is set
        /// </summary>
        [Test]
        public void SetVersionTest()
        {
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            Assert.AreEqual(versionInfo.MajorVersion, 1);
            Assert.AreEqual(versionInfo.MinorVersion, 2);
            Assert.AreEqual(versionInfo.PatchVersion, 3);
            Assert.AreEqual(versionInfo.Stage, VersionStage.Alpha);
            Assert.AreEqual(versionInfo.StageAbbreviation, "a");
            Assert.AreEqual(versionInfo.BuildVersion, 4);
        }

        /// <summary>
        /// Checks that the Unity project's bundle version is updated by setting the version
        /// Tests run for all four stage versions
        /// </summary>
        [Test]
        public void ApplyBundleVersionTest()
        {
            // test alpha stage
            // set version
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            // check version
            string expectedStringVersion = "1.2.3a4";
            Assert.AreEqual(versionInfo.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test beta stage
            versionInfo.SetVersion(2, 3, 4, VersionStage.Beta, 5);
            // check version
            expectedStringVersion = "2.3.4b5";
            Assert.AreEqual(versionInfo.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test release candidate stage
            versionInfo.SetVersion(3, 4, 5, VersionStage.RC, 6);
            // check version
            expectedStringVersion = "3.4.5rc6";
            Assert.AreEqual(versionInfo.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test finale stage
            versionInfo.SetVersion(4, 5, 6, VersionStage.Release, 7);
            // check version
            expectedStringVersion = "4.5.6f7";
            Assert.AreEqual(versionInfo.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);
        }

        /// <summary>
        /// Checks that the project's WSA version is applied if the version is set
        /// </summary>
        [Test]
        public void ApplyWSAVersionTest()
        {
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            Version expectedVersion = new Version(1, 2, 3, 4);
            Assert.AreEqual(PlayerSettings.WSA.packageVersion, expectedVersion);
        }

        /// <summary>
        /// Checks that the project's Android version is applied if the version is set
        /// </summary>
        [Test]
        public void ApplyAndroidVersionTest()
        {
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            int expectedVersion = 10203;
            Assert.AreEqual(versionInfo.VersionNumeric, expectedVersion);
            Assert.AreEqual(PlayerSettings.Android.bundleVersionCode, expectedVersion);
        }

        /// <summary>
        /// Checks that the project's iOS version is applied if the version is set
        /// </summary>
        [Test]
        public void ApplyiOSVersionTest()
        {
            // test alpha stage
            // set version
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            // check version
            string expectedStringVersion = "1.2.3a4";
            Assert.AreEqual(versionInfo.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test beta stage
            versionInfo.SetVersion(2, 3, 4, VersionStage.Beta, 5);
            // check version
            expectedStringVersion = "2.3.4b5";
            Assert.AreEqual(versionInfo.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test release candidate stage
            versionInfo.SetVersion(3, 4, 5, VersionStage.RC, 6);
            // check version
            expectedStringVersion = "3.4.5rc6";
            Assert.AreEqual(versionInfo.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test finale stage
            versionInfo.SetVersion(4, 5, 6, VersionStage.Release, 7);
            // check version
            expectedStringVersion = "4.5.6f7";
            Assert.AreEqual(versionInfo.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);
        }

        /// <summary>
        /// Checks that the major increment works as intended
        /// </summary>
        [Test]
        public void IncrementMajorTest()
        {
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            versionInfo.IncrementMajorVersion();
            string expectedVersion = "2.0.0a0";
            Assert.AreEqual(versionInfo.MajorVersion, 2);
            Assert.AreEqual(versionInfo.MinorVersion, 0);
            Assert.AreEqual(versionInfo.PatchVersion, 0);
            Assert.AreEqual(versionInfo.BuildVersion, 0);
            Assert.AreEqual(versionInfo.VersionString, expectedVersion);
        }

        /// <summary>
        /// Checks that the minor increment works as intended
        /// </summary>
        [Test]
        public void IncrementMinorTest()
        {
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            versionInfo.IncrementMinorVersion();
            string expectedVersion = "1.3.0a0";
            Assert.AreEqual(versionInfo.MajorVersion, 1);
            Assert.AreEqual(versionInfo.MinorVersion, 3);
            Assert.AreEqual(versionInfo.PatchVersion, 0);
            Assert.AreEqual(versionInfo.BuildVersion, 0);
            Assert.AreEqual(versionInfo.VersionString, expectedVersion);
        }

        /// <summary>
        /// Checks that the patch increment works as intended
        /// </summary>
        [Test]
        public void IncrementPatchTest()
        {
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            versionInfo.IncrementPatchVersion();
            string expectedVersion = "1.2.4a0";
            Assert.AreEqual(versionInfo.MajorVersion, 1);
            Assert.AreEqual(versionInfo.MinorVersion, 2);
            Assert.AreEqual(versionInfo.PatchVersion, 4);
            Assert.AreEqual(versionInfo.BuildVersion, 0);
            Assert.AreEqual(versionInfo.VersionString, expectedVersion);
        }

        /// <summary>
        /// Checks that the build increment works as intended
        /// </summary>
        [Test]
        public void IncrementBuildTest()
        {
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            versionInfo.IncrementBuildVersion();
            string expectedVersion = "1.2.3a5";
            Assert.AreEqual(versionInfo.MajorVersion, 1);
            Assert.AreEqual(versionInfo.MinorVersion, 2);
            Assert.AreEqual(versionInfo.PatchVersion, 3);
            Assert.AreEqual(versionInfo.BuildVersion, 5);
            Assert.AreEqual(versionInfo.VersionString, expectedVersion);
        }

        /// <summary>
        /// Checks that the save operation works if the save file already exists
        /// </summary>
        [Test]
        public void SaveOverwriteTest()
        {
            File.WriteAllText(VersionInfo.infoPath, "unit test stub");
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            versionInfo.Save();

            string expectedJsonContent = "{\"majorVersion\":1,\"minorVersion\":2,\"patchVersion\":3,\"stage\":0,\"buildVersion\":4}";
            Assert.True(File.Exists(VersionInfo.infoPath));
            Assert.AreEqual(File.ReadAllText(VersionInfo.infoPath), expectedJsonContent);
        }

        /// <summary>
        /// Checks that the save operation works if the save file does not exist
        /// </summary>
        [Test]
        public void SaveCreateTest()
        {
            if (File.Exists(VersionInfo.infoPath))
            {
                File.Delete(VersionInfo.infoPath);
            }
            versionInfo.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            versionInfo.Save();

            string expectedJsonContent = "{\"majorVersion\":1,\"minorVersion\":2,\"patchVersion\":3,\"stage\":0,\"buildVersion\":4}";
            Assert.True(File.Exists(VersionInfo.infoPath));
            Assert.AreEqual(File.ReadAllText(VersionInfo.infoPath), expectedJsonContent);
        }

        /// <summary>
        /// Checks that the load operation works if the save file already exists
        /// </summary>
        [Test]
        public void LoadExistingTest()
        {
            // prepare a save file which corresponds to 1.2.3a4
            File.WriteAllText(VersionInfo.infoPath, "{\"majorVersion\":1,\"minorVersion\":2,\"patchVersion\":3,\"stage\":0,\"buildVersion\":4}");
            // set the version to something different to see if loading works
            versionInfo.SetVersion(0, 0, 0, VersionStage.Alpha, 0);

            versionInfo = VersionInfo.TryLoad();
            Assert.AreEqual(versionInfo.MajorVersion, 1);
            Assert.AreEqual(versionInfo.MinorVersion, 2);
            Assert.AreEqual(versionInfo.PatchVersion, 3);
            Assert.AreEqual(versionInfo.Stage, VersionStage.Alpha);
            Assert.AreEqual(versionInfo.BuildVersion, 4);
            Assert.AreEqual(versionInfo.VersionString, "1.2.3a4");
        }

        /// <summary>
        /// Checks that the load operation returns a default version if the save file does not exist
        /// </summary>
        [Test]
        public void LoadNotExistingTest()
        {
            File.Delete(VersionInfo.infoPath);
            versionInfo.SetVersion(1, 1, 1, VersionStage.Alpha, 1);

            versionInfo = VersionInfo.TryLoad();
            Assert.AreEqual(versionInfo.MajorVersion, 0);
            Assert.AreEqual(versionInfo.MinorVersion, 1);
            Assert.AreEqual(versionInfo.PatchVersion, 0);
            Assert.AreEqual(versionInfo.Stage, VersionStage.Alpha);
            Assert.AreEqual(versionInfo.BuildVersion, 0);
        }

        /// <summary>
        /// Restores all values and save files to the project's original settings
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            // reset the versions to their original values
            PlayerSettings.bundleVersion = origBundleVersion;
            PlayerSettings.WSA.packageVersion = origWsaVersion;
            PlayerSettings.Android.bundleVersionCode = origAndroidVersion;
            PlayerSettings.iOS.buildNumber = origiOSVersion;

            if (!string.IsNullOrEmpty(saveData))
            {
                File.WriteAllText(VersionInfo.infoPath, saveData);
                AssetDatabase.Refresh();
            }
        }
    }
}
