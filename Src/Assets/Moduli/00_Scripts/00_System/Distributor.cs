using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moduli
{
    public class Distributor : MonoBehaviour
    {

        public List<string> contentPaths = new List<string>();
        public float objectCountMulti = 100;

        [Space(5)]

        public float posRangeX = 100;
        public float posRangeY = 0;
        public float posRangeZ = 100;

        [Space(5)]

        public float minScale = 0;
        public float maxScale = 1;

        [Space(5)]

        public float rotRangeX = 0;
        public float rotRangeY = 180;
        public float rotRangeZ = 0;

        private List<GameObject> objContent = new List<GameObject>();

        [HideInInspector]
        public bool isInit = false;


        void InitContent(GameObject rootObj, List<string> gameObjectPaths)
        {

            foreach (string path in gameObjectPaths)
            {
                if (path != "")
                {
                    GameObject[] prefabFiles = Resources.LoadAll<GameObject>(path);
                    foreach (GameObject ob in prefabFiles)
                    {
                        objContent.Add(ob);
                    }
                }
            }

            foreach (GameObject go in objContent)
            {
                for (int i = 0; i < objectCountMulti; i++)
                {
                    GameObject newObj = Instantiate(
                    go,
                    rootObj.transform.position,
                    rootObj.transform.rotation) as GameObject;

                    newObj.transform.SetParent(rootObj.transform);

                    newObj.name = i.ToString();
                }

            }

            isInit = true;
        }


        public void RefreshContent(GameObject objectRoot)
        {
            if (objectRoot.transform.childCount > 0)
            {
                foreach (Transform activeContentObj in objectRoot.transform)
                {
                    float objScale = Random.Range(minScale, maxScale);

                    activeContentObj.transform.position = new Vector3(
                        transform.position.x + Random.Range(-posRangeX, posRangeX),
                        transform.position.y + Random.Range(-posRangeY, posRangeY),
                        transform.position.z + Random.Range(-posRangeZ, posRangeZ)
                    );

                    activeContentObj.transform.rotation = new Quaternion(
                        Random.Range(-rotRangeX, rotRangeX),
                        Random.Range(-rotRangeY, rotRangeY),
                        Random.Range(-rotRangeZ, rotRangeZ),
                        Random.Range(-rotRangeY, rotRangeY)
                    );

                    activeContentObj.transform.localScale = new Vector3(
                        objScale,
                        objScale,
                        objScale
                    );

                    activeContentObj.gameObject.SetActive(true);

                }
            }
        }



        void OnEnable()
        {
            if (!isInit)
            {
                InitContent(
                    transform.gameObject,
                    contentPaths
                );
            }

            RefreshContent(transform.gameObject);


        }
    }
}