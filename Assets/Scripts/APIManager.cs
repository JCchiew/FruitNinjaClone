using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System.Drawing;
//using static GameModel;


[RequireComponent(typeof(DataHandler))]
[RequireComponent(typeof(NetworkHandler))]
[RequireComponent(typeof(EncryptionHandler))]
public class APIManager : MonoBehaviour
{
    public static APIManager instance;

    public DataHandler dataHandler;
    public NetworkHandler networkHandler;
    //public SceneHandler sceneHandler;
    public UIHandler uiHandler;
    public EncryptionHandler encryptionHandler;

    public bool isRunning;

    object webResponse;

    public string json;

    [DllImport("__Internal")]
    private static extern string GetToken(string key);

    private void Awake()
    {
        encryptionHandler = FindObjectOfType<EncryptionHandler>();

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        dataHandler = GetComponent<DataHandler>();
    }

    public IEnumerator PostToGetUserModelCo(UserModel code)
    {
        isRunning = true;
        yield return new WaitUntil(() => !networkHandler.isRunning);

        string token = GetToken("Token"); //CHANGE BACK TO THIS WHEN DEPLOY

        //string token = "QPFJZvrGSgLcpS-vnrNl6OSUhyXnjIMwrRTQ1dEPO6Ekm8hTMBZO13pnMN49DHzzzIAtgZOqcYuqWwnHJ_JGgFRs-daA5UZzcdZKDbCZZIFMpjDZXgovcpdwKjkIcIuR6f0XcfvrBByhBuSNb4laKAiy0bns9PIHIr25pDInIONbdvOOBUjV87ZF97uDoTyRHRZIWlaOkPCMnpJxTv3JB7TF0pmTdyCFQUbLE45SAtDPVoEm";

        CheckUserToken(token);

        string url = $"http://glaunch.crococodile.biz/GameSetting";
        code = dataHandler.userData.userModel;

        //sceneHandler.DisplayLoading(true);

        yield return networkHandler.PostRequestCo<UserModel>(token, url, code, false);

        if (webResponse != null && webResponse.GetType() == typeof(UserModel))
        {
            UserModel response = (UserModel)webResponse;

            //CheckResponseCode(response);

            //convert string to double
            //double chance = response.@return.balance;
            dataHandler.userData.userModel.Chances = (float)response.Chances;

            //Deserialize Config into prize array
            //if (!string.IsNullOrEmpty(response.@return.config))
            //{
            //    PrizeList items = JsonConvert.DeserializeObject<PrizeList>(response.@return.config);

            //    dataHandler.wheelData.wheelPieces = new Prize[items.prize.Length];
            //    for (int i = 0; i < items.prize.Length; i++)
            //    {
            //        dataHandler.wheelData.wheelPieces[i] = items.prize[i];
            //    }
            //}
            webResponse = null;
        }
        //sceneHandler.DisplayLoading(false);
        isRunning = false;
    }

    public IEnumerator PostToGetGameDataCo()
    {
        isRunning = true;
        yield return new WaitUntil(() => !networkHandler.isRunning);

        //string token = GetToken("Token"); //CHANGE BACK TO THIS WHEN DEPLOY

        string token = "QPFJZvrGSgLcpS-vnrNl6OSUhyXnjIMwrRTQ1dEPO6Ekm8hTMBZO13pnMN49DHzzzIAtgZOqcYuqWwnHJ_JGgFRs-daA5UZzcdZKDbCZZIFMpjDZXgovcpdwKjkIcIuR6f0XcfvrBByhBuSNb4laKAiy0bns9PIHIr25pDInIONbdvOOBUjV87ZF97uDoTyRHRZIWlaOkPCMnpJxTv3JB7TF0pmTdyCFQUbLE45SAtDPVoEm";

        //CheckUserToken(token);

        string url = "https://my-json-server.typicode.com/KazT452/demo/db";
        //code = dataHandler.userData.code;

        //sceneHandler.DisplayLoading(true);
        uiHandler.ToggleDisplay("LoadingPanel", true);

        var jsonText = DecryptJson(json);

        webResponse = jsonText;

        //yield return networkHandler.GetRequestCo<UserModel>(token, url, true);

        if (webResponse != null && webResponse.GetType() == typeof(UserModel))
        {
            UserModel response = (UserModel)webResponse;

            dataHandler.userData.userModel.Chances = response.Chances;
            //convert string to double
            dataHandler.gameData.fruits = response.Items;
            GameHandler.instance.frenzyTime = response.Frenzy;

            webResponse = null;
        }
        //sceneHandler.DisplayLoading(false);
        uiHandler.ToggleDisplay("LoadingPanel", false);

        isRunning = false;
    }

    public IEnumerator PostEndGameScoreCo(GameResult gameResult)
    {
        isRunning = true;

        yield return new WaitUntil(() => !networkHandler.isRunning);

        string token = "";

        gameResult.userName = "MayaKyle"; //remove after testing

        string json = JsonConvert.SerializeObject(gameResult);

        string signature = encryptionHandler.AESEncryption(json);

        string url = $"https://hypernations.tk/api/game/mg/fn?mg={signature}";


        uiHandler.ToggleDisplay("Loading", true);

        yield return networkHandler.PostRequestCo<GameResult>(token, url, signature);

        if (webResponse != null)
        {
            webResponse = null;
        }

        uiHandler.ToggleDisplay("Loading", false);
        isRunning = false;
    }

    //public IEnumerator PostFinalScoreCo(ScoreModel score)
    //{
    //    isRunning = true;
    //    yield return new WaitUntil(() => !networkHandler.isRunning);

    //    string token = GetToken("Token"); //CHANGE BACK TO THIS WHEN DEPLOY

    //    //string token = "QPFJZvrGSgLcpS-vnrNl6OSUhyXnjIMwrRTQ1dEPO6Ekm8hTMBZO13pnMN49DHzzzIAtgZOqcYuqWwnHJ_JGgFRs-daA5UZzcdZKDbCZZIFMpjDZXgovcpdwKjkIcIuR6f0XcfvrBByhBuSNb4laKAiy0bns9PIHIr25pDInIONbdvOOBUjV87ZF97uDoTyRHRZIWlaOkPCMnpJxTv3JB7TF0pmTdyCFQUbLE45SAtDPVoEm";

    //    CheckUserToken(token);

    //    string url = "Insert URL here";

    //    //sceneHandler.DisplayLoading(true);
    //    uiHandler.ToggleDisplay("LoadingPanel", true);

    //    yield return networkHandler.PostRequestCo<ScoreModel>(token, url, score, true);

    //    if (webResponse != null)
    //    {
    //        //convert string to int
    //        //int prizeID = int.Parse(prizes.ID);

    //        //dataHandler.userData.chances -= 1;

    //        //dataHandler.wheelData.resultID = prizeID;

    //        //dataHandler.wheelData.resultName = prizes.Name;

    //        webResponse = null;
    //    }

    //    //sceneHandler.DisplayLoading(false);
    //    uiHandler.ToggleDisplay("LoadingPanel", false);
    //    isRunning = false;
    //}

    //void CheckResponseCode(GameScoreRS response)
    //{
    //    if (response.responseCode == "000000")
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        string errorTxt = $"INTERNAL SERVER ERROR ({response.responseCode})";
    //        networkHandler.HandleError(errorTxt, true);
    //    }
    //}

    //void CheckResponseCode(GameSettingRS response)
    //{
    //    if (response.responseCode == "000000")
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        string errorTxt = $"INTERNAL SERVER ERROR ({response.responseCode})";
    //        networkHandler.HandleError(errorTxt, false);
    //    }
    //}

    public void CheckUserToken(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            return;
        }
        else
        {
            string errorTxt = "INTERNAL SERVER ERROR (TOKEN IS NULL OR EMPTY)";
            networkHandler.HandleError(errorTxt, false);
        }
    }

    //void StoreUserData(UserModel response)
    //{
    //    dataHandler.userData.userName = response.userName;
    //    dataHandler.userData.chances = response.chance;
    //}

    public object SetWebResponseResult
    {
        set { webResponse = value; }
    }

    //public SceneHandler SetSceneHandler
    //{
    //    set { sceneHandler = value; }
    //}

    //public IEnumerator GetUserModelCo()
    //{
    //    yield return new WaitUntil(() => !networkHandler.isRunning);

    //    string userName = dataHandler.userData.userName;
    //    string token = GetToken("Token");
    //    string url = "";

    //    sceneHandler.DisplayLoading(true);

    //    yield return networkHandler.GetRequestCo<bool>(token, url);

    //    if (webResponse != null && webResponse.GetType() == typeof(UserModel))
    //    {
    //        UserModel response = (UserModel)webResponse;

    //        StoreUserData(response);
    //        webResponse = null;
    //    }
    //    sceneHandler.DisplayLoading(false);
    //}

    //public IEnumerator GetPrizeListCo()
    //{
    //    yield return new WaitUntil(() => !networkHandler.isRunning);

    //    string token = "insert token here";
    //    string url = "insert url here";

    //    sceneHandler.DisplayLoading(true);

    //    yield return networkHandler.GetRequestCo<GameSettingRS>(token, url);

    //    if (webResponse != null && webResponse.GetType() == typeof(GameSettingRS))
    //    {
    //        GameSettingRS response = (GameSettingRS)webResponse;
    //        if (string.IsNullOrEmpty(response.@return.config))
    //        {
    //            Prize[] items = JsonConvert.DeserializeObject<Prize[]>(response.@return.config);
    //            dataHandler.wheelData.wheelPieces = new Prize[items.Length];

    //            for (int i = 0; i < items.Length; i++)
    //            {
    //                dataHandler.wheelData.wheelPieces[i] = items[i];
    //            }
    //        }
    //        webResponse = null;
    //    }
    //    sceneHandler.DisplayLoading(false);
    //}
    static UserModel DecryptJson(string json)
    {
        return JsonUtility.FromJson<UserModel>(json);
    }
}