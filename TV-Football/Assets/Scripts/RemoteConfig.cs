using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Loads remote configuration data.
/// </summary>
[DefaultExecutionOrder(-99)]
public class RemoteConfig : MonoBehaviour
{
    [TextArea(1, 10)]
    public string jsonData;
    [Tooltip("Data that got loaded")]
    public RemoteConfigData remoteConfigData;

    public Components components;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(JsonUtility.ToJson(remoteConfigData, true));
        LoadData(jsonData);
    }

    /// <summary>
    /// If you want to load the json through the web
    /// </summary>
    /// <param name="url"></param>
    public void GetDataFromUrl(string url)
    {
        StartCoroutine(GetDataFromUrlAsync(url));
    }

    /// <summary>
    /// Load the config data from json string
    /// </summary>
    /// <param name="json"></param>
    public void LoadData(string json)
    {
        remoteConfigData = JsonUtility.FromJson<RemoteConfigData>(json);
        components.penaltyKick.values.ballMaxAliveTime = remoteConfigData.ballMaxAliveTime;
        components.football.values.gravity = remoteConfigData.ballGravity;
        components.pointsController.values.defaultAddScore = remoteConfigData.scorePerGoal;
        components.gameManager.values.gameTime = remoteConfigData.gameTime;
        components.timer.values.countDown = remoteConfigData.gameTime;
    }

    private IEnumerator GetDataFromUrlAsync(string url)
    {
        using(UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // Show results as text
                string json = www.downloadHandler.text;
                LoadData(json);                
            }
        }
    }

    [System.Serializable]
    public class Components
    {
        public PenaltyKick penaltyKick;
        public Football football;
        public PointsController pointsController;
        public GameManager gameManager;
        public Timer timer;
    }
}
