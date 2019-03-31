using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;

namespace GoogleVRWithOpenCVForUnityExample
{
    public class GoogleVRWithOpenCVForUnityExample : MonoBehaviour
    {

        public Text exampleTitle;
        public Text versionInfo;
        public ScrollRect scrollRect;
        static float verticalNormalizedPosition = 1f;

        // Use this for initialization
        IEnumerator Start ()
        {
            yield return StartCoroutine (SwitchTo2D ());            


            exampleTitle.text = "GoogleVRWithOpenCVForUnity Example " + Application.version;

            versionInfo.text = OpenCVForUnity.CoreModule.Core.NATIVE_LIBRARY_NAME + " " + OpenCVForUnity.UnityUtils.Utils.getVersion () + " (" + OpenCVForUnity.CoreModule.Core.VERSION + ")";
            versionInfo.text += " / UnityEditor " + Application.unityVersion;
            versionInfo.text += " / ";

            #if UNITY_EDITOR
            versionInfo.text += "Editor";
            #elif UNITY_STANDALONE_WIN
            versionInfo.text += "Windows";
            #elif UNITY_STANDALONE_OSX
            versionInfo.text += "Mac OSX";
            #elif UNITY_STANDALONE_LINUX
            versionInfo.text += "Linux";
            #elif UNITY_ANDROID
            versionInfo.text += "Android";
            #elif UNITY_IOS
            versionInfo.text += "iOS";
            #elif UNITY_WSA
            versionInfo.text += "WSA";
            #elif UNITY_WEBGL
            versionInfo.text += "WebGL";
            #endif
            versionInfo.text += " ";
            #if ENABLE_MONO
            versionInfo.text += "Mono";
            #elif ENABLE_IL2CPP
            versionInfo.text += "IL2CPP";
            #elif ENABLE_DOTNET
            versionInfo.text += ".NET";
            #endif

            scrollRect.verticalNormalizedPosition = verticalNormalizedPosition;
        }

        // Update is called once per frame
        void Update ()
        {

        }

        public void OnScrollRectValueChanged ()
        {
            verticalNormalizedPosition = scrollRect.verticalNormalizedPosition;
        }

        // Call via `StartCoroutine(SwitchTo2D())` from your code. Or, use
        // `yield SwitchTo2D()` if calling from inside another coroutine.
        IEnumerator SwitchTo2D ()
        {
            // Empty string loads the "None" device.
            XRSettings.LoadDeviceByName ("");

            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;

            // Not needed, since loading the None (`""`) device takes care of this.
            // XRSettings.enabled = false;

            // Restore 2D camera settings.
            ResetCameras ();
        }

        // Resets camera transform and settings on all enabled eye cameras.
        void ResetCameras ()
        {
            // Camera looping logic copied from GvrEditorEmulator.cs
            for (int i = 0; i < Camera.allCameras.Length; i++) {
                Camera cam = Camera.allCameras [i];
                if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None) {

                    // Reset local position.
                    // Only required if you change the camera's local position while in 2D mode.
                    cam.transform.localPosition = Vector3.zero;

                    // Reset local rotation.
                    // Only required if you change the camera's local rotation while in 2D mode.
                    cam.transform.localRotation = Quaternion.identity;

                    // No longer needed, see issue github.com/googlevr/gvr-unity-sdk/issues/628.
                    // cam.ResetAspect();

                    // No need to reset `fieldOfView`, since it's reset automatically.
                }
            }
        }

        public void OnShowLicenseButton ()
        {
            SceneManager.LoadScene ("ShowLicense");
        }

        public void OnShowARMarkerButton ()
        {
            SceneManager.LoadScene ("ShowARMarker");
        }

        public void OnGoogleVRMarkerBasedARExample ()
        {
            SceneManager.LoadScene ("GoogleVRMarkerBasedARExample");
        }
    }
}