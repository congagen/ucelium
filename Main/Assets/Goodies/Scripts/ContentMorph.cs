using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Misc
{
	public class ContentMorph : MonoBehaviour {

		public GameObject noiseSourceObject;
		private bool objectSetupComplete = false;
		private List<GameObject> currentObjectPool = new List<GameObject>();

		private float noiseValue;

		[Range(1f, 100f)] public float probabilitySkew = 50f;

		void MorpContent(){
			if (!(noiseSourceObject)) {
				noiseValue = Mathf.Sin (transform.position.x * transform.position.z);
			} else {
				noiseValue = Mathf.Sin (noiseSourceObject.transform.position.x * noiseSourceObject.transform.position.z);
			}

			float noiseSkew = (Mathf.Abs (noiseValue) + 0.001f) * probabilitySkew;
			float selectvalue = 1f / noiseSkew;
			int objectIndex = Mathf.Clamp (Mathf.RoundToInt (currentObjectPool.Count * selectvalue), 0, Mathf.Abs (currentObjectPool.Count - 1));

			foreach (Transform tr in transform) {
				tr.gameObject.SetActive (false);
			}

			currentObjectPool [objectIndex].gameObject.SetActive (true);
		}



		void OnEnable()
		{	
			if (transform.childCount > 1) {
				if (!(objectSetupComplete)) {
					foreach (Transform tr in transform) {
						currentObjectPool.Add (tr.gameObject);
					}
					objectSetupComplete = true;
					MorpContent ();
				} 
				else 
				{
					MorpContent ();
				}
			}
		}
	}
}