using UnityEngine;
using UnityEditor;

using System.Collections;


namespace Moduli
{
    public class AddObj : MonoBehaviour
    {
        public void createObject(string prefabName)
        {
            GameObject instance = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
            instance.name = prefabName;
        }
    }
}