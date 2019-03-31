using UnityEngine;
using System.Collections;

namespace GoogleVRWithOpenCVForUnityExample
{
    [RequireComponent (typeof(Collider))]
    public class ColorChangeGvrResponder : MonoBehaviour
    {
        void Start ()
        {
            SetGazedAt (false);
        }

        public void SetGazedAt (bool gazedAt)
        {
            GetComponent<Renderer> ().material.color = gazedAt ? Color.green : Color.red;
        }

        public void ChangeColor ()
        {
            GetComponent<Renderer> ().material.color = Color.blue;
        }

        #region IGvrGazeResponder implementation

        /// Called when the user is looking on a GameObject with this script,
        /// as long as it is set to an appropriate layer (see GvrGaze).
        public void OnGazeEnter ()
        {
            SetGazedAt (true);
        }

        /// Called when the user stops looking on the GameObject, after OnGazeEnter
        /// was already called.
        public void OnGazeExit ()
        {
            SetGazedAt (false);
        }

        /// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
        public void OnGazeTrigger ()
        {
            ChangeColor ();
        }

        #endregion
    }
}