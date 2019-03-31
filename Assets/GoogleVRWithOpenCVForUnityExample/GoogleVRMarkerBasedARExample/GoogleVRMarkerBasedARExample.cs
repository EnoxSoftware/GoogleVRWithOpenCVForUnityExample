using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using OpenCVMarkerBasedAR;
using MarkerBasedARExample;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.Calib3dModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.XR;
using System;

namespace GoogleVRWithOpenCVForUnityExample
{
    /// <summary>
    /// GoogleVR With OpenCVForUnity Example.
    /// </summary>
    [RequireComponent (typeof(WebCamTextureToMatHelper))]
    public class GoogleVRMarkerBasedARExample : MonoBehaviour
    {
        /// <summary>
        /// The screen Quad.
        /// </summary>
        public Transform ScreenQuad;

        /// <summary>
        /// The Quad camera.
        /// </summary>
        public Camera QuadCamera;

        /// <summary>
        /// The AR camera.
        /// </summary>
        public Camera ARCamera;

        /// <summary>
        /// The marker settings.
        /// </summary>
        public MarkerSettings[] markerSettings;

        /// <summary>
        /// The texture.
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The cam matrix.
        /// </summary>
        Mat camMatrix;

        /// <summary>
        /// The dist coeffs.
        /// </summary>
        MatOfDouble distCoeffs;

        /// <summary>
        /// The marker detector.
        /// </summary>
        MarkerDetector markerDetector;

        /// <summary>
        /// The invert Y.
        /// </summary>
        Matrix4x4 invertYM;

        /// <summary>
        /// The transformation m.
        /// </summary>
        Matrix4x4 transformationM;

        /// <summary>
        /// The invert Z.
        /// </summary>
        Matrix4x4 invertZM;

        /// <summary>
        /// The ar m.
        /// </summary>
        Matrix4x4 ARM;

        /// <summary>
        /// The web cam texture to mat helper.
        /// </summary>
        WebCamTextureToMatHelper webCamTextureToMatHelper;

        // Use this for initialization
        IEnumerator Start ()
        {
            yield return StartCoroutine (SwitchToVR ());


            webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper> ();

            #if UNITY_ANDROID && !UNITY_EDITOR
            // Avoids the front camera low light issue that occurs in only some Android devices (e.g. Google Pixel, Pixel2).
            webCamTextureToMatHelper.avoidAndroidFrontCameraLowLightIssue = true;
            #endif
            webCamTextureToMatHelper.Initialize ();
        }

        // Call via `StartCoroutine(SwitchToVR())` from your code. Or, use
        // `yield SwitchToVR()` if calling from inside another coroutine.
        IEnumerator SwitchToVR ()
        {
            // Device names are lowercase, as returned by `XRSettings.supportedDevices`.
            string desiredDevice = "cardboard"; // Or "daydream".

            // Some VR Devices do not support reloading when already active, see
            // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
            if (String.Compare (XRSettings.loadedDeviceName, desiredDevice, true) != 0) {
                XRSettings.LoadDeviceByName (desiredDevice);

                // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
                yield return null;
            }

            // Now it's ok to enable VR mode.
            XRSettings.enabled = true;
        }

        /// <summary>
        /// Raises the web cam texture to mat helper initialized event.
        /// </summary>
        public void OnWebCamTextureToMatHelperInitialized ()
        {
            Debug.Log ("OnWebCamTextureToMatHelperInitialized");

            Mat webCamTextureMat = webCamTextureToMatHelper.GetMat ();

            texture = new Texture2D (webCamTextureMat.cols (), webCamTextureMat.rows (), TextureFormat.RGBA32, false);
            ScreenQuad.GetComponent<Renderer> ().material.mainTexture = texture;


            Debug.Log ("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);


            float width = webCamTextureMat.width ();
            float height = webCamTextureMat.height ();

            float imageSizeScale = 1.0f;
            float widthScale = (float)Screen.width / width;
            float heightScale = (float)Screen.height / height;
            if (widthScale < heightScale) {
            } else {
                imageSizeScale = (float)Screen.height / (float)Screen.width;
            }                

            //set cameraparam
            int max_d = (int)Mathf.Max (width, height);
            double fx = max_d;
            double fy = max_d;
            double cx = width / 2.0f;
            double cy = height / 2.0f;
            camMatrix = new Mat (3, 3, CvType.CV_64FC1);
            camMatrix.put (0, 0, fx);
            camMatrix.put (0, 1, 0);
            camMatrix.put (0, 2, cx);
            camMatrix.put (1, 0, 0);
            camMatrix.put (1, 1, fy);
            camMatrix.put (1, 2, cy);
            camMatrix.put (2, 0, 0);
            camMatrix.put (2, 1, 0);
            camMatrix.put (2, 2, 1.0f);
            Debug.Log ("camMatrix " + camMatrix.dump ());

            distCoeffs = new MatOfDouble (0, 0, 0, 0);
            Debug.Log ("distCoeffs " + distCoeffs.dump ());


            //calibration camera
            Size imageSize = new Size (width * imageSizeScale, height * imageSizeScale);
            double apertureWidth = 0;
            double apertureHeight = 0;
            double[] fovx = new double[1];
            double[] fovy = new double[1];
            double[] focalLength = new double[1];
            Point principalPoint = new Point (0, 0);
            double[] aspectratio = new double[1];

            Calib3d.calibrationMatrixValues (camMatrix, imageSize, apertureWidth, apertureHeight, fovx, fovy, focalLength, principalPoint, aspectratio);

            Debug.Log ("imageSize " + imageSize.ToString ());
            Debug.Log ("apertureWidth " + apertureWidth);
            Debug.Log ("apertureHeight " + apertureHeight);
            Debug.Log ("fovx " + fovx [0]);
            Debug.Log ("fovy " + fovy [0]);
            Debug.Log ("focalLength " + focalLength [0]);
            Debug.Log ("principalPoint " + principalPoint.ToString ());
            Debug.Log ("aspectratio " + aspectratio [0]);


            //To convert the difference of the FOV value of the OpenCV and Unity. 
            double fovXScale = (2.0 * Mathf.Atan ((float)(imageSize.width / (2.0 * fx)))) / (Mathf.Atan2 ((float)cx, (float)fx) + Mathf.Atan2 ((float)(imageSize.width - cx), (float)fx));
            double fovYScale = (2.0 * Mathf.Atan ((float)(imageSize.height / (2.0 * fy)))) / (Mathf.Atan2 ((float)cy, (float)fy) + Mathf.Atan2 ((float)(imageSize.height - cy), (float)fy));

            Debug.Log ("fovXScale " + fovXScale);
            Debug.Log ("fovYScale " + fovYScale);


            //resize screen Quad
            Matrix4x4 p = ARUtils.CalculateProjectionMatrixFromCameraMatrixValues ((float)fx, (float)fy, (float)cx, (float)cy, width, height, 0.3f, 1000f);
            Vector3 cameraSpacePos = UnProjectVector (p, new Vector3 (1.0f, 1.0f, 1.0f));
            if (widthScale > heightScale) {
                ScreenQuad.transform.localScale = new Vector3 (cameraSpacePos.x * 2f, cameraSpacePos.x * height / width * 2f, 1);
            } else {
                ScreenQuad.transform.localScale = new Vector3 (cameraSpacePos.y * width / height * 2f, cameraSpacePos.y * 2f, 1);
            }


            //create markerDetector
            MarkerDesign[] markerDesigns = new MarkerDesign[markerSettings.Length];
            for (int i = 0; i < markerDesigns.Length; i++) {
                markerDesigns [i] = markerSettings [i].markerDesign;
            }
            markerDetector = new MarkerDetector (camMatrix, distCoeffs, markerDesigns);


            invertYM = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, -1, 1));
            Debug.Log ("invertYM " + invertYM.ToString ());

            invertZM = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, 1, -1));
            Debug.Log ("invertZM " + invertZM.ToString ());


            //if WebCamera is frontFaceing,flip Mat.
            if (webCamTextureToMatHelper.GetWebCamDevice ().isFrontFacing) {
                webCamTextureToMatHelper.flipHorizontal = true;
            }
        }

        /// <summary>
        /// Raises the web cam texture to mat helper disposed event.
        /// </summary>
        public void OnWebCamTextureToMatHelperDisposed ()
        {
            Debug.Log ("OnWebCamTextureToMatHelperDisposed");

            if (texture != null) {
                Texture2D.Destroy (texture);
                texture = null;
            }
        }

        /// <summary>
        /// Raises the web cam texture to mat helper error occurred event.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        public void OnWebCamTextureToMatHelperErrorOccurred (WebCamTextureToMatHelper.ErrorCode errorCode)
        {
            Debug.Log ("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
        }

        void Update ()
        {
            if (Input.GetKeyDown (KeyCode.Escape)) {
                OnBackButton ();
            }

            if (webCamTextureToMatHelper.IsPlaying () && webCamTextureToMatHelper.DidUpdateThisFrame ()) {

                Mat rgbaMat = webCamTextureToMatHelper.GetMat ();

                markerDetector.processFrame (rgbaMat, 1);

                foreach (MarkerSettings settings in markerSettings) {
                    if (!settings.shouldNotSetToInactivePerFrame) {
                        settings.setAllARGameObjectsDisable ();
                    } else {
                        GameObject ARGameObject = settings.getARGameObject ();
                        if (ARGameObject != null) {
                            DelayableSetActive obj = ARGameObject.GetComponent<DelayableSetActive> ();
                            if (obj != null) {
                                obj.SetActive (false, 0.5f);
                            }
                        }
                    }
                }

                List<Marker> findMarkers = markerDetector.getFindMarkers ();
                for (int i = 0; i < findMarkers.Count; i++) {
                    Marker marker = findMarkers [i];

                    foreach (MarkerSettings settings in markerSettings) {
                        if (marker.id == settings.getMarkerId ()) {
                            transformationM = marker.transformation;
                            //Debug.Log ("transformationM " + transformationM.ToString ());

                            ARM = ARCamera.transform.localToWorldMatrix * invertYM * transformationM * invertZM;
                            //Debug.Log ("arM " + arM.ToString ());

                            GameObject ARGameObject = settings.getARGameObject ();
                            if (ARGameObject != null) {
                                ARUtils.SetTransformFromMatrix (ARGameObject.transform, ref ARM);

                                DelayableSetActive obj = ARGameObject.GetComponent<DelayableSetActive> ();
                                if (obj != null) {
                                    obj.SetActive (true);
                                } else {
                                    ARGameObject.SetActive (true);
                                }
                            }
                        }
                    }
                }
                Utils.fastMatToTexture2D (rgbaMat, texture);
            }
        }

        /// <summary>
        /// Raises the disable event.
        /// </summary>
        void OnDisable ()
        {
            webCamTextureToMatHelper.Dispose ();
        }

        private Vector3 UnProjectVector (Matrix4x4 proj, Vector3 to)
        {
            Vector3 from = new Vector3 (0, 0, 0);
            var axsX = proj.GetRow (0);
            var axsY = proj.GetRow (1);
            var axsZ = proj.GetRow (2);
            from.z = to.z / axsZ.z;
            from.y = (to.y - (from.z * axsY.z)) / axsY.y;
            from.x = (to.x - (from.z * axsX.z)) / axsX.x;
            return from;
        }

        /// <summary>
        /// Raises the back button event.
        /// </summary>
        public void OnBackButton ()
        {
            SceneManager.LoadScene ("GoogleVRWithOpenCVForUnityExample");
        }

        /// <summary>
        /// Raises the play button event.
        /// </summary>
        public void OnPlayButton ()
        {
            webCamTextureToMatHelper.Play ();
        }

        /// <summary>
        /// Raises the pause button event.
        /// </summary>
        public void OnPauseButton ()
        {
            webCamTextureToMatHelper.Pause ();
        }

        /// <summary>
        /// Raises the stop button event.
        /// </summary>
        public void OnStopButton ()
        {
            webCamTextureToMatHelper.Stop ();
        }

        /// <summary>
        /// Raises the change camera button event.
        /// </summary>
        public void OnChangeCameraButton ()
        {
            webCamTextureToMatHelper.requestedIsFrontFacing = !webCamTextureToMatHelper.IsFrontFacing ();
        }

        /// <summary>
        /// Raises the recenter button event.
        /// </summary>
        public void OnRecenterButton ()
        {
            InputTracking.Recenter ();
        }
    }
}