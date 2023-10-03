using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnElement : MonoBehaviour
{
    DataHandler dataHandler;

    public float accumulatedWeight;
    public bool isRunning;

    private List<int> nonZeroChancesIndices = new List<int>();


    private void Awake()
    {
        dataHandler = FindObjectOfType<DataHandler>();
    }

    private void Start()
    {
        StartCoroutine(UpdateFruitListCo());
    }

    private IEnumerator UpdateFruitListCo()
    {
        yield return APIManager.instance.PostToGetGameDataCo();
        yield return new WaitUntil(() => !APIManager.instance.isRunning);
    }

    public void CalculateOccurance()
    {
        isRunning = true;
        if (accumulatedWeight == 0)
        {
            for (int i = 0; i < dataHandler.gameData.fruits.Length; i++)
            {


                accumulatedWeight += dataHandler.gameData.fruits[i].Occurences;

                if (dataHandler.gameData.fruits[i].Occurences > 0)
                    nonZeroChancesIndices.Add(i);
                if (nonZeroChancesIndices.Count == 0)
                    Debug.LogError("You can't set all pieces chance to zero");
            }
        }
        isRunning = false;
    }
}
