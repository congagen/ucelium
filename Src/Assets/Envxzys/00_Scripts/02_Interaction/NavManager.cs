using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Envxzys 
{
    public class NavManager: MonoBehaviour
	{
		private bool objectSetupComplete = false;
		public GameObject OriginObject;
        
        public bool touchNavigation;
        public bool mouseNavigation;
        public bool keyboardNavigation;

		[Space(5)]

		[Tooltip("Toggles axis restriction for mouse look")]
		public bool lockYAxis = true;
       
		[Space(10)]

		[Range(0.1f, 10f)] public float mouseNavigationSpeed = 1.0f;
		[Range(0.1f, 10f)] public float keyboardNavigationSpeed = 0.2f;

		[Space(5)]

        private Vector3 initialNavigationPosition = Vector3.zero;
        private Vector3 navigationDestination;

        private int fingerCount;
		private float new_x;
		private float new_z;

		private float mouseLookRotationX = 0.0f;
		private float mouseLookRotationY = 0.0f; 

		private bool startTouchZoom = false;
		private float startTouchDistance = 0;
		private float currentTouchDistance = 0;

		private Vector3 initialOrthoPosition;
		private Quaternion initialCameraRotation;



        public void MoveToPosition (Vector3 inputv3) {
            new_x = transform.position.x + (inputv3.x * mouseNavigationSpeed);
            new_z = transform.position.z + (inputv3.z * mouseNavigationSpeed);

			transform.position = new Vector3 (new_x, 0f, new_z);
		}


		void TouchNavigation ()
		{
			Vector2 touchDeltaPosition = Input.touches [0].deltaPosition;

			float positionX = touchDeltaPosition.x * Time.deltaTime;
			float positionY = touchDeltaPosition.y * Time.deltaTime;

			navigationDestination = new Vector3 ((float)Mathf.RoundToInt (positionX), 0f, (float)Mathf.RoundToInt (positionY));

            MoveToPosition (navigationDestination);
		}
			

		void MonitorTouchInput()
		{
			fingerCount = Input.touches.Length;

			if (fingerCount == 1) {
				TouchNavigation ();
			} 

			if (fingerCount == 2) {
				if (!(startTouchZoom)) 
				{
					startTouchDistance = Vector3.Distance (Input.touches [0].position, Input.touches [1].position);
					startTouchZoom = true;
				}	

				currentTouchDistance = Vector3.Distance (Input.touches [0].position, Input.touches [1].position);

			} else {
				startTouchZoom = false;
			}
		}


		void MouseButtonPressed ()
		{
			Vector3 direction = Camera.main.ScreenToWorldPoint (Input.mousePosition) - Camera.main.ScreenToWorldPoint (initialNavigationPosition);

			direction = direction * -1;
            Vector3 dir_pos = OriginObject.transform.position + direction;
            navigationDestination = new Vector3 (dir_pos.x, dir_pos.y, dir_pos.z);

            MoveToPosition (navigationDestination);
		}

       
		void MonitorMouseInput()
		{

			if (Input.GetMouseButtonDown (0)) 
			{
				initialNavigationPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton (0)) 
			{
				MouseButtonPressed ();
			}

		}


		void MonitorKeyboardInput()
		{
            
			if (Input.GetKey (KeyCode.W)) 
			{
				if (!lockYAxis) 
                {
                    transform.position += (new Vector3(
                        Camera.main.transform.forward.x * keyboardNavigationSpeed,
                        Camera.main.transform.forward.y * keyboardNavigationSpeed,
                        Camera.main.transform.forward.z * keyboardNavigationSpeed));
                }
                else
                {
                    transform.position += (new Vector3(
                        Camera.main.transform.forward.x * keyboardNavigationSpeed,
                        0f,
                        Camera.main.transform.forward.z * keyboardNavigationSpeed));
                }
			} 

			if (Input.GetKey (KeyCode.S)) 
			{
				if (!lockYAxis)
                {
                    transform.position -= (new Vector3(
                        Camera.main.transform.forward.x * keyboardNavigationSpeed,
                        Camera.main.transform.forward.y * keyboardNavigationSpeed,
                        Camera.main.transform.forward.z * keyboardNavigationSpeed));
                } else {
                    transform.position -= (new Vector3(
                        Camera.main.transform.forward.x * keyboardNavigationSpeed,
                        0f,
                        Camera.main.transform.forward.z * keyboardNavigationSpeed));
                }
			} 

			if (Input.GetKey (KeyCode.A)) 
			{
				transform.position -= (new Vector3(Camera.main.transform.right.x * keyboardNavigationSpeed, 0f, Camera.main.transform.right.z * keyboardNavigationSpeed));
			} 

			if (Input.GetKey (KeyCode.D)) 
			{
				transform.position -= -(new Vector3(Camera.main.transform.right.x * keyboardNavigationSpeed, 0f, Camera.main.transform.right.z * keyboardNavigationSpeed));
			} 


			if (Input.GetKey (KeyCode.Keypad8)) 
			{
				RenderSettings.fogDensity *= 1.01F;
			}

			if (Input.GetKey (KeyCode.Keypad2)) 
			{
				RenderSettings.fogDensity *= 0.99F;
			}
		}


		void Update () {
            
			if (mouseNavigation) 
			{
				MonitorMouseInput ();
			}

			if (touchNavigation) 
			{
				MonitorTouchInput ();
			}

			if (keyboardNavigation) 
			{
				MonitorKeyboardInput ();
			}

		}


		void Start()
        {
            initialNavigationPosition = transform.position;
            MouseButtonPressed();

			objectSetupComplete = true;
		}

	}
}