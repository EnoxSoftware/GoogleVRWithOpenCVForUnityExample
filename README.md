GoogleVR With OpenCVForUnity Example
====================

Screen Shot
-----
![ScreenShot.jpg](ScreenShot.jpg)


Environment
-----
Windows 8.1  
Unity 5.3.0f4  
OpenCVForUnity 2.1.6  
MarkerBased AR Example 1.2.0  
Google VR SDK for Unity v1.40.0


Setup
-----
* Create New Project. (GoogleVRMarkerBasedARExample)
* Import GoogleVRForUnity.unitypackage  
* Fix a bug in multiple cameras in GVR SDK by applying the codes of the screenshot posted on [this page](https://github.com/googlevr/gvr-unity-sdk/issues/456).  
![screenshot1](https://cloud.githubusercontent.com/assets/22044289/22729389/7f976b3e-ee1d-11e6-9586-0e14ba92e316.png)  
![screenshot2](https://cloud.githubusercontent.com/assets/22044289/22729388/7f963692-ee1d-11e6-8d89-cf143da1c89e.png)  
* Import OpenCVForUnity from AssetStore (if iOS platform, please use "OpenCVForUnity/Extra/ios_exclude_contrib.zip".)  
* Import MarkerBased AR Example from AssetStore  
* Import GoogleVRWithOpenCVForUnityExample.unitypackage 
* Change Product Name. (GoogleVRMarkerBasedARExample)  
* Change PlayerSettings.bundleIdentifier. (xxx.xxxxxxx.googlevrmarkerbasedarexample)  
* Add the “Assets/GoogleVRWithOpenCVForUnityExample/Scenes/*.unity” files to “Scenes In Build” list in “Build Settings” window.
* [iOS]Set In PlayerSettings, at the bottom under Settings for iOS, click Resolution and Presentation to expand that panel. Set the Default Orientation to Auto Rotation, and then uncheck all of the Allowed Orientations for Auto Rotation except for Landscape Left.
* [Android]Remove "Assets/Plugin/Android/gvr_android_common.aar" file from builds. (See http://answers.unity3d.com/questions/1283008/android-unable-to-merge-android-manifest-error.html)
* Build and Deploy.

![ProjectWindow.jpg](ProjectWindow.jpg)  
![MainCamera_Inspector.jpg](MainCamera_Inspector.jpg)  
![GvrAdaptor_Inspector.jpg](GvrAdaptor_Inspector.jpg)  
![GvrMain_Inspector.jpg](GvrMain_Inspector.jpg)  
![GvrMain_Head_Inspector.jpg](GvrMain_Head_Inspector.jpg)  
![ARCamera_Inspector.jpg](ARCamera_Inspector.jpg)  


[Android] .apk file
-----
[GoogleVRWithOpenCVForUnityExample.apk](GoogleVRWithOpenCVForUnityExample.apk)


AR Marker
-----
![marker.png](marker.png) 

