using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Controllers.Enemy;
using Model.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject suicideEnemyPrefab;
    [SerializeField] private GameObject rangedEnemyPrefab;
    [SerializeField] private GameObject meleeEnemyPrefab;
    [SerializeField] private GameObject bossEnemyPrefab;
    private GameObject[] _enemyPrefabs;

    [SerializeField] private float minDistance = 2f;
    [SerializeField] private GameObject player;

    // TODO: balancing!!!
    [SerializeField] private int waves = 2;
    [SerializeField] private int baseEnemiesPerSpawn = 1;
    [SerializeField] private int spawnsPerWave = 1;
    [SerializeField] private float spawnDelay = 15f;
    [SerializeField] private float pauseAfterWave = 30f;

    private PlayerModel _playerModel;

    private readonly IList<AbstractEnemyController> _enemies = new List<AbstractEnemyController>();

    private bool _bossSpawned = false;
    private float _planetRadius;
    private int _lvl = 0; //TODO: load

    private int _currentWave = 0;

    public void Start()
    {
        _playerModel = player.GetComponent<PlayerController>().PlayerModel;
        _enemyPrefabs = new[] { suicideEnemyPrefab, rangedEnemyPrefab, meleeEnemyPrefab };
    }

    private bool _started = false;

    private void Update()
    {
        if (!_started)
        {
            _started = true;
            _planetRadius = 15; // TODO: dynamic: GetComponent<GameObject>().transform.localScale.x;
            StartCoroutine(SpawnSystem());
        }

        if (!_bossSpawned || _enemies.Any(enemy => enemy.GetEnemy().IsAlive)) return;
        Debug.Log("Finished");
        OnLvlCompleted();
    }

    public void OnLvlCompleted()
    {
        _playerModel.OnLvlUp();
        ShowLvlFinishedScreen();
    }

    private void ShowLvlFinishedScreen()
    {
        //TODO
    }

    /// <summary>
    /// <c>SpawnEnemies</c> randomly spawns given enemy prefabs on the planet surface.
    /// </summary>
    /// <param name="planetRadius">the radius of the planet</param>
    /// <param name="enemiesToSpawn">enemy prefabs to be spawned</param>
    public void SpawnEnemies(float planetRadius, IEnumerable<GameObject> enemiesToSpawn)
    {
        // Exclude the "Planet" layer for the first layer mask and include only it for the second layer mask
        var planetLayer = LayerMask.NameToLayer("Planet");
        var layerMask = ~(1 << planetLayer);
        var layerMask2 = 1 << planetLayer;

        var spawnPosition = GetRandomSpawnPosition(planetRadius);
        foreach (var enemyPrefab in enemiesToSpawn)
        {
            // Sample a random point on the sphere
            // Check for collisions, excluding the "Planet" layer
            RaycastHit hit = default;
            while (Physics.CheckSphere(spawnPosition, minDistance, layerMask) && !Physics.Raycast(spawnPosition,
                       -spawnPosition.normalized, out hit, planetRadius, layerMask2))
            {
                spawnPosition = GetRandomSpawnPosition(planetRadius);
            }

            spawnPosition = hit.point;

            // Calculate rotation so that the prefab is always facing outwards from the sphere
            var spawnRotation = Quaternion.FromToRotation(Vector3.up, spawnPosition.normalized);
            // Instantiate a random prefab at the sampled position and rotation
            var spawnedPrefab = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            var controller = spawnedPrefab.GetComponent<AbstractEnemyController>();
            controller.player = player;
            _enemies.Add(controller);
            // Randomize the rotation of the spawned prefab and parent it to the planet
            RandomizePrefabRotation(spawnedPrefab.transform);
        }
    }

    /// <summary>
    /// <c>GetRandomSpawnPosition</c> generates a random spawn position on a sphere's surface.
    /// </summary>
    /// <param name="radius">radius of the sphere</param>
    /// <returns>random spawn position</returns>
    private Vector3 GetRandomSpawnPosition(float radius)
    {
        // An offset is added to the radius to avoid spawning objects inside the planet
        return Random.onUnitSphere * (radius + 0.5f);
    }

    /// <summary>
    /// <c>RandomizePrefabRotation</c> randomizes the rotation of the specified prefab transform.
    /// </summary>
    /// <param name="prefabTransform">transform of the prefab to rotate</param>
    private void RandomizePrefabRotation(Transform prefabTransform)
    {
        prefabTransform.RotateAround(prefabTransform.position, prefabTransform.up, Random.Range(0f, 360f));
    }

    private IEnumerator SpawnSystem()
    {
        // SPAWNING
        // TODO: if player is alive
        for (_currentWave = 0; _currentWave < waves; _currentWave++)
        {
            Debug.Log("wave: " + _currentWave);
            for (var j = 0; j < spawnsPerWave; j++)
            {
                yield return new WaitForSeconds(spawnDelay);
                Debug.Log("sub wave: " + j);
                SpawnEnemies(_planetRadius, GenerateEnemies());
            }

            yield return new WaitForSeconds(pauseAfterWave);
        }

        // spawn boss
        SpawnEnemies(_planetRadius, new[] { bossEnemyPrefab });
        _bossSpawned = true;
        // add boss health bar
    }

    private IEnumerable<GameObject> GenerateEnemies() => Enumerable.Range(0, EnemiesInWave())
        .Select(x => _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);

    private int EnemiesInWave()
    {
        return baseEnemiesPerSpawn + _lvl + _currentWave;
    }
}