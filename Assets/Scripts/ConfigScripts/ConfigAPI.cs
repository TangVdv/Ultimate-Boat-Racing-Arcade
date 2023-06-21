using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "API", menuName = "ConfigAPI", order = 0)]
public class ConfigAPI : ScriptableObject
{
    
    /*
     *      API TEST
     */
    public IEnumerator GetDataTest(Action<string> callback)
    {
        string uri = "https://api.chucknorris.io/jokes/random";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                callback(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                callback(json);
            }
        }
    }

    /*
     *      GET /auth
     *      id_code = string
     *      data = skins
     */
    public IEnumerator GetSkins(Action<string> callback, string idCode)
    {
        string uri = "/auth?id_code=" + idCode + "&data=skins";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error : "+request.error);
                callback(null);
            }
            else
            {
                string json = request.downloadHandler.text;
                callback(json);
            }
        }
    }
}
