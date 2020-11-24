using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Envxzys
{
    public class ViewManager : MonoBehaviour
    {

        public Camera viewCamera;
        private Vector3 initialCameraPosition;
        private Quaternion initialCameraRotation;

		[Space(10)]
        
		[Range(0.1f, 10f)] public float zoomSensitivity = 0.5f;
		private float minZoom = 1.0f;
		[Range(0.1f, 100000f)] public float maxZoom = 10000.0f;

		[Space(10)]

		public bool mouseLook = false;
		[Range(0.1f, 10f)] public float mouseLookSensitivity = 2.0f;
		[Range(0.1f, 360f)] public float mouseLookClampAngle = 80.0f;


		private float mouseLookRotationX = 0.0f;
        private float mouseLookRotationY = 0.0f;


        public void Zoom(string scroll_dir)
        {
            Camera cam = viewCamera;

            if (cam.orthographic == true)
            {
                if (scroll_dir == "in")
                {
                    cam.orthographicSize -= Mathf.Clamp((zoomSensitivity * (cam.orthographicSize * 0.5f)), minZoom, maxZoom);
                    cam.farClipPlane -= Mathf.Clamp((zoomSensitivity * (cam.orthographicSize * 0.5f)), minZoom, maxZoom) * 100f;
                    cam.transform.position += cam.transform.forward * 100f;
                }
                if (scroll_dir == "out")
                {
                    cam.orthographicSize += Mathf.Clamp((zoomSensitivity * (cam.orthographicSize * 0.5f)), minZoom, maxZoom);
                    cam.farClipPlane += Mathf.Clamp((zoomSensitivity * (cam.orthographicSize * 0.5f)), minZoom, maxZoom) * 100f;
                    cam.transform.position -= cam.transform.forward * 100f;
                }
            }
            else
            {
                if (scroll_dir == "in")
                {
                    cam.fieldOfView += 1f;
                }

                if (scroll_dir == "out")
                {
                    cam.fieldOfView -= 1f;
                }
            }
        }


        public void MooveCamera(string direction) {
            Camera cam = viewCamera;
            float camY = Camera.main.transform.position.y;

            if (direction.ToLower() == "down")
            {
                if (cam.orthographic)
                {
                    cam.orthographicSize *= 1.01f;
                }
                else 
                {
                    camY *= .98f;
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, camY, Camera.main.transform.position.z);
                }
            }

            if (direction.ToLower() == "up")
            {
                if (cam.orthographic)
                {
                    cam.orthographicSize *= .998f;
                }
                else
                {
                    camY *= 1.1f;
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, camY, Camera.main.transform.position.z);
                }
            }

        }


        public void MouseLook()
        {
            if (!(viewCamera.orthographic))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = -Input.GetAxis("Mouse Y");

                mouseLookRotationY += mouseX * (mouseLookClampAngle * mouseLookSensitivity) * Time.deltaTime;
                mouseLookRotationX += mouseY * (mouseLookClampAngle * mouseLookSensitivity) * Time.deltaTime;

                viewCamera.transform.rotation = Quaternion.Euler(mouseLookRotationX, mouseLookRotationY, 0.0f);
            } 
        }


        public void setCameraPerspective(int persp)
        {
            
            if (persp == 1)
            {
                mouseLook = false;
                viewCamera.orthographic = true;
                viewCamera.transform.rotation = Quaternion.Euler(new Vector3(25, 45, 0));
            }
            else
            {
                mouseLook = true;
                viewCamera.orthographic = false;
                viewCamera.nearClipPlane = 1f;
                viewCamera.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
            }

        }


		private void Update()
		{
            if (viewCamera != null)
            { 
                    if (Input.GetKeyDown (KeyCode.Alpha1)) 
                {
                    mouseLook = false;
                    setCameraPerspective(1);

                }

                if (Input.GetKeyDown (KeyCode.Alpha2)) 
                {
                    setCameraPerspective(2);
                    mouseLook = true;
                } 

                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    Zoom("out");
                }

                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    Zoom("in");
                }

                if (Input.GetKey (KeyCode.DownArrow)) {
                    MooveCamera("down");
                }

                if (Input.GetKey (KeyCode.UpArrow)) {
                    MooveCamera("up");
                } 

                if (mouseLook)
                {
                    MouseLook();
                } 
            }

		}


		void Start()
        {
            if (viewCamera != null)
            {
                Vector3 rot = Camera.main.transform.localRotation.eulerAngles;
                mouseLookRotationX = rot.x;
                mouseLookRotationY = rot.y;

                mouseLook = (viewCamera.orthographic != true);

                initialCameraPosition = viewCamera.transform.position;
            }

        }


    }
}