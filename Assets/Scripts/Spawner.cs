using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    SpawnElement spawnElement;
    GameHandler gameHandler;

    [Header("Spawner parameters")]
    [Space]
    [Header("Frenzy Mode")]
    [Tooltip("Toggle Frenzy mode")]
    public bool frenzyMode;
    [Space]
    [Header("Spawn Delay")]
    [Tooltip("Delay between each fruit spawn")]
    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;
    private float defaultMinDelay;
    private float defaultMaxDelay;
    [Space]
    [Header("Spawn Angle")]
    [Tooltip("Angle of the fruit will spawned out")]
    public float minAngle = -15f;
    public float maxAngle = 15f;
    [Space]
    [Header("Spawn Force")]
    [Tooltip("Velocity of the spawned fruit")]
    public float minForce = 18f;
    public float maxForce = 22f;
    [Space]
    [Header("Fruit Lifetime")]
    [Tooltip("Spawned fruit lifetime")]
    public float maxLifeTime = 5f;

    [SerializeField]
    private Collider spawnArea;
    private bool spawning;

    [Space]
    [Header("Bomb")]
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private int defaultBombDelay = 2;

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
        spawnElement = FindObjectOfType<SpawnElement>();
        gameHandler = FindObjectOfType<GameHandler>();
    }

    private void Start()
    {
        frenzyMode = false;
        SetDefaultValue();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        spawning = false;
    }

    public void StartSpawn()
    {
        spawning = true;
        StartCoroutine(Spawn());
        StartCoroutine(SpawnBombCo()); //Check for spawn bomb
    }

    private void SetDefaultValue()
    {
        defaultMaxDelay = maxSpawnDelay;
        defaultMinDelay = minSpawnDelay;
    }

    public IEnumerator Spawn()
    {
        spawnElement.CalculateOccurance();
        while (spawning)
        {
            StartCoroutine(FrenzyTime()); //Check for frenzy mode bool

            yield return new WaitUntil(() => !spawnElement.isRunning);

            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            float roll = UnityEngine.Random.Range(0, spawnElement.accumulatedWeight);

            //spawn fruit
            for (int i = 0; i < gameHandler.dataHandler.gameData.fruits.Length; i++)
            {
                roll -= gameHandler.dataHandler.gameData.fruits[i].Occurences;
                if (roll < 0)
                {
                    //GameObject prefab = spawnElement.fruitPrefabs[Random.Range(0, spawnElement.fruitPrefabs.Length)].FruitPrefab;
                    GameObject fruit = Instantiate(gameHandler.dataHandler.gameData.fruits[i].FruitPrefab, position, rotation);
                    //GameObject fruit = Instantiate(prefab, position, rotation);

                    Destroy(fruit, maxLifeTime);

                    float force = Random.Range(minForce, maxForce);
                    fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);
                    break;
                }
            }
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

    public IEnumerator SpawnBombCo()
    {
        yield return new WaitForSeconds(defaultBombDelay);
        //spawn bomb
        while (spawning)
        {
            int rollBomb = UnityEngine.Random.Range(2, 4);
            if (rollBomb <= gameHandler.time)
            {
                SpawnBomb();
                yield return new WaitForSeconds(rollBomb);
            }
            else
            {
                break;
            }
        }
    }

    public void SpawnBomb()
    {
        Vector3 position = new Vector3();
        position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

        GameObject spawnedBomb = Instantiate(bombPrefab, position, rotation);

        Destroy(spawnedBomb, maxLifeTime);

        float force = Random.Range(minForce, maxForce);
        spawnedBomb.GetComponent<Rigidbody>().AddForce(spawnedBomb.transform.up * force, ForceMode.Impulse);
    }

    public IEnumerator FrenzyTime()
    {
        if (frenzyMode)
        {
            minSpawnDelay = 0.1f;
            maxSpawnDelay = 0.5f;
        }
        else
        {
            minSpawnDelay = defaultMinDelay;
            maxSpawnDelay = defaultMaxDelay;
        }
        yield return null;
    }
}
