using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;
    public Spawner spawner;
    public Blade blade;
    public SpawnElement spawnElement;
    public UIHandler uiHandler;
    public DataHandler dataHandler;
    public NetworkHandler networkHandler;

    private bool isGameStart;
    [SerializeField] private bool initialRun = true;

    public bool isCountDown;
    public int playerScore;
    public GameObject fruitSpawner;
    public float countDownTime; //total countdown time
    [HideInInspector] public float cdTime; //countdown timer--
    [HideInInspector] public float frenzyTime;
    public float time;

    APIManager apiManager;
    [SerializeField] bool isHyperNation;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        blade = FindObjectOfType<Blade>();
        spawnElement = FindObjectOfType<SpawnElement>();
        uiHandler = FindObjectOfType<UIHandler>();
        dataHandler = FindObjectOfType<DataHandler>();
        networkHandler = FindObjectOfType<NetworkHandler>();
        apiManager = FindObjectOfType<APIManager>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(ResetGameCo());
    }

    private void Update()
    {
        if (isCountDown)
        {
            if (cdTime > 0f)
            {
                cdTime -= Time.deltaTime;
            }
            else
            {
                isCountDown = false;
                isGameStart = true;
                spawner.StartSpawn(); //activate spawner
            }
        }
        if(isGameStart)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                time = 0f;
                EndGame();
                Debug.Log("TIME'S UP");
            }
            if (time <= frenzyTime)
            {
                spawner.frenzyMode = true;
            }
        }
    }

    public void StartGame()
    {
        StartGameCo();
    }

    public void StartGameCo()
    {
        Time.timeScale = 1f;
        fruitSpawner.SetActive(true);
        spawner.enabled = true;
        blade.enabled = true;

        cdTime = countDownTime;
        isCountDown = true;
        isGameStart = false;

        uiHandler.ToggleDisplay("UserUIPanel", false);
    }

    public IEnumerator ResetGameCo()
    {
        if(initialRun)
        {
            uiHandler.ToggleDisplay("TutorialPanel", true);
            initialRun = false;
        }

        if(spawnElement.accumulatedWeight != 0)
        {
            spawnElement.accumulatedWeight = 0;
        }

        if (time <= 0 || time != 0)
        {
            time = 10f;
        }

        if(playerScore != 0)
        {
            playerScore = 0;
        }

        uiHandler.ToggleDisplay("UserUIPanel", true);
        uiHandler.ToggleDisplay("LoadingPanel",true);
        yield return APIManager.instance.PostToGetGameDataCo();

        uiHandler.ToggleDisplay("LoadingPanel",false);

        if (!string.IsNullOrEmpty(networkHandler.errorMessage))
        {
            uiHandler.ToggleDisplay("ErrorPanel", true, false, networkHandler.errorMessage);
        }
        else
        {
            for (int i = 0; i < dataHandler.gameData.fruits.Length; i++)
            {
                dataHandler.gameData.fruits[i].FruitPrefab = Resources.Load<GameObject>(dataHandler.gameData.fruits[i].Name);
            }
        }
    }

    public void EndGame()
    {
        fruitSpawner.SetActive(false);
        isGameStart = false;
        spawner.enabled = false;
        blade.enabled = false;
        StartCoroutine(SubmitScore());
    }

    public IEnumerator SubmitScore()
    {
        if (isHyperNation)
        {
            GameResult gameResult = new GameResult
            {
                userName = "",
                score = playerScore
            };
            uiHandler.UpdateResultUI();
            uiHandler.ToggleDisplay("ResultPanel", true);

            uiHandler.ToggleDisplay("LoadingPanel", true);

            yield return APIManager.instance.PostEndGameScoreCo(gameResult);
            uiHandler.ToggleDisplay("LoadingPanel", false);
            if (!string.IsNullOrEmpty(networkHandler.errorMessage))
            {
                uiHandler.ToggleDisplay("ErrorPanel", true, false, networkHandler.errorMessage);
            }
            else
            {
                StartCoroutine(ResetGameCo());
            }
        }

        else
        {
            //ScoreModel scoreModel = new ScoreModel
            //{ userName = APIManager.instance.dataHandler.userData.userModel.userName,
            //score = playerScore};
            uiHandler.UpdateResultUI();
            uiHandler.ToggleDisplay("ResultPanel", true);

            uiHandler.ToggleDisplay("LoadingPanel", true);

            //yield return APIManager.instance.PostFinalScoreCo(scoreModel);
            uiHandler.ToggleDisplay("LoadingPanel", false);
            if (!string.IsNullOrEmpty(networkHandler.errorMessage))
            {
                uiHandler.ToggleDisplay("ErrorPanel", true, false, networkHandler.errorMessage);
            }
            else
            {
                StartCoroutine(ResetGameCo());
            }
        }
    }
}
