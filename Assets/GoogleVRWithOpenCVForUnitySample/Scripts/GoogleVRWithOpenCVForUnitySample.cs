using UnityEngine;
using System.Collections;

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

namespace GoogleVRWithOpenCVForUnitySample
{
    public class GoogleVRWithOpenCVForUnitySample : MonoBehaviour
    {
        // Use this for initialization
        void Start ()
        {
            
        }

        // Update is called once per frame
        void Update ()
        {

        }

        public void OnShowLicenseButton ()
        {
            #if UNITY_5_3 || UNITY_5_3_OR_NEWER
            SceneManager.LoadScene ("ShowLicense");
            #else
            Application.LoadLevel ("ShowLicense");
            #endif
        }

        public void OnShowARMarkerButton ()
        {
            
            #if UNITY_5_3 || UNITY_5_3_OR_NEWER
            SceneManager.LoadScene ("ShowARMarker");
            #else
            Application.LoadLevel ("ShowARMarker");
            #endif
        }

        public void OnGoogleVRMarkerBasedARSample ()
        {
            #if UNITY_5_3 || UNITY_5_3_OR_NEWER
            SceneManager.LoadScene ("GoogleVRMarkerBasedARSample");
            #else
            Application.LoadLevel ("GoogleVRMarkerBasedARSample");
            #endif
        }
    }
}