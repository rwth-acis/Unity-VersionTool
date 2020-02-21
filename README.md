# Unity-VersionTool

This editor allows Unity developers to set the version of an application for all platforms at once.

## Installation

### Alternative 1: Unity Dependency File with Git (Unity 2018.3 or later)
The tool is available as a package for the Unity Package Manager.
It can be included in new projects by referencing the git-repository on GitHub in the dependency file of the Unity project:

1. Open your project's root folder in a file explorer.
2. Navigate to the `Packages` folder and open the file `manifest.json`.
   It contains a list of package dependencies which are loaded into the project.
3. To add a specific version of the tool to the dependencies, add the following line inside of the `"dependencies"` object and replace `[version]` with the release number.
   To receive the latest version, replace `[release]` with `upm`.
   ```"com.i5.versiontool": "https://github.com/rwth-acis/Unity-VersionTool.git#[release]",```
   After that, Unity will automatically download the package and it is immediately available.

### Alternative 2: Unity Package Manager UI with Git (Unity 2019.3 or later)
The package can be downloaded from a git-repository in the package manager's UI.
1. In Unity, go to `Window > Package Manger`.
2. Click on the plus-button in the top left corner of the package manager and select "add".
3. Enter `https://github.com/rwth-acis/Unity-VersionTool.git#[release]` into the text field where `[release]` is replaced with the release number or `upm` for the latest version.
   Confirm the download by clicking on the "add" button.

### Alternative 3: Import custom package (Unity 2017 or later)
Another option is to import the package as a .unitypackage.

1. Download the .unitypackage-file which is supplied with the corresponding release on the [releases page](https://github.com/rwth-acis/Unity-VersionTool/releases).
2. With your project opened, perform a right-click on the assets browser in Unity.
   Select "Import Package > Custom Packge" from the context menu.
3. Navigate to the path where you downloaded the .unitypackage-file, select it and confirm by clicking the "Open" buttom
4. A dialog window opens where you can select which files should be imported.
   Select everything and click on "Import".

*Important for alternative 3*: If you are updating from an earlier version, it is recommended to delete the existing "VersionTool" folder.
After that, import the new package.

## Usage
Once the package has been imported, a new menu entry `Window > Version Settings` is available.
It opens a window where the version number can be selected.

There are two options to set the version:
1. Enter the major, minor and patch numbers, as well as the production status, with the text fields and dropdown menu.
To apply the version number, click the button "Save Version".
The currently saved version number is displayed in the window.

2. The major, minor and patch version can also be incremented quickly by pressing the corresponding button on the menu.

The build number is not editable since it is automatically increased each time the project is compiled.

## Project Structure

Dowloadable versions of the package are pushed to the *upm* branch.
This branch should not be modified manually.
Instead, git subtree is used to push the contents of the VersionTools folder in the assets to this branch.
One can push to this branch by executing the *deploy.bat* script.

The master branch contains an example project where the tool is developed.
Anything that should be included in the tool must be placed in the *VersionTool* folder in the assets.