using UnityEngine;
using System.Collections;

namespace Misc
{
    public class Wander : MonoBehaviour
    {

        private bool objectSetupComplete = false;
        public float maxWanderDistance = 10;
        public float retrigDistance = 0.01f;

        public bool wanderY = false;
        public float maxSpeed = 100;

        public Vector3 wanderOffset = new Vector3(0, 0, 0);
        private Vector3 desitnation;


        void WanderBounds()
        {
            float dist = Vector3.Distance(transform.position, desitnation);

            if (dist > maxSpeed)
            {
                dist = maxSpeed;
            }

            if (dist < retrigDistance)
            {
                float destval_x = Random.Range(-maxWanderDistance, maxWanderDistance);
                float destval_z = Random.Range(-maxWanderDistance, maxWanderDistance);

                if (wanderY)
                {
                    float destval_y = Random.Range(-maxWanderDistance, maxWanderDistance);

                    desitnation = new Vector3(
                        wanderOffset.x + destval_x,
                        wanderOffset.y + destval_y,
                        wanderOffset.z + destval_z
                    );

                }
                else
                {
                    desitnation = new Vector3(
                        wanderOffset.x + destval_x,
                        transform.position.y,
                        wanderOffset.z + destval_z
                    );
                }


            }
            else
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, desitnation,
                    (Time.deltaTime * (dist + 1f))
                );
            }
        }


        void Update()
        {
            WanderBounds();
        }


        void OnEnable()
        {
            float destval_x = Random.Range(-maxWanderDistance, maxWanderDistance);
            float destval_z = Random.Range(-maxWanderDistance, maxWanderDistance);

            desitnation = new Vector3(destval_x, 0, destval_z);
        }

    }
}