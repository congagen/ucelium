using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Misc
{
	public class PositionColor : MonoBehaviour {

		private bool objectSetupComplete = false;
		public GameObject EnvxzysSettingsObject;

		private Material objectMaterial;
		public float minInvokeDelay = 0.1f;
		public float maxInvokeDelay = 1f;

		public bool onlyGreys = false;
        public bool useY = false;
        public bool continuous = false;

		public Color colorA = new Color(1f, 0.7f, 0.3f, 1f);
		public Color colorB = new Color(0.5f, 1f, 0.5f, 1f);
		public Color colorC = new Color(0.5f, 0.5f, 1f, 1f);
        

        float currentNoiseValue;

		void SetColor()
		{
            if (!useY)
            {
                currentNoiseValue = Mathf.PerlinNoise(
                   (transform.position.x) * 0.1f,
                   (transform.position.z) * 0.1f
               );
            } else {
                currentNoiseValue = Mathf.PerlinNoise(
                    (transform.position.x) * 0.1f,
                    (transform.position.y + transform.position.z) * 0.1f
                );
            }

            if (onlyGreys) 
			{
				objectMaterial.color = Color.grey * currentNoiseValue;
			} 
			else 
			{

                if(transform.GetComponent<Renderer>()) 
                {
                    if (currentNoiseValue < 0.3f)
                    {
                        transform.GetComponent<Renderer>().material.color = colorA;
                    }
                    else if (currentNoiseValue > 0.3f && currentNoiseValue < 0.6f)
                    {
                        transform.GetComponent<Renderer>().material.color = colorB;
                    }
                    else if (currentNoiseValue > 0.6f && currentNoiseValue <= 1f)
                    {
                        transform.GetComponent<Renderer>().material.color = colorC;
                    }
                    else
                    {
                        transform.GetComponent<Renderer>().material.color = Color.gray;
                    }
                }
	
			}

		}


		void InitPositionColor()
        {
            if (transform.GetComponent<Renderer>())
            {
                objectMaterial = transform.GetComponent<Renderer>().material;
            }
		}


		void OnEnable()
		{


			if (objectSetupComplete) 
			{
				Invoke ("SetColor", Random.Range (minInvokeDelay, maxInvokeDelay));
			} 
			else 
			{
				objectSetupComplete = true;
				InitPositionColor ();
				Invoke ("SetColor", Random.Range (minInvokeDelay, maxInvokeDelay));
			}
		}


        private void Update()
        {
            if (continuous) {
                Invoke("SetColor", 0);
            }
        }

    }
}