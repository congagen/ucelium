using UnityEngine;
using System.Collections;

namespace Misc
{
    public class Bounce : MonoBehaviour
    {

        private bool objectSetupComplete = false;
        public float bounceHeight = 10f;
        public float bounceSpeed;
        private float noiseValue;

        private Renderer currentObjectRender;
        private float currentObjectHeight;


        void Update()
        {
            if (objectSetupComplete)
            {
                currentObjectHeight = currentObjectRender.bounds.size.y;
                float sinVal = Mathf.Abs(Mathf.Sin(Time.time));

                transform.position = new Vector3(transform.position.x, (currentObjectHeight * 0.5f) + (sinVal * bounceHeight), transform.position.z);
            }
            else
            {
                currentObjectRender = transform.GetComponent<Renderer>();
                objectSetupComplete = true;
            }
        }


        void OnEnable()
        {
            noiseValue = Mathf.PerlinNoise(transform.position.x * 0.25f, transform.position.z * 0.25f);
            bounceHeight = Mathf.Abs(bounceHeight * noiseValue);
        }
    }
}