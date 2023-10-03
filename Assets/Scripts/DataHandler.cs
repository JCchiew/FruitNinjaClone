using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public UserData userData;
    public GameData gameData;
}

[System.Serializable]
public class UserModel
{
    public float Chances;
    public FruitsData[] Items;
    public float Frenzy;
}

public class ScoreModel
{
    public string userName;
    public int score;
}

[System.Serializable]
public class FruitList
{
    public FruitsData[] fruit;
}

[System.Serializable]
public struct FruitsData
{
    public GameObject FruitPrefab;
    public string Name;
    public int Score;

    [Tooltip("Probability in %, preferably all adds up to 1")]
    [Range(0f, 1f)]
    public float Occurences;

}

[System.Serializable]
public class GameResult
{
    public string userName;
    public float score;
}
