# Changelog

This file keeps track of the changes between releases of the Unity-VersionTool.

## [Unreleased]
Refactor and optimization of code

### Added
- Check for deprecated save data

### Changes
- Code refactor
- Internal optimizations to the editor window

### Breaking Changes
- save file now contains a packageVersion value
- renamed "stage" to "stageNumber" in save file

## [0.1.2] - 2020-02-21
Update with documentation and unit tests

### Added
- Unit tests
- Documentation
- Short string representation of the version

## [0.1.1] - 2020-02-10
Support for project builds

### Fixes
- fixed "namespace not found"-errors for UnityEditor namespaces when building the project
 
### Changes
- changed output message on build

## [0.1.0] - 2020-01-31
Initial preview release of the Unity-VersionTool
### New Features
- Editor UI window "Version Settings"
- Controls for setting a version number based on the SemVer versioning principle
- Controls for incrementing the version number
- Automatic build number increment if the Unity project is built
- Version numbers are applied to Standalone, iOS, Android and UWP
- Support for Unity packages