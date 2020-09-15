using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Envxzys 
{
	public class TileContentManager : MonoBehaviour
    {

		private bool objectSetupComplete = false;
		public GameObject settingsObject;
        public GameObject sessionObject;
        public bool midpointPivot = false;

        private GameObject currentTileObject;

		private List<GameObject> objectList = new List<GameObject>();
		private List<GameObject> objectPool = new List<GameObject>();

        private bool rotateContent = false;
        private bool resizeContent = false;

		private float minSize;
		private float maxSize;

		private float rootTileSize;
		private float positionNoiseValue;
		private float contentSizeLimit;

		private float cur_obj_size = 0f;
		private float minUpdateDelay = 0.01f;
		private float maxUpdateDelay = 0.1f;

		private GameObject currentContentObject;
		private Vector3 absolutePosition;

        private int noiseType = 1;
        Noise nvGens;


		void InitContentSelectorObjectPool()
		{
            objectList = sessionObject.GetComponent<Envxzys.ResourceManager> ().projectPrefabObjecstList;

			if(objectList.Count > 0)
			{
				for (int i = 0; i < objectList.Count; i++) 
				{
					GameObject inst_obj = objectList[i];
					GameObject obj = Instantiate (inst_obj, transform.position, Quaternion.identity) as GameObject;
					obj.transform.SetParent (transform);

                    objectPool.Add (obj);

					obj.transform.gameObject.SetActive (false);
				}
			}
		}


		void RotateContent(GameObject cr_ob)
		{
			int rotval = Mathf.RoundToInt(4 * Mathf.Sin(((cr_ob.transform.position.x + cr_ob.transform.position.z) * (Mathf.Sin(positionNoiseValue * 5648.456f)))));
			cr_ob.transform.localRotation = Quaternion.identity; 
			cr_ob.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f * rotval, 0f));
		}


		void ScaleContent(GameObject cr_ob)
		{
            cur_obj_size = Mathf.Clamp(Mathf.Abs(Mathf.Sin(positionNoiseValue * 23415612.4284f)) * maxSize, minSize, maxSize);
            cr_ob.transform.localScale = new Vector3(cur_obj_size, cur_obj_size, cur_obj_size);
		}


		void UpdateContent()
		{
			absolutePosition = transform.parent.position + transform.position;
            positionNoiseValue = nvGens.PositionNoise (
                absolutePosition.x, absolutePosition.y, 
                absolutePosition.z, noiseType
            );

			foreach(Transform tr in transform)
			{
				tr.gameObject.SetActive (false);
			}

			if (objectPool.Count > 0 && transform.parent != null) 
			{
				int obindx = Mathf.Clamp (
                    Mathf.RoundToInt (objectPool.Count * positionNoiseValue), 
                    0, 
                    Mathf.Abs(objectPool.Count - 1)
                );

				currentContentObject = objectPool[obindx];
				currentContentObject.gameObject.SetActive (true);

				if (rotateContent) 
				{
					RotateContent (currentContentObject);
				}

				if (resizeContent) 
				{
					ScaleContent (currentContentObject);
                } 

                if (midpointPivot) {
                    float currentObjectHeight = currentContentObject.GetComponent<Renderer>().bounds.size.y;

                    currentContentObject.transform.position = new Vector3(
                        transform.position.x, 
                        transform.parent.position.y + (currentObjectHeight * 0.5f), 
                        transform.position.z);
                } else {
                    currentContentObject.transform.position = new Vector3(
                        transform.position.x, 
                        transform.parent.position.y, 
                        transform.position.z);
                }

			}
		}


		void InitContentManager()
		{
            noiseType = (int)settingsObject.GetComponent<Envxzys.Settings>().noiseType;

			minUpdateDelay = settingsObject.GetComponent<Envxzys.Settings>().tileUpdateDelay;
			maxUpdateDelay = settingsObject.GetComponent<Envxzys.Settings>().tileUpdateDelay * 2f;

            currentTileObject = transform.parent.gameObject;

            rotateContent = settingsObject.GetComponent<Envxzys.Settings>().rotation;
            resizeContent = settingsObject.GetComponent<Envxzys.Settings>().scale;

            int contentCount = (int)settingsObject.GetComponent<Envxzys.Settings>().contentCount;

			float tileBounds = currentTileObject.GetComponent<MeshRenderer> ().bounds.size.x;
			rootTileSize = tileBounds / currentTileObject.transform.localScale.x;

			if (contentCount > 1) 
			{
				contentSizeLimit = (rootTileSize / (contentCount));
			} 
			else 
			{
				contentSizeLimit = ((rootTileSize));
			}

            minSize = ((settingsObject.GetComponent<Envxzys.Settings>().minSize * 0.01f) * (contentSizeLimit));
            maxSize = ((settingsObject.GetComponent<Envxzys.Settings>().maxSize * 0.01f) * (contentSizeLimit));
		}


		void OnEnable()
		{
			if (objectSetupComplete) {
				Invoke ("UpdateContent", Random.Range (minUpdateDelay, maxUpdateDelay));
			} 
			else 
            {
                nvGens = sessionObject.GetComponent<Envxzys.Noise>();
                InitContentManager();
                InitContentSelectorObjectPool();
				objectSetupComplete = true;
                UpdateContent();
			}
		}


	}
}