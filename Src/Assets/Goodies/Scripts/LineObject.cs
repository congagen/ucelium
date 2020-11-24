using UnityEngine;
using System.Collections;

namespace Misc
{
	[RequireComponent (typeof (LineRenderer))]
	public class LineObject : MonoBehaviour {
		public LineRenderer lineRenderer;

		public float minInvokeDelay = 0.1f;
		public float maxInvokeDelay = 0.3f;

		public float multinum = 0.5f;
		public float scaleMultiplier = 0.1f;

		void InitLineObjectGenerator()
		{
			if (transform.GetComponent<LineRenderer> () != null)
			{
				lineRenderer = transform.GetComponent<LineRenderer> ();
				int numElements = Mathf.Abs(Mathf.RoundToInt(Mathf.Sin (transform.position.x+transform.position.z) * 10f));
                lineRenderer.positionCount = numElements;

				multinum = Mathf.Abs(Mathf.Sin ((transform.position.x * 44632) * (transform.position.z * 154f)));

				for (int i = 0; i < numElements; i++) 
				{
					float xmul = Mathf.Abs(Mathf.Sin (i * (i * multinum)) * i) * scaleMultiplier;
					float zmul = Mathf.Abs(Mathf.Sin (i * (i * multinum)) * i) * scaleMultiplier;

					lineRenderer.SetPosition (i, new Vector3(transform.position.x + xmul, (float)i * scaleMultiplier, transform.position.z - zmul));
				}
			}
		}


		void OnEnable ()
		{
			Invoke ("InitLineObjectGenerator", Random.Range(minInvokeDelay, maxInvokeDelay));

		}
	}
}