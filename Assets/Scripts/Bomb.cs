using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionEffect;

    private void Start()
    {
        AudioManager.Instance.PlaySoundEffect("FruitSpawn");
        AudioManager.Instance.PlaySoundEffect("BombFuseBurning");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameHandler>().EndGame();
            FindObjectOfType<UIHandler>().Fade();
            Explosion();
        }
    }

    private void Explosion()
    {
        AudioManager.Instance.PlaySoundEffect("BombExplode");
        explosionEffect.SetActive(true);
    }
}
