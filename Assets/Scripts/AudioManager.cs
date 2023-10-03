using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioData musicData;
    [Space]
    [SerializeField]
    private AudioSource effectSource;
    [SerializeField]
    private AudioData effectData;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundEffect(string clipName)
    {
        for (int i = 0; i < effectData.audioArray.Length; i++)
        {
            AudioClip[] clips = effectData.audioArray[i].clipArray;

            for (int j = 0; j < clips.Length; j++)
            {
                if (clips[j].name == clipName)
                {
                   //soundSource.clip = clips[j];
                   effectSource.PlayOneShot(clips[j]);
                   break;
                }
            }
        }
    }
}
