using UnityEngine;
using System.Collections;


namespace Moduli
{
    public class ObjInteraction : MonoBehaviour
    {

        public Camera rayPointCamera;


        void SetPosition()
        {

            Ray ray = new Ray();

            if (rayPointCamera) 
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            } else {
                if (Camera.main)
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                }
            }

            if (Camera.main || rayPointCamera) 
            {
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 pos = new Vector3(
                       hit.point.x, 0f, hit.point.z
                   );
                    transform.position = pos;
                }
            }
        }


        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                SetPosition();
            }
        }

    }
}