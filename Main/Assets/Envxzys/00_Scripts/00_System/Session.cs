using UnityEngine;
using System.Collections;


namespace Envxzys 
{
    public class Session : MonoBehaviour 
    {

		[HideInInspector]
		public bool sessionSetupComplete = false;
        public GameObject envxzysSettingsObject;

		private GameObject navigationObject;
		public Vector3 currentQuantizedPosition;

		[HideInInspector]
		public Vector3 previousQuantizedPosition;


        private void Update()
        {
            transform.gameObject.GetComponent<Envxzys.Noise>().noiseOffset = envxzysSettingsObject.GetComponent<Settings>().noiseOffset;
            transform.gameObject.GetComponent<Envxzys.Noise>().noiseScale  = envxzysSettingsObject.GetComponent<Settings>().noiseScale;
            transform.gameObject.GetComponent<Envxzys.Noise>().randomSeed  = envxzysSettingsObject.GetComponent<Settings>().randomSeed;

            if (!sessionSetupComplete) {
                transform.gameObject.GetComponent<Envxzys.Noise>().SetRandomSeed(
                    envxzysSettingsObject.GetComponent<Settings>().randomSeed
                );

                if (envxzysSettingsObject.GetComponent<Envxzys.Settings>().navigationObject)
                {
                    navigationObject = envxzysSettingsObject.GetComponent<Envxzys.Settings>().navigationObject;
                    if (navigationObject.GetComponent<Envxzys.ViewManager>())
                    {
                        navigationObject.GetComponent<Envxzys.ViewManager>().setCameraPerspective(
                            (int)envxzysSettingsObject.GetComponent<Envxzys.Settings>().initialProjection
                        );
                    }
                }

                bool dataReady = transform.GetComponent<Envxzys.ResourceManager>().sessionDataReady;
                bool tileObjectFound = envxzysSettingsObject.GetComponent<Envxzys.Settings>().rootTileObject != null;

                if (dataReady && tileObjectFound)
                {
                    sessionSetupComplete = true;
                }
            }

        }


	}
}
