using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moduli
{
    public class ObjectManager : MonoBehaviour
    {

        //----------------------------------------------------------------------
        [Header("Content:")]
        //----------------------------------------------------------------------

        [Tooltip("Objects to be populated with Mod Objects")]
        public List<GameObject> targetObjects = new List<GameObject>();

        [Tooltip("Objects to be added to corresponding Target Objects")]
        public List<GameObject> modObjects = new List<GameObject>();

        [Space(5)]

        [Tooltip("Deactivates target object renderers if enabled")]
        public bool hidePlaceholders = true;

        [Tooltip("Root object scale factor")]
        public float scaleMultiplier = 1;

        [Space(15)]

        //----------------------------------------------------------------------
        [Header("Noise:")]
        //----------------------------------------------------------------------

        [Space(5)]


        [Tooltip("Distribute manager object noise seed")]
        public bool useRootNoise = true;

        [Tooltip("Get noise seed from root object position")]
        public bool positionNoise = true;

        [Tooltip("Noise seed values")]
        public Vector3 noiseSeed = new Vector3(0, 0, 0);

        private bool isInit = false;
        private Bounds bounds;
        private GameObject contentSelector;
        private Vector3 initScale = new Vector3();



        void AddModObjects(GameObject rootObj, GameObject selObject)
        {
            GameObject modObject = Instantiate(
                selObject, 
                rootObj.transform.position,
                transform.rotation) as GameObject;


            if (modObject.GetComponent<ContentSelector>())
            {
                modObject.GetComponent<ContentSelector>().managed = true;

                if (useRootNoise)
                {
                    modObject.GetComponent<ContentSelector>().useExternalSeed = true;
                    modObject.GetComponent<ContentSelector>().externalSeed = noiseSeed;
                }

                modObject.GetComponent<ContentSelector>().RefreshContent();
            }


            modObject.transform.SetParent(rootObj.transform);
            modObject.name = "Selector"; 
        }


        void UpdatePlaceholders()
        {
            if (hidePlaceholders)
            {
                if (transform.gameObject.GetComponent<Renderer>())
                {
                    transform.gameObject.GetComponent<Renderer>().enabled = false;
                }

                foreach (GameObject o in targetObjects)
                {
                    if(o){
                        if (o.transform.gameObject.GetComponent<Renderer>())
                        {
                            o.transform.gameObject.GetComponent<Renderer>().enabled = false;
                        }

                        foreach (Transform obj in o.transform)
                        {
                            if (obj.transform.gameObject.GetComponent<Renderer>())
                            {
                                obj.transform.gameObject.GetComponent<Renderer>().enabled = false;
                            }
                        }
                    }
                }
            }
        }


        void InitObj()
        {
            UpdatePlaceholders();
            
            initScale = transform.localScale;

            for (int i = 0; i < targetObjects.Count; i++)
            {
                GameObject g = targetObjects[i];

                if (g != null)
                {
                    if (modObjects.Count > i)
                    {
                        contentSelector = modObjects[i];
                    }

                    if (contentSelector)
                    {
                        AddModObjects(g, contentSelector);
                    }
                }

            }

            isInit = true;
        }


        private void OnEnable()
        {
            if (positionNoise)
            {
                noiseSeed = transform.localPosition;
            }

            if (!isInit)
            {
                InitObj();
            }

            transform.localScale = new Vector3(
                initScale.x * scaleMultiplier,
                initScale.y * scaleMultiplier,
                initScale.z * scaleMultiplier
            );

        }


    }

}
