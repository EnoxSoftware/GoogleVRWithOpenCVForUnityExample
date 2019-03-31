using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GoogleVRWithOpenCVForUnityExample
{

    public class ShowLicense : MonoBehaviour
    {

        // Use this for initialization
        void Start ()
        {

        }

        // Update is called once per frame
        void Update ()
        {

        }

        public void OnBackButton ()
        {
            SceneManager.LoadScene ("GoogleVRWithOpenCVForUnityExample");
        }
    }
}
