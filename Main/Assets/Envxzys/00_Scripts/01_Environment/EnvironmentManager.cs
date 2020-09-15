using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Envxzys 
{
	public class EnvironmentManager : MonoBehaviour
	{
		[HideInInspector]
		public bool objectSetupComplete = false;

        [Tooltip("Game object to be used as root tile")]
        public GameObject EnvxzysSettingsObject;
        private GameObject navigationObject;

        public GameObject currentRootTileObject;

        [Space(10)]

        private Vector3 currentQuantizedEnvPosition;
        private Vector3 previousQuantizedPosition;
        private int currentEnvType;
        private int currentEnvMode;
        private float mainUpdateDelay = 0.1F;
        private int tileCount;

		public int rootTileSizePositionOffset;

		private List<Vector3> rootTilePositionOffsetList = new List<Vector3>();

        private Dictionary<Vector3, GameObject> tileMapDict = new Dictionary<Vector3, GameObject>();
        private Dictionary<Vector3, Vector3> offsetDict = new Dictionary<Vector3, Vector3>();

		private HashSet<Vector3> newRootTilePositions = new HashSet<Vector3>();
		private List<GameObject> tilesToMoveList = new List<GameObject>();

		private GameObject tileToMove;

        public Vector3 currentQuantizedPosition;

        private float currentTileScale;
        private float tileSizeMultiplier;

        private float currentX;
        private float currentY;
        private float currentZ;


        void UpdateQuantizedPosition()
        {   
            tileSizeMultiplier = 0.1f / (float)currentTileScale;
            float tlScale = currentTileScale * 10f;

            if (navigationObject) {
                Vector3 navObjPos = navigationObject.transform.position;

                currentX = (float)Mathf.RoundToInt(navObjPos.x * tileSizeMultiplier) * (tlScale);
                currentY = (float)Mathf.RoundToInt(navObjPos.y * tileSizeMultiplier) * (tlScale);
                currentZ = (float)Mathf.RoundToInt(navObjPos.z * tileSizeMultiplier) * (tlScale);
                currentQuantizedEnvPosition = new Vector3(currentX, currentY, currentZ);    
            } else {
                currentX = (float)Mathf.RoundToInt(transform.position.x * tileSizeMultiplier) * (tlScale);
                currentY = (float)Mathf.RoundToInt(transform.position.y * tileSizeMultiplier) * (tlScale);
                currentZ = (float)Mathf.RoundToInt(transform.position.z * tileSizeMultiplier) * (tlScale);
                currentQuantizedEnvPosition = new Vector3(currentX, currentY, currentZ);    
            }
        }


        void UpdateRootTiles() {
			newRootTilePositions.Clear ();
			tilesToMoveList.Clear ();
			int count = 0;

			foreach (Vector3 ofPos in rootTilePositionOffsetList) {
				newRootTilePositions.Add (currentQuantizedEnvPosition + ofPos);
			}

			foreach (Transform rootTile in transform) {
                if (newRootTilePositions.Contains (rootTile.transform.position)) {
                    newRootTilePositions.Remove (rootTile.transform.position);
				} 
				else {
                    tilesToMoveList.Add (rootTile.gameObject);
                    rootTile.transform.gameObject.SetActive (false);
				}
			}

            foreach (Vector3 rootTilePos in newRootTilePositions) {
				tileToMove = tilesToMoveList [count];
                tileToMove.transform.position = rootTilePos;
				tileToMove.transform.gameObject.SetActive (true);
				count += 1;
			}

		}


		void NavigationUpdate() {
            
            if (currentEnvMode == 1) {
				if (currentQuantizedEnvPosition != previousQuantizedPosition) {
                    
					if (!(Input.anyKey) && Input.touches.Length < 1) {
						previousQuantizedPosition = currentQuantizedEnvPosition;
						Invoke ("UpdateRootTiles", mainUpdateDelay);
					}
				}
			}  
            else if (currentEnvMode == 2) {
                if (currentQuantizedEnvPosition != previousQuantizedPosition) {
                    
					previousQuantizedPosition = currentQuantizedEnvPosition;
					Invoke ("UpdateRootTiles", mainUpdateDelay);
				}
			}
		}



        void InitTileMap(){
            int nameCount = 0;


            for (int x = -(tileCount); x < tileCount; x++)
            {
                for (int z = -(tileCount); z < tileCount; z++)
                {

                    nameCount += 1;
                    GameObject tile = Instantiate(currentRootTileObject, transform.position, Quaternion.identity) as GameObject;

                    tile.name = "RootTile_" + Mathf.Abs(nameCount).ToString();
                    tile.GetComponent<Envxzys.TileManager>().sessionObject = transform.gameObject;
                    tile.GetComponent<Envxzys.TileManager>().settingsObject = EnvxzysSettingsObject;

                    tile.SetActive(true);

                    tile.transform.localScale = new Vector3(
                        rootTileSizePositionOffset * 0.1f,
                        rootTileSizePositionOffset * 0.1f,
                        rootTileSizePositionOffset * 0.1f
                    );

                    tile.transform.SetParent(transform);

                    float quantizedX = (float)Mathf.RoundToInt((x * rootTileSizePositionOffset)) + (rootTileSizePositionOffset * 0.5f);
                    float quantizedZ = (float)Mathf.RoundToInt((z * rootTileSizePositionOffset)) + (rootTileSizePositionOffset * 0.5f);

                    Vector3 offsetPos = new Vector3(quantizedX, transform.position.y, quantizedZ);

                    tileMapDict.Add(offsetPos, tile);
                    offsetDict.Add(offsetPos, offsetPos);

                    rootTilePositionOffsetList.Add(offsetPos);
                }
            }
        }


        void InitSpaceMap()
        {
            int nameCount = 0;


            for (int x = -(tileCount); x < tileCount; x++)
            {
                for (int y = -(tileCount); y < tileCount; y++)
                {
                    for (int z = -(tileCount); z < tileCount; z++)
                    { 
                        nameCount += 1;
                        GameObject tile = Instantiate(currentRootTileObject, transform.position, Quaternion.identity) as GameObject;

                        tile.name = "RootTile_" + Mathf.Abs(nameCount).ToString();
                        tile.GetComponent<Envxzys.TileManager>().sessionObject = transform.gameObject;
                        tile.GetComponent<Envxzys.TileManager>().settingsObject = EnvxzysSettingsObject;

                        tile.SetActive(true);

                        tile.transform.localScale = new Vector3(
                            rootTileSizePositionOffset * 0.1f,
                            rootTileSizePositionOffset * 0.1f,
                            rootTileSizePositionOffset * 0.1f
                        );

                        tile.transform.SetParent(transform);

                        float yOffset = tileCount * 0.5f;

                        float quantizedX = (float)Mathf.RoundToInt((x * rootTileSizePositionOffset)) + (rootTileSizePositionOffset * 0.5f);
                        float quantizedY = (float)Mathf.RoundToInt((yOffset + (y * rootTileSizePositionOffset))) + (rootTileSizePositionOffset * 0.5f);
                        float quantizedZ = (float)Mathf.RoundToInt((z * rootTileSizePositionOffset)) + (rootTileSizePositionOffset * 0.5f);

                        Vector3 offsetPos = new Vector3(
                            quantizedX, quantizedY, quantizedZ
                        );

                        tileMapDict.Add(offsetPos, tile);
                        offsetDict.Add(offsetPos, offsetPos);

                        rootTilePositionOffsetList.Add(offsetPos);
                    }


                }
            }
        }




        void InitEnvironment()
        {

            if (EnvxzysSettingsObject.GetComponent<Settings>().rootTileObject != null)
            {
                currentRootTileObject = EnvxzysSettingsObject.GetComponent<Settings>().rootTileObject;
            }

            if (EnvxzysSettingsObject.GetComponent<Settings>().navigationObject != null)
            {
                navigationObject = EnvxzysSettingsObject.GetComponent<Settings>().navigationObject;
            }

            currentTileScale = EnvxzysSettingsObject.GetComponent<Envxzys.Settings>().rootTileSize;
            tileCount = (int)EnvxzysSettingsObject.GetComponent<Settings>().mapSize;
            currentEnvMode = (int)EnvxzysSettingsObject.GetComponent<Settings>().updateMethod;
            currentEnvType = (int)EnvxzysSettingsObject.GetComponent<Settings>().environmentType;

            rootTileSizePositionOffset = Mathf.RoundToInt(EnvxzysSettingsObject.GetComponent<Settings>().rootTileSize * 10f);
            mainUpdateDelay = EnvxzysSettingsObject.GetComponent<Settings>().mainUpdateInterval + 0.001F;
            currentQuantizedEnvPosition = transform.GetComponent<Session>().currentQuantizedPosition;

            if (currentEnvType <= 2)
            {
                InitTileMap();
            }
            else {
                InitSpaceMap();
            }

			
			Invoke ("UpdateRootTiles", mainUpdateDelay);
		}


		void Update () {
            if (!(objectSetupComplete) && EnvxzysSettingsObject) {
                if (transform.GetComponent<Envxzys.Session>().sessionSetupComplete) {
					InitEnvironment ();

                    if(currentEnvType == 1 || currentEnvType == 3) {
						InvokeRepeating ("NavigationUpdate", 1f, mainUpdateDelay);
					}
					objectSetupComplete = true;
				}
			}
            else if (EnvxzysSettingsObject) {
                UpdateQuantizedPosition();
            } 
		}
		

	}
}