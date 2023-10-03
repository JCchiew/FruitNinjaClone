using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
    [Header("Fruit Data")]
    public FruitsData[] fruits;
}
