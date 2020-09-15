using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Envxzys
{
    public class ResourceManager : MonoBehaviour 
    {

        public GameObject EnvxzysSettingsObject;

        [HideInInspector]

        public bool sessionDataReady = false;
		public List<GameObject> projectPrefabObjecstList;
		public List<Texture> projectTextureList;


		void LoadGameObjects(List<string> gameObjectPaths) 
		{
			foreach (string path in gameObjectPaths) 
			{
				if (path != "") 
				{
					GameObject[] prefabFiles = Resources.LoadAll<GameObject> (path);
					foreach (GameObject ob in prefabFiles) 
					{
						if(ob.gameObject.GetComponent<Renderer>() != null)
						{
							projectPrefabObjecstList.Add (ob);
						}
					}
				} 
			}

		}
			

		void LoadTextures(List<string> texturePaths) 
		{
			if(texturePaths.Count > 0)
			{
				foreach (string path in texturePaths) 
				{
					if (path != "") 
					{
						Texture[] textureFiles = Resources.LoadAll<Texture> (path);
						foreach (Texture ob in textureFiles) 
						{
							projectTextureList.Add (ob);
						}
					}
				}
			}
		}

			
		void Start() 
		{
			List <string>  prefabPaths = EnvxzysSettingsObject.GetComponent<Envxzys.Settings> ().prefabPaths;
			LoadGameObjects (prefabPaths);

			List <string> texturePaths = EnvxzysSettingsObject.GetComponent<Envxzys.Settings> ().texturePaths;
			LoadTextures (texturePaths);

			Debug.Log ("Textures loaded: " + projectTextureList.Count.ToString());
			Debug.Log ("GameObjects loaded: " + projectPrefabObjecstList.Count.ToString());

			sessionDataReady = true;
		}

	}
}