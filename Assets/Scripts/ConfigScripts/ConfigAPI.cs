using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;

[CreateAssetMenu(fileName = "API", menuName = "ConfigAPI", order = 0)]
public class ConfigAPI : ScriptableObject
{
    private string _idCode;
    private const string APIUrl = "http://localhost";
    private UserData _userData;
    private SkinsArray _skinsArray;
    
    public UserData UserData => _userData;
    public SkinsArray SkinsArray => _skinsArray;
    
    public string IdCode
    {
        set => _idCode = value;
    }

    public string GetApiUrl => APIUrl;

    public void ClearData()
    {
        _userData = new UserData();
        _skinsArray.skins = new SkinData[] { };
    }

    /*
     *      GET /
     */
    public IEnumerator APITest()
    {
        string url = $"{APIUrl}/";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    
    /*
     *      GET /auth
     *      id_code = string
     *      data = string (user, skins)
     */
    public IEnumerator GetAuth(string data)
    {
        string url = $"{APIUrl}/auth?id_code={_idCode}&data={data}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Erreur de connexion : {request.error}");
                yield break;
            }

            if (request.responseCode == 400)
            {
                Debug.LogError("Requête incorrecte. Voir Format Erreurs 400");
                yield break;
            }
            else if (request.responseCode == 403)
            {
                Debug.LogError("Token invalide");
                yield break;
            }
            else if (request.responseCode == 500)
            {
                Debug.LogError("Erreur interne. Voir Format Erreurs 500");
                yield break;
            }
            else if (request.responseCode == 200)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Obtention de la donnée demandée");
                if (data == "user")
                {
                    _userData = new UserData();
                    _userData = JsonUtility.FromJson<UserData>(jsonResponse);
                }
                else if (data == "skins")
                {
                    _skinsArray.skins = new SkinData[] { };
                    _skinsArray = JsonUtility.FromJson<SkinsArray>("{\"skins\":" + jsonResponse + "}");
                }
            }
        }

        yield return null;
    }
}

[System.Serializable]
public class UserData
{
    public string login;
    public string username;
    public string id_code;
    public int points;
    public bool is_admin;
}

[Serializable]
public class SkinsArray
{
    public SkinData[] skins;
}

[Serializable]
public class SkinData
{
    public int id;
    public int id_boat;
    public string name;
    public string identifier;
    public string boat_name;
    public string boat_identifier;
}