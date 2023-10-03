using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioData : MonoBehaviour
{
    public AudioArray[] audioArray;
}

[Serializable]
public struct AudioArray
{
    public string name;
    public AudioClip[] clipArray;
}