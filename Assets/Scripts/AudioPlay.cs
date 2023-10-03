using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;

    public void PlaySoundEffect(string audioName)
    {
        AudioManager.Instance.PlaySoundEffect(audioName);
    } 
}
