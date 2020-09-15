using UnityEngine;
using UnityEditor;
using System.Collections;


namespace Moduli
{
    public static class MainMenu
    {
        // -------------------------------------------------------------------------
        // Objects:Agents:

        [MenuItem("Moduli/Add/Selector", false, 12)]
        static void NewSelector()
        {
            Moduli.AddObj addObjScript = (Moduli.AddObj)new Moduli.AddObj();
            addObjScript.createObject("Selector");
        }

        [MenuItem("Moduli/Add/Manager", false, 12)]
        static void NewManager()
        {
            Moduli.AddObj addObjScript = (Moduli.AddObj)new Moduli.AddObj();
            addObjScript.createObject("Manager");
        }

        [MenuItem("Moduli/Add/Examples/Misc", false, 12)]
        static void NewAgentLegHead()
        {
            Moduli.AddObj addObjScript = (Moduli.AddObj)new Moduli.AddObj();
            addObjScript.createObject("Agents/Misc");
        }

        [MenuItem("Moduli/Add/Examples/Cat", false, 12)]
        static void NewModulinimal()
        {
            Moduli.AddObj addObjScript = (Moduli.AddObj)new Moduli.AddObj();
            addObjScript.createObject("Agents/Cat");
        }

        [MenuItem("Moduli/Add/Examples/Being", false, 12)]
        static void NewAgentHumanoid()
        {
            Moduli.AddObj addObjScript = (Moduli.AddObj)new Moduli.AddObj();
            addObjScript.createObject("Agents/Being");
        }

        [MenuItem("Moduli/Add/Examples/Environment", false, 12)]
        static void NewSwarm()
        {
            Moduli.AddObj addObjScript = (Moduli.AddObj)new Moduli.AddObj();
            addObjScript.createObject("Environment");
        }

        // -------------------------------------------------------------------------
        // Objects:Navigation:

        [MenuItem("Moduli/Add/Navigation/User", false, 12)]
        static void NewUserNavObject()
        {
            Moduli.AddObj addObjScript = (Moduli.AddObj)new Moduli.AddObj();
            addObjScript.createObject("NavObject_User");
        }

        [MenuItem("Moduli/Add/Navigation/Agent", false, 12)]
        static void NewAgentNavObject()
        {
            Moduli.AddObj addObjScript = (Moduli.AddObj)new Moduli.AddObj();
            addObjScript.createObject("NavObject_Agent");
        }

    }
}