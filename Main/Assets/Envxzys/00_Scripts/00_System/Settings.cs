using UnityEngine;
using System.Collections.Generic;

namespace Envxzys
{
    public class Settings : MonoBehaviour
    {

        public enum environmentTypeValues
        {
            DynamicXZ = 1, StaticXZ = 2, DynamicXYZ = 3, StaticXYZ = 4
        }

        public enum updateModeValues
        {
            Static = 1, Instant = 2
        }

        public enum cameraProjectionValues
        {
            Orthographic = 1, Perspective = 2
        }

        public enum rootTileCountVaules
        {
            _4x4 = 2, _6x6 = 3, _8x8 = 4, _10x10 = 5, _12x12 = 6, _14x14 = 7, _16x16 = 8, _18x18 = 9, _20x20 = 10,
            _22x22 = 11, _24x24 = 12, _26x26 = 13, _28x28 = 14, _30x30 = 15, _40x40 = 20, _50x50 = 25
        }

        public enum tileContentVaules
        {
            _1x1 = 1, _2x2 = 2, _3x3 = 3, _4x4 = 4
        }

        public enum NoiseTypes
        {
            BasicPerlin = 1, PerlinSine_A = 2, PerlinSine_B = 3, Random = 4
        }


        //------------------------------------------------------------------------------------
        [Header("System")]
        //------------------------------------------------------------------------------------

        [Space(5)]

        [Tooltip("Initial camera projection")]
        public cameraProjectionValues initialProjection = cameraProjectionValues.Orthographic;
        public GameObject navigationObject;

        [Space(10)]

        [Tooltip("Continuously updated or static environment")]
        public environmentTypeValues environmentType = environmentTypeValues.DynamicXZ;

        [Tooltip("Update environment continuously or when no user input is detected")]
        public updateModeValues updateMethod = updateModeValues.Instant;

        [Tooltip("Update frequency for user input and position")]
        [Range(0f, 1f)] public float mainUpdateInterval = 0.1f;

        [Tooltip("Minimum delay before updating tiles, max will depend on tile/content count")]
        [Range(0f, 1f)] public float tileUpdateDelay = 0.01f;

        [Space(10)]

        //------------------------------------------------------------------------------------
        [Header("Content")]
        //------------------------------------------------------------------------------------

        [Space(5)]

        [Tooltip(("Prefab paths (relative to any project directory named Resources)"))]
        public List<string> prefabPaths = new List<string>() { "" };

        [Tooltip(("Texture paths (relative to any project directory named Resources)"))]
        public List<string> texturePaths = new List<string>() { "01_Textures" };

        [Space(5)]

        [Tooltip("Game object to be used as root tile")]
        public GameObject rootTileObject;

        [Tooltip("Game object to be used as content selector")]
        public GameObject contentSelector;

        [Tooltip("Toggles the mesh renderer for root tiles")]
		public bool renderTiles = true;

        [Tooltip("Scale value for root tiles")]
        [Range(0.1f, 100f)] public float rootTileSize = 1f;

        [Space(5)]

        [Tooltip("Number of root tiles to be instantiated")]
        //public rootTileCountVaules mapSize = rootTileCountVaules._20x20;
        [Range(4, 100)] public int mapSize = 4;

        [Tooltip("Number of content selectors on each tile")]
        public tileContentVaules contentCount = tileContentVaules._2x2;

        [Space(5)]

        [Tooltip("The probability value for tile-content activation")]
        [Range(1, 100)] public int probability = 50;

        [Tooltip("Apply extra noise to content probability")]
        public bool alternate = false;

        [Space(10)]

        //------------------------------------------------------------------------------------
        [Header("Dynamics")]
        //------------------------------------------------------------------------------------

        [Space(5)]

        [Tooltip("Rotate content using the chosen noise method")]
        public bool rotation = false;

        [Space(5)]

        [Tooltip("Scale content using the chosen noise method")]
        public bool scale = true;

        [Tooltip("Minimum scale value for content root object")]
        [Range(0.01f, 1000)] public float minSize = 20;

        [Tooltip("Maximum scale value for content root object")]
        [Range(0.01f, 1000)] public float maxSize = 100;

        [Space(10)]

        //------------------------------------------------------------------------------------
        [Header("Noise")]
        //------------------------------------------------------------------------------------

        [Space(5)]

        [Tooltip("Noise scale multiplier")]
        public float noiseScale = 0.987654321f;

        [Tooltip("Main noise method, may affect content activation and customization")]
        public NoiseTypes noiseType = NoiseTypes.BasicPerlin;

        [Tooltip("Position noise offset")]
        public Vector3 noiseOffset = new Vector3(0, 0, 0);
        [Space(5)]

        [Tooltip("Random seed for random noise types")]
        public int randomSeed = 0;
    }
}