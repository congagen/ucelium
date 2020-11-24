using UnityEngine;
using System.Collections;


namespace Misc 
{
	public class NoiseTexture : MonoBehaviour {

		public Color colorA = Color.green;
		public Color colorB = Color.blue;
		[Range(0f, 1f)] public float colorThreshold = 0.5f;

		[Space(5)]

		public int textureSize = 10;
		public bool dynamicTextureSize = false;

		[Space(5)]

		public FilterMode textureFileter = FilterMode.Point; 

		[Space(5)]

		public float minUpdateDelay = 0.01f;
		public float maxUpdateDelay = 0.1f;

		private Texture2D currentTexture;
		private float updateClock;
		private float updateDelay;
		public bool liveUpdate = false;


		void UpdateTexture() 
		{
			if (dynamicTextureSize) 
			{
				int dynam_texzi = Mathf.RoundToInt(Mathf.Abs(Mathf.Sin ((transform.position.x + transform.position.z) * 0.5f)) * (textureSize)) + 3;
				currentTexture = new Texture2D(dynam_texzi, dynam_texzi, TextureFormat.RGBAHalf, false);
			} 
			else 
			{
				currentTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBAHalf, false);
			}


			for (int h = 0; h < textureSize; h++)
			{
				for (int w = 0; w < textureSize; w++) 
				{

					if (Mathf.PerlinNoise((transform.position.x * (h)) * 0.5f, (transform.position.z * (w)) * 0.5f) > colorThreshold)
					{
						currentTexture.SetPixel(h, w, colorA);
					} 
					else
					{
						currentTexture.SetPixel(h, w, colorB);
					}

					if(liveUpdate){
						currentTexture.Apply();
						GetComponent<Renderer>().material.mainTexture = currentTexture;
					}
				}
			}

			if(!(liveUpdate))
			{
				currentTexture.Apply();
				GetComponent<Renderer>().material.mainTexture = currentTexture;
			}

			transform.GetComponent<Renderer> ().material.mainTexture.filterMode = textureFileter;
		}


		void OnEnable(){
            Invoke("UpdateTexture", Random.Range(minUpdateDelay, maxUpdateDelay * textureSize));
		}

	}
}