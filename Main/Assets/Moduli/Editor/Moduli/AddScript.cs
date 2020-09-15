using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Moduli
{
    public class AddScript : EditorWindow
    {
        public void addScript(string scriptID)
        {

            GameObject obj = Selection.activeGameObject;

            if (obj)
            {
                if (scriptID == "nav")
                {
                    obj.AddComponent<Moduli.ObjectNavigation>();
                }

                if (scriptID == "hoover")
                {
                    obj.AddComponent<Misc.Hoover>();
                }

                if (scriptID == "rotate")
                {
                    obj.AddComponent<Misc.Rotate>();
                }

            }

        }
    }
}