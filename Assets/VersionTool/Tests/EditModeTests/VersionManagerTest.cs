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
    public class VersionManagerTest
    {
        private string origBundleVersion;
        private Version origWsaVersion;
        private int origAndroidVersion;
        private string origiOSVersion;

        private int origMajorVersion;
        private int origMinorVersion;
        private int origPatchVersion;
        private VersionStage origStage;
        private int origBuildVersion;

        private string origSaveData;
        private bool saveFileExisted;

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

            origMajorVersion = VersionManager.DataInstance.MajorVersion;
            origMinorVersion = VersionManager.DataInstance.MinorVersion;
            origPatchVersion = VersionManager.DataInstance.PatchVersion;
            origStage = VersionManager.DataInstance.Stage;
            origBuildVersion = VersionManager.DataInstance.BuildVersion;

            saveFileExisted = File.Exists(VersionManager.savePath);
            if (saveFileExisted)
            {
                origSaveData = File.ReadAllText(VersionManager.savePath);
            }

            VersionManager.SetVersion(0, 0, 0, VersionStage.Alpha, 0);
        }

        /// <summary>
        /// Checks that the version properties are updated if a new version is set
        /// </summary>
        [Test]
        public void SetVersionTest()
        {
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            Assert.AreEqual(VersionManager.DataInstance.MajorVersion, 1);
            Assert.AreEqual(VersionManager.DataInstance.MinorVersion, 2);
            Assert.AreEqual(VersionManager.DataInstance.PatchVersion, 3);
            Assert.AreEqual(VersionManager.DataInstance.Stage, VersionStage.Alpha);
            Assert.AreEqual(VersionManager.DataInstance.StageAbbreviation, "a");
            Assert.AreEqual(VersionManager.DataInstance.BuildVersion, 4);
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
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            // check version
            string expectedStringVersion = "1.2.3a4";
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test beta stage
            VersionManager.SetVersion(2, 3, 4, VersionStage.Beta, 5);
            // check version
            expectedStringVersion = "2.3.4b5";
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test release candidate stage
            VersionManager.SetVersion(3, 4, 5, VersionStage.RC, 6);
            // check version
            expectedStringVersion = "3.4.5rc6";
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test finale stage
            VersionManager.SetVersion(4, 5, 6, VersionStage.Release, 7);
            // check version
            expectedStringVersion = "4.5.6f7";
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);
        }

        /// <summary>
        /// Checks that the project's WSA version is applied if the version is set
        /// </summary>
        [Test]
        public void ApplyWSAVersionTest()
        {
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            Version expectedVersion = new Version(1, 2, 3, 4);
            Assert.AreEqual(PlayerSettings.WSA.packageVersion, expectedVersion);
        }

        /// <summary>
        /// Checks that the project's Android version is applied if the version is set
        /// </summary>
        [Test]
        public void ApplyAndroidVersionTest()
        {
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            int expectedVersion = 10203;
            Assert.AreEqual(VersionManager.DataInstance.VersionNumeric, expectedVersion);
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
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            // check version
            string expectedStringVersion = "1.2.3a4";
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test beta stage
            VersionManager.SetVersion(2, 3, 4, VersionStage.Beta, 5);
            // check version
            expectedStringVersion = "2.3.4b5";
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test release candidate stage
            VersionManager.SetVersion(3, 4, 5, VersionStage.RC, 6);
            // check version
            expectedStringVersion = "3.4.5rc6";
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);

            // test finale stage
            VersionManager.SetVersion(4, 5, 6, VersionStage.Release, 7);
            // check version
            expectedStringVersion = "4.5.6f7";
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedStringVersion);
            Assert.AreEqual(PlayerSettings.bundleVersion, expectedStringVersion);
        }

        /// <summary>
        /// Checks that the major increment works as intended
        /// </summary>
        [Test]
        public void IncrementMajorTest()
        {
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            VersionManager.IncrementMajorVersion();
            string expectedVersion = "2.0.0a0";
            Assert.AreEqual(VersionManager.DataInstance.MajorVersion, 2);
            Assert.AreEqual(VersionManager.DataInstance.MinorVersion, 0);
            Assert.AreEqual(VersionManager.DataInstance.PatchVersion, 0);
            Assert.AreEqual(VersionManager.DataInstance.BuildVersion, 0);
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedVersion);
        }

        /// <summary>
        /// Checks that the minor increment works as intended
        /// </summary>
        [Test]
        public void IncrementMinorTest()
        {
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            VersionManager.IncrementMinorVersion();
            string expectedVersion = "1.3.0a0";
            Assert.AreEqual(VersionManager.DataInstance.MajorVersion, 1);
            Assert.AreEqual(VersionManager.DataInstance.MinorVersion, 3);
            Assert.AreEqual(VersionManager.DataInstance.PatchVersion, 0);
            Assert.AreEqual(VersionManager.DataInstance.BuildVersion, 0);
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedVersion);
        }

        /// <summary>
        /// Checks that the patch increment works as intended
        /// </summary>
        [Test]
        public void IncrementPatchTest()
        {
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            VersionManager.IncrementPatchVersion();
            string expectedVersion = "1.2.4a0";
            Assert.AreEqual(VersionManager.DataInstance.MajorVersion, 1);
            Assert.AreEqual(VersionManager.DataInstance.MinorVersion, 2);
            Assert.AreEqual(VersionManager.DataInstance.PatchVersion, 4);
            Assert.AreEqual(VersionManager.DataInstance.BuildVersion, 0);
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedVersion);
        }

        /// <summary>
        /// Checks that the build increment works as intended
        /// </summary>
        [Test]
        public void IncrementBuildTest()
        {
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            VersionManager.IncrementBuildVersion();
            string expectedVersion = "1.2.3a5";
            Assert.AreEqual(VersionManager.DataInstance.MajorVersion, 1);
            Assert.AreEqual(VersionManager.DataInstance.MinorVersion, 2);
            Assert.AreEqual(VersionManager.DataInstance.PatchVersion, 3);
            Assert.AreEqual(VersionManager.DataInstance.BuildVersion, 5);
            Assert.AreEqual(VersionManager.DataInstance.VersionString, expectedVersion);
        }

        /// <summary>
        /// Checks that the save operation works if the save file already exists
        /// </summary>
        [Test]
        public void SaveOverwriteTest()
        {
            File.WriteAllText(VersionManager.savePath, "unit test stub");
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            VersionManager.Save();

            string expectedJsonContent = "{\"packageVersion\":" + VersionManager.DataInstance.PackageVersion + ",\"majorVersion\":1,\"minorVersion\":2,\"patchVersion\":3,\"stageNumber\":0,\"buildVersion\":4}";
            Assert.True(File.Exists(VersionManager.savePath));
            Assert.AreEqual(File.ReadAllText(VersionManager.savePath), expectedJsonContent);
        }

        /// <summary>
        /// Checks that the save operation works if the save file does not exist
        /// </summary>
        [Test]
        public void SaveCreateTest()
        {
            if (File.Exists(VersionManager.savePath))
            {
                File.Delete(VersionManager.savePath);
            }
            VersionManager.SetVersion(1, 2, 3, VersionStage.Alpha, 4);
            VersionManager.Save();

            string expectedJsonContent = "{\"packageVersion\":" + VersionManager.DataInstance.PackageVersion + ",\"majorVersion\":1,\"minorVersion\":2,\"patchVersion\":3,\"stageNumber\":0,\"buildVersion\":4}";
            Assert.True(File.Exists(VersionManager.savePath));
            Assert.AreEqual(File.ReadAllText(VersionManager.savePath), expectedJsonContent);
        }

        /// <summary>
        /// Checks that the load operation works if the save file already exists
        /// </summary>
        [Test]
        public void LoadExistingTest()
        {
            // prepare a save file which corresponds to 1.2.3a4
            File.WriteAllText(VersionManager.savePath, "{\"packageVersion\":200,\"majorVersion\":1,\"minorVersion\":2,\"patchVersion\":3,\"stageNumber\":0,\"buildVersion\":4}");
            // set the version to something different to see if loading works
            VersionManager.SetVersion(0, 0, 0, VersionStage.Alpha, 0);

            VersionManager.TryLoad();
            Assert.AreEqual(VersionManager.DataInstance.PackageVersion, 200);
            Assert.AreEqual(VersionManager.DataInstance.MajorVersion, 1);
            Assert.AreEqual(VersionManager.DataInstance.MinorVersion, 2);
            Assert.AreEqual(VersionManager.DataInstance.PatchVersion, 3);
            Assert.AreEqual(VersionManager.DataInstance.Stage, VersionStage.Alpha);
            Assert.AreEqual(VersionManager.DataInstance.BuildVersion, 4);
            Assert.AreEqual(VersionManager.DataInstance.VersionString, "1.2.3a4");
        }

        /// <summary>
        /// Checks that the load operation returns a default version if the save file does not exist
        /// </summary>
        [Test]
        public void LoadNotExistingTest()
        {
            File.Delete(VersionManager.savePath);
            VersionManager.SetVersion(1, 1, 1, VersionStage.Alpha, 1);

            VersionManager.TryLoad();
            Assert.AreEqual(VersionManager.DataInstance.MajorVersion, 0);
            Assert.AreEqual(VersionManager.DataInstance.MinorVersion, 1);
            Assert.AreEqual(VersionManager.DataInstance.PatchVersion, 0);
            Assert.AreEqual(VersionManager.DataInstance.Stage, VersionStage.Alpha);
            Assert.AreEqual(VersionManager.DataInstance.BuildVersion, 0);
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

            if (saveFileExisted)
            {
                File.WriteAllText(VersionManager.savePath, origSaveData);
            }
            else
            {
                if (File.Exists(VersionManager.savePath))
                {
                    File.Delete(VersionManager.savePath);
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
