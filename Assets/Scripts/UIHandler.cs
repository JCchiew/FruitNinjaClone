using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    GameHandler gameHandler;
    DataHandler dataHandler;

    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private TextMeshProUGUI countdownText;
    [SerializeField]
    private TextMeshProUGUI chanceText;
    [SerializeField]
    private TextMeshProUGUI errorText;
    [SerializeField]
    private GameObject errorButton;
    public Image uiPanelImg;
    public GameObject gameOverPanel; //could be refactor
    public GameObject countDownPanel;//could be refactor
    [Space]
    public GameObject[] panelArray;

    private void Awake()
    {
        gameHandler = FindObjectOfType<GameHandler>();
        dataHandler = FindObjectOfType<DataHandler>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        chanceText.text = $"Chances: {dataHandler.userData.userModel.Chances.ToString()}";
        countdownText.text = gameHandler.cdTime.ToString("F0");
        timerText.text = gameHandler.time.ToString("F2");
        scoreText.text = gameHandler.playerScore.ToString();
        CountDown();
    }

    public void Fade()
    {
        StartCoroutine(FadeSequence());
    }

    public IEnumerator FadeSequence()
    {
        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            uiPanelImg.color = Color.Lerp(Color.clear, Color.white, t);

            //Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        gameOverPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        gameOverPanel.SetActive(false);

        //gameHandler.ResetGameCo();

        elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            uiPanelImg.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }

    public void CountDown()
    {
        if (gameHandler.isCountDown && gameHandler.cdTime > 0f)
        {
            countDownPanel.SetActive(true);
        }
        else
        {
            countDownPanel.SetActive(false);
        }
    }

    public void ToggleDisplay(string name, bool flag)
    {
        for (int i = 0; i < panelArray.Length; i++)
        {
            if (panelArray[i].name == name)
            {
                GameObject panel = panelArray[i];
                if (panel != null)
                {
                    if (flag == true)
                    {
                        panel.SetActive(true);
                    }
                    else
                    {
                        panel.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError($"Object not found: {panel}");
                }
                break;
            }
        }
    }

    public void ToggleDisplay(string name, bool flag, bool withButton, string errorMsg) //withButton > error WITH button or WITHOUT button
    {
        for (int i = 0; i < panelArray.Length; i++)
        {
            if (panelArray[i].name == name)
            {
                GameObject panel = panelArray[i];
                if (panel != null)
                {
                    if (flag == true)
                    {
                        errorButton.SetActive(withButton);
                        errorText.text = errorMsg; 
                        panel.SetActive(true);
                    }
                    else
                    {
                        panel.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError($"Object not found: {panel}");
                }
                break;
            }
        }
    }

    public void UpdateResultUI()
    {
        int score = gameHandler.playerScore;

        resultText.text = $"You have won {score}.";
    }    

    public void CloseDisplay (GameObject obj) //button function
    {
        StartCoroutine(CloseDisplayCo(obj));
    }

    public IEnumerator CloseDisplayCo(GameObject obj)
    {
        if (obj.GetComponentInChildren<Animator>() != null)
        {
            TriggerAnimation(obj, "Out");
        }
        yield return new WaitForSeconds(0.2f);
        obj.SetActive(false);
    }

    public void TriggerAnimation(GameObject obj, string triggerName)
    {
        Animator anim = obj.GetComponentInChildren<Animator>();

        anim.SetTrigger(triggerName);
    }
}
