using UnityEngine;
using System.Collections;


namespace Misc
{

    public class Rotate : MonoBehaviour
    {
    
        public float xSpeed = 0;
        public float ySpeed = 50;
        public float zSpeed = 0;

        [Space(5)]

        public bool randomXOffset = false;
        public bool randomYOffset = false;
        public bool randomZOffset = false;


        void rotateDefault() {
            transform.Rotate(
                (xSpeed * Time.deltaTime),
                (ySpeed * Time.deltaTime),
                (zSpeed * Time.deltaTime)
            );
        }

        void Update()
        {
            rotateDefault();
        }


        private void Start()
        {
            if (randomXOffset)
            {
                transform.Rotate(Random.Range(0, 360), 
                                 transform.rotation.y, 
                                 transform.rotation.z);

            }
            if (randomYOffset)
            {
                transform.Rotate(transform.rotation.x, 
                                 Random.Range(0, 360), 
                                 transform.rotation.z);

            }
            if (randomZOffset)
            {
                transform.Rotate(transform.rotation.x, 
                                 transform.rotation.y, 
                                 Random.Range(0, 360));

            }
        }


    }

}