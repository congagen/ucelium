using UnityEngine;
using System.Collections;

namespace Misc
{
	public class CameraCapture : MonoBehaviour {
		
		public string outputPath = "";
		private int frame = 0;

		void capture()
		{
			ScreenCapture.CaptureScreenshot (outputPath + frame.ToString(), 1);
		}


		void Update () 
		{
			frame += 1;
			capture ();
		}

	}
}