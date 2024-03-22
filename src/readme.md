# Readme

* This is the source code for the instructions system
* This is to be used in conjunction with the Unity scene also stored in this directory.
* Alternatively this GitHub repository (https://github.com/pauldoh21/FlatpackAssembly.git) can be set as a source for a Unity project and opened that way.
* This application relies on the VisionLib SDK which requires a license to use.
* This means that without the correct license file, it will not build.
* The license which should be provided in the Unity scene will not work on any other devices but the device it was built on.
* Make sure that the project is set up correctly to build (This official guide shows the correct project set up to build to the Hololens 2: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/unity-development-overview?tabs=arr%2CD365%2Chl2).
* Required Unity version is 2021.3.13f1

* To use you must import these Unity Packages:

    - QuickOutline
    - VisionLib
    - VisionLib Hololens
    - ARFoundation
    - TextMeshPro
    - Mixed Reality Feature Tool - Open XR Plugin
    - Mixed Reality Feature Tool - Mixed Reality Toolkit Foundation
    - Mixed Reality Feature Tool - Mixed Reality Toolkit Standard Assets
    - XR Interaction Toolkit package

* VisionLib requires Windows 10 to build to Hololens 2
* Guide to build application to the Hololens 2:

    - Build the project using the build settings window in Unity
    - Open the .sln file from the build in Visual Studio
    - Set the build configuration to Release and ARM64
    - Build to the device or alternatively use a remote connection

* The application can also be previewed within the Unity editor without any AR or tracking capabilities (this also may not work without a VisionLib license).