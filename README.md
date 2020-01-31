# Unity-VersionTool

This editor allows Unity developers to set the version of an application for all platforms at once.

## Installation

### Alternative 1: Unity Package Manager with Git (Unity 2018.3 or later)
The tool is available as a package for the Unity Package Manager.
It can be included in new projects by referencing the git-repository on GitHub in the dependency file of the Unity project:

1. Open your project's root folder in a file explorer.
2. Navigate to the `Packages` folder and open the file `manifest.json`.
   It contains a list of package dependencies which are loaded into the project.
3. To add the latest version of the tool to the dependencies, add the following line inside of the `"dependencies"` object:
   ```"com.i5.versiontool": "https://github.com/rwth-acis/Unity-VersionTool.git#upm",```
   After that, Unity will automatically download the package and it is immediately available.

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