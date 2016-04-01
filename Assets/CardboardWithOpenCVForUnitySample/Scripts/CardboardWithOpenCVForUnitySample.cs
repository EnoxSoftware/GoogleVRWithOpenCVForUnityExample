using UnityEngine;
using System.Collections;

namespace CardboardWithOpenCVForUnitySample
{
    public class CardboardWithOpenCVForUnitySample : MonoBehaviour
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
			Application.LoadLevel ("ShowLicense");
		}

		public void OnShowARMarkerButton ()
		{
			Application.LoadLevel ("ShowARMarker");
		}

        public void OnCardboardMarkerBasedARSample()
        {
            Application.LoadLevel("CardboardMarkerBasedARSample");
        }
	}
}