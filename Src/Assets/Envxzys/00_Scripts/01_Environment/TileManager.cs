using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Envxzys 
{
    public class TileManager : MonoBehaviour
	{
		private bool objectInitComplete = false;
        private bool objectRefreshComplete = false;

		public GameObject settingsObject;
        public GameObject sessionObject;

		public GameObject contentSelector;

		public Vector3 previousTilePosition;
		private float positionOffset;
		public List<GameObject> contentSelectorObjectList;

		private float minUpdateDelay;
		private float maxUpdateDelay;
		private float tileUpdateDelay;
		private float rootTileXBounds;

		private bool dynamicProbability;
		private float contentProbability;
		private int tileCount;
		private int currentrootTileContentCount;

		private float startposX;
		private float startposZ;

        private int noiseType = 1;
        private Noise nvGens;
		private bool renderTiles = true;


		IEnumerator UpdateContentSelectors()
		{
			foreach (Transform selector_obj in transform) 
			{
				yield return new WaitForSeconds(tileUpdateDelay);

                if (settingsObject.GetComponent<Envxzys.Settings>().probability < 100) 
                {
					if (dynamicProbability)
                    {
                        float actival_a = nvGens.PositionNoise (
                            transform.position.x + selector_obj.transform.position.x, 
                            transform.position.y + transform.position.z + selector_obj.transform.position.z, 
                            transform.position.z + selector_obj.transform.position.z, 
                            noiseType
                        );

                        float actival_b = Mathf.Abs(
                            contentProbability * Mathf.Sin(
                                (transform.parent.position.x + selector_obj.transform.position.x) * (transform.parent.position.z + selector_obj.transform.position.z)));

                        if (actival_a < actival_b) 
						{
							selector_obj.gameObject.SetActive (true);
						}
					} 
					else 
					{
                        float actival = nvGens.PositionNoise (
                            transform.position.x * selector_obj.transform.position.x, 
                            transform.position.y * selector_obj.transform.position.y,
                            transform.position.z * selector_obj.transform.position.z, 
                            noiseType
                        );
                        
                        if (actival < contentProbability) {
							selector_obj.gameObject.SetActive (true);
						}
					}
				} 
				else 
				{
					selector_obj.gameObject.SetActive (true);
				}
			}
			transform.GetComponent<Renderer> ().enabled = renderTiles;
		}


		void BuildContentSelectorPool()
		{
            int s_num_ctsel_objs = (int)settingsObject.GetComponent<Envxzys.Settings>().contentCount;
			int num_ctsel_objs = Mathf.RoundToInt (s_num_ctsel_objs * s_num_ctsel_objs);

			for (int i = 0; i <= num_ctsel_objs-1; i++) 
			{
				GameObject ct_gen = Instantiate (contentSelector, transform.position, Quaternion.identity) as GameObject;
                ct_gen.GetComponent<Envxzys.TileContentManager>().settingsObject = settingsObject;
                ct_gen.GetComponent<Envxzys.TileContentManager>().sessionObject = sessionObject;

                ct_gen.transform.SetParent (transform);
				ct_gen.transform.name = "ContentSelector_" + (i+1).ToString ();
				contentSelectorObjectList.Add (ct_gen);
			}
		}


		void DistributeContentSelectors()
		{
			if (currentrootTileContentCount > 1) 
			{
				int count = 0;

				for (int x = 0; x < currentrootTileContentCount; x++) 
				{
					for (int z = 0; z < currentrootTileContentCount; z++) 
					{
						if (count <= contentSelectorObjectList.Count) 
						{
							GameObject ctobj = contentSelectorObjectList [count];
							Vector3 content_pos = new Vector3 ((startposX + (x * (positionOffset))) + (positionOffset * 0.5f), ctobj.transform.position.y, (startposZ + (z * (positionOffset))) + (positionOffset * 0.5f));
							ctobj.transform.name = "ContentSelector_00" + count.ToString();
							ctobj.transform.position = content_pos;
							ctobj.gameObject.SetActive (true);
							count += 1;
						}
					}
				}
			} 
			else 
			{
				GameObject ctobj = contentSelectorObjectList [0];
				ctobj.transform.name = "ContentSelector_00";
				ctobj.transform.position = transform.position;
				ctobj.gameObject.SetActive (true);
			}
		}


		void InitTileManager() {
            contentSelector = settingsObject.GetComponent<Envxzys.Settings>().contentSelector;
            noiseType = (int)settingsObject.GetComponent<Envxzys.Settings>().noiseType;
			tileCount = (int)settingsObject.GetComponent<Envxzys.Settings>().mapSize;
            currentrootTileContentCount = (int)settingsObject.GetComponent<Envxzys.Settings>().contentCount;
			renderTiles = settingsObject.GetComponent<Envxzys.Settings>().renderTiles;
            contentProbability = 0.01f * settingsObject.GetComponent<Envxzys.Settings>().probability;
            dynamicProbability = settingsObject.GetComponent<Envxzys.Settings>().alternate;

			var totalContentCount = ((currentrootTileContentCount) * (currentrootTileContentCount)) * ((tileCount * 2f) * (tileCount * 2f));

			minUpdateDelay = (settingsObject.GetComponent<Envxzys.Settings>().tileUpdateDelay * (totalContentCount));
			maxUpdateDelay = (settingsObject.GetComponent<Envxzys.Settings>().tileUpdateDelay * (totalContentCount)) * 2f;
			tileUpdateDelay = Random.Range (minUpdateDelay, maxUpdateDelay);

            if (transform.GetComponent<Renderer>()){
                rootTileXBounds = transform.GetComponent<Renderer>().bounds.size.x;
            }

			positionOffset = rootTileXBounds / currentrootTileContentCount;
			startposX = transform.position.x - (rootTileXBounds * 0.5f);
			startposZ = transform.position.z - (rootTileXBounds * 0.5f);
		}


		void StartUpdate()
		{
			StartCoroutine (UpdateContentSelectors ());
		}


        void ObjUpdate() 
        {
            foreach (Transform selector_obj in transform) {
                selector_obj.gameObject.SetActive(false);
            }

            tileUpdateDelay = Random.Range(minUpdateDelay, maxUpdateDelay) * 0.00001f;
            transform.GetComponent<Renderer>().enabled = false;
            Invoke("StartUpdate", tileUpdateDelay);
        }


		void OnEnable () 
        {
            if (objectInitComplete && settingsObject) 
            {
                ObjUpdate();
			} 
            else if (settingsObject) 
            {
                nvGens = sessionObject.GetComponent<Envxzys.Noise>();

				InitTileManager();
				BuildContentSelectorPool();
				DistributeContentSelectors();
				StartCoroutine(UpdateContentSelectors());

                ObjUpdate();
                objectInitComplete = true;
			}
		}


	}
}