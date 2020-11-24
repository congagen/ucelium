using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

namespace Moduli

{

    [System.Serializable]
    public class ResponseData
    {
        public string response;

        public static ResponseData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ResponseData>(jsonString);
        }
    }


    [System.Serializable]
    public class Message
    {
        public Text textObject;

        public bool userMsg;
        public string msgText;
    }


    public class Dialogue : MonoBehaviour
    {

        private bool isInit = false;


        public string apiUrl = "";
        public string payloadKey = "request";

        public List<string> apiHeaderKeys;
        public List<string> apiHeaderVals;

        [Space(15)]

        public string userName = "User";
        public string agentName = "Agent";

        [Space(15)]

        public string greetingMsg = "Hello!";
        public string fallbackMsg = "Wha?";

        [Space(15)]

        public InputField inputField;
        public InputField convField;

        private List<string> conversation = new List<string>();


        void RefreshChat(string agentName, string respData, bool userMsg)
        {
            string convString = string.Join("\n", conversation.ToArray());
            convField.text = convString;
        }


        string MockResponse(string msg)
        {
            string resp = agentName + ": " + fallbackMsg;
            conversation.Insert(0, resp);

            return resp;
        }


        IEnumerator PostReq(string url, string reqKey, string reqVal)
        {
            var request = new UnityWebRequest(url, "POST");
            string reqJsonString = "{ \"" + reqKey + "\" : \"" + reqVal + "\" }";

            byte[] bodyRaw = Encoding.UTF8.GetBytes(reqJsonString);

            if (apiHeaderKeys.Count == apiHeaderVals.Count)
            {
                for (int i = 0; i < Mathf.Abs(apiHeaderKeys.Count - 1); i++)
                {
                    if (apiHeaderKeys[i] != null && apiHeaderVals[i] != null)
                    {
                        request.SetRequestHeader(
                            apiHeaderKeys[i],
                            apiHeaderVals[i]
                        );
                    }
                }
            }

            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                byte[] pLoadData = request.downloadHandler.data;
                string pLoadText = request.downloadHandler.text;

                Debug.Log(pLoadText);

                ResponseData respObj = new ResponseData();
                var respData = JsonUtility.FromJson<ResponseData>(pLoadText);
                string r_val = respData.response;

                Debug.Log("SUCCESS? -> " + r_val);

                RefreshChat(agentName, respData.response, false);
            }
            else
            {
                Debug.Log("ERROR: " + request.responseCode.ToString());

                RefreshChat(agentName, fallbackMsg, false);

            }
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                RefreshChat(userName, inputField.text.ToString(), true);

                StartCoroutine(PostReq(apiUrl, payloadKey, inputField.text.ToString()));

                inputField.text = "";
            }

            if (!inputField.isFocused)
            {
                inputField.Select();
                inputField.ActivateInputField();
            }

        }


        private void Start()
        {
            if (!isInit)
            {
                RefreshChat(agentName, greetingMsg, false);
                isInit = true;


            }
        }
    }

}