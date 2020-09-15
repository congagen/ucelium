using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moduli
{
    public class ObjectNavigation : MonoBehaviour
    {

        private float sin_val;
        private float cos_val;
        private Vector3 initialWanderPosition;

        public float yOffset = 0f;
        public float speed = 10f;
        public float maxDistance = 0.001f;

        public bool yAxis = false;
        public bool facing = false;

        public GameObject navObject;
        public Vector3 desitnation;


        void Updaterotation()
        {

            Vector3 navObjPos = new Vector3(
                navObject.transform.position.x,
                transform.position.y,
                navObject.transform.position.z
            );

            Vector3 targetDir = navObjPos - transform.position;
            float step = 10 * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            //Debug.DrawRay(transform.position, newDir, Color.red);

            transform.rotation = Quaternion.LookRotation(newDir);
        }


        void UpdateLocation()
        {
            float dist = Vector3.Distance(transform.position, desitnation);

            if (dist > maxDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, desitnation, (Time.deltaTime * (dist + speed)));
            }
            else
            {
                if (navObject)
                {
                    if (yAxis)
                    {
                        desitnation = navObject.transform.position;
                    }
                    else
                    {
                        desitnation = new Vector3(
                            navObject.transform.position.x,
                            yOffset,
                            navObject.transform.position.z
                        );
                    }
                }
            }
        }


        void Update()
        {
            UpdateLocation();

            if (facing)
            {
                Updaterotation();
            }
        }

    }
}