# AR Flatpack Furniture Assembly

* The source code for the instructions system is in the `src` directory.
* This is used in conjunction with a Unity scene which can also be accessed in the `src` directory.
* Alternatively this GitHub repository (https://github.com/pauldoh21/FlatpackAssembly.git) can be set as a source for a Unity project and opened that way.
* This application relies on the VisionLib SDK which requires a license to use.
* This means that without the correct license file, it will not build.
* The license which should be provided in the Unity scene will not work on any other devices but the device it was built on.
* Make sure that the project is set up correctly to build (This official guide shows the correct project set up to build to the Hololens 2: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/unity-development-overview?tabs=arr%2CD365%2Chl2).

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