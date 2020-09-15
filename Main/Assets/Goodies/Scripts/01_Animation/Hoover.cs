using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class Hoover : MonoBehaviour
    {

        public float hooverRange = 10f;
        public float hooverSpeed = 1f;

        public bool randomStartOffset = true;
        public float yOffset;

        void Update()
        {
            float sinVal = (Mathf.Sin(Time.time * hooverSpeed));
            float yVal = yOffset + hooverRange + (sinVal * hooverRange);

            transform.position = new Vector3(transform.position.x, yVal, transform.position.z);
        }

        void Start()
        {
            if (randomStartOffset)
            {
                yOffset = Random.Range(0, hooverRange);
            }
        }
    }
}