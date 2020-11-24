using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Misc
{
    public class RndSfx : MonoBehaviour
    {
        [Space(5)]

        public float maxVolume = 0.5f;
        public float repeatRate = 1;

        [Space(5)]

        public bool randomRepeatRate = true;
        public float minRepeatRate = 0.5f;
        public float maxRepeatRate = 1;

        [Space(5)]

        public bool randomPitch = true;
        public float minPitch = 1f;
        public float maxPitch = 2f;

        [Space(5)]

        public bool indicateState = true;
        public bool randomColor = true;
        public Color aciveColor = new Color(0, 1, 0);

        public bool nameColor = false;
        public List<string> clipFolders = new List<string>();

        private List<AudioClip> aClips = new List<AudioClip>();

        private AudioClip currentClip;
        private Vector3 objSize;
        private Color objColor;



        private string clipName;

        [HideInInspector]
        public bool isInit = false;


        void InitContent(GameObject rootObj, List<string> gameObjectPaths)
        {
            foreach (string path in gameObjectPaths)
            {
                if (path != "")
                {
                    AudioClip[] prefabFiles = Resources.LoadAll<AudioClip>(path);
                    foreach (AudioClip a in prefabFiles)
                    {
                        aClips.Add(a);
                    }
                }
            }

            isInit = true;
        }



        void PlaySample()
        {
            transform.GetComponent<AudioSource>().PlayOneShot(currentClip);
        }


        private void Update()
        {
            if (transform.GetComponent<AudioSource>())
            {
                transform.GetComponent<AudioSource>().volume = maxVolume;

                if (indicateState)
                {
                    if (transform.GetComponent<AudioSource>().isPlaying)
                    {
                        transform.GetComponent<Renderer>().material.color = aciveColor;
                    }
                    else
                    {
                        transform.GetComponent<Renderer>().material.color = objColor;
                    }
                }

            }
        }

        private void OnEnable()
        {
            if (!transform.GetComponent<AudioSource>())
            {
                transform.gameObject.AddComponent<AudioSource>();
            }

            if (!isInit)
            {
                InitContent(transform.gameObject, clipFolders);
                int idx = Random.Range(0, Mathf.Abs(aClips.Count - 1));

                clipName = aClips[idx].name;

                if (randomColor)
                {
                    float r = Random.Range(0f, 1f);
                    float g = Random.Range(0f, 1f);
                    float b = Random.Range(0f, 1f);
                    aciveColor = new Color(r, g, b);
                }

                currentClip = aClips[idx];

                if (randomPitch)
                {
                    float p = Random.Range(minPitch, maxPitch);
                    transform.gameObject.GetComponent<AudioSource>().pitch = p;
                }
            }


            if (transform.GetComponent<Renderer>())
            {
                objColor = transform.GetComponent<Renderer>().material.color;
            }


            if (transform.GetComponent<AudioSource>() && currentClip)
            {

                if (randomRepeatRate)
                {
                    repeatRate = Random.Range(minRepeatRate, maxRepeatRate);
                }

                InvokeRepeating("PlaySample", repeatRate, repeatRate);
            }


        }


    }
}