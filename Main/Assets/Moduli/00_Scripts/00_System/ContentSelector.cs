using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moduli
{
    public class ContentSelector : MonoBehaviour
    {

        [HideInInspector]
        public bool isInit = false;

        public bool managed = false;

        [Space(10)]

        [Tooltip("Root object scale")]
        public float scaleMultiplier = 1;

        public float randomScaleMultiplier = 2;
        public bool randomize = false;

        [Tooltip("Conform conntent scale to placeholder scale")]
        public bool useRootObjScale = false;

        [Tooltip("Refresh content when activated")]
        public bool refreshContent = true;

        [HideInInspector]
        public Vector3 externalSeed = new Vector3(0, 0, 0);

        public bool useExternalSeed = true;

        [Space(10)]

        public bool hidePlaceholders = true;
        public bool deactivatePlaceholders = true;
        public bool deletePlaceholders = true;

        [Space(10)]

        public List<string> objectPaths = new List<string>();
        private List<GameObject> objContent = new List<GameObject>();

        private Vector3 scaledScale = new Vector3();


        public void RefreshContent()
        {
            double indexNoiseVal = 0.0D;
            int objIndex = 0;
            Transform activeContentObj = transform;

            if (transform.childCount > 0)
            {

                foreach (Transform obj in transform)
                {
                    obj.transform.gameObject.SetActive(false);
                }

                if (!randomize) 
                {
                    if(transform.childCount > 0) 
                    {
                        if (!useExternalSeed)
                        {
                            Transform rootTrf = transform;
                            float noiseSeedA = (rootTrf.position.x + rootTrf.position.y);
                            float noiseSeedB = (rootTrf.position.x + rootTrf.position.z);
                            indexNoiseVal = Mathf.PerlinNoise(noiseSeedA, noiseSeedB);
                        }
                        else
                        {
                            float noiseSeedA = Mathf.Sin(externalSeed.x + externalSeed.y + externalSeed.z);
                            float noiseSeedB = Mathf.Cos(externalSeed.x + externalSeed.z);

                            indexNoiseVal = Mathf.PerlinNoise(
                                noiseSeedA * externalSeed.x,
                                noiseSeedB * externalSeed.y
                            );
                        }

                        objIndex = (int)(Mathf.Abs(transform.childCount - 1) * indexNoiseVal);
                    }

                    if (!managed)
                    {
                        transform.localScale = scaledScale;
                    }

                } 
                else 
                {
                    objIndex = (int)Random.Range(0, Mathf.Abs(transform.childCount - 1));

                    if (!managed)
                    {
                        float rndScaleMulti = Random.Range(scaleMultiplier, randomScaleMultiplier);

                        transform.localScale = new Vector3(
                            transform.localScale.x * rndScaleMulti,
                            transform.localScale.y * rndScaleMulti,
                            transform.localScale.z * rndScaleMulti
                        );
                    } 
                }

                activeContentObj = transform.GetChild(objIndex);
                activeContentObj.gameObject.SetActive(true);
            }


        }


        void InitPlaceholders()
        {
            if (hidePlaceholders)
            {
                if (transform.gameObject.GetComponent<Renderer>())
                {
                    transform.gameObject.GetComponent<Renderer>().enabled = false;
                }
            }

            if (deletePlaceholders)
            {
                foreach (Transform obj in transform)
                {
                    Destroy(obj.gameObject);
                }
            } else {
                foreach (Transform obj in transform)
                {
                    if (hidePlaceholders)
                    {
                        if (obj.GetComponent<Renderer>())
                        {
                            obj.GetComponent<Renderer>().enabled = false;
                        }
                    }
                }

                if (deactivatePlaceholders)
                {
                    foreach (Transform obj in transform)
                    {
                        obj.gameObject.SetActive(false);
                    }
                }
            }
        }


        void InitContent(GameObject rootObj, List<string> gameObjectPaths, List<GameObject> objList)
        {
            int count = 0;

            scaledScale = new Vector3(
                transform.localScale.x * scaleMultiplier,
                transform.localScale.y * scaleMultiplier,
                transform.localScale.z * scaleMultiplier
            );

            foreach (string path in gameObjectPaths)
            {
                if (path != "")
                {
                    GameObject[] prefabFiles = Resources.LoadAll<GameObject>(path);
                    foreach (GameObject ob in prefabFiles)
                    {
                        objList.Add(ob);
                    }
                }
            }

            foreach (GameObject go in objList)
            {
                GameObject newObj = Instantiate(
                    go, 
                    rootObj.transform.position, 
                    rootObj.transform.rotation) as GameObject;

                newObj.transform.SetParent(rootObj.transform);

                newObj.name = count.ToString();
                count += 1;
            }
        }


        void OnEnable()
        {

            if (!isInit)
            {
                InitPlaceholders();
                InitContent(transform.gameObject, objectPaths, objContent);

                RefreshContent();
                isInit = true;
            }
            else
            {
                RefreshContent();
            }

        }


    }
}