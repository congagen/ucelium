using UnityEngine;
using System.Collections;

namespace Misc
{
	public class AutoMoveUser : MonoBehaviour {

		public float speedX = 0f;
		public float speedZ = 0f;
		public float rotationSpeed = 0f;

		float currentCamRotationY;
		private float currentX = 0;
		private float currentZ = 0;


		void Update () 
		{
			currentX = (1f * speedX);
			currentZ = (1f * speedZ);
			currentCamRotationY += (1f * rotationSpeed);
			
			transform.position += new Vector3 (currentX, transform.position.y, currentZ);
			Camera.main.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

			if (Input.GetKey (KeyCode.R)) 
			{
				speedX += 0.01f;
			} 

			if (Input.GetKey (KeyCode.F)) 
			{
				speedZ += 0.01f;
			} 

			if (Input.GetKey (KeyCode.Space)) 
			{
				speedX = 0f;
				speedZ = 0f;
			} 
		}
	}
}