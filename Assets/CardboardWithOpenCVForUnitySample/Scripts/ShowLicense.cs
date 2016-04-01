using UnityEngine;
using System.Collections;

namespace CardboardWithOpenCVForUnitySample
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
            Application.LoadLevel("CardboardWithOpenCVForUnitySample");
		}
	}
}
