using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    SpawnElement spawnElement;
    DataHandler dataHandler;
    GameHandler gameHandler;

    public GameObject whole;
    public GameObject sliced;

    [SerializeField]
    private int fruitIndex;
    [SerializeField]
    private FruitsData fruitsData;
    private Rigidbody fruitRB;
    private Collider fruitCollider;
    private ParticleSystem juiceEffect;

    private void Awake()
    {
        fruitRB = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();
        spawnElement = FindObjectOfType<SpawnElement>();
        dataHandler = FindObjectOfType<DataHandler>();
        gameHandler = FindObjectOfType<GameHandler>();
    }

    private void Start()
    {
        //set fruit value
        fruitsData = dataHandler.gameData.fruits[fruitIndex];
        AudioManager.Instance.PlaySoundEffect("FruitSpawn");
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        AddScore();
        whole.SetActive(false);
        sliced.SetActive(true);

        fruitCollider.enabled = false;
        juiceEffect.Play();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();
        
        foreach(Rigidbody slice in slices)
        {
            slice.velocity = fruitRB.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.Direction, blade.transform.position, blade.sliceForce);
            AudioManager.Instance.PlaySoundEffect("FruitCut");
        }
    }

    public void AddScore()
    {
        gameHandler.playerScore += fruitsData.Score;
    }
}
