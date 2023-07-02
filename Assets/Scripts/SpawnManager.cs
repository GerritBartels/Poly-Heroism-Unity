using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Controllers.Enemy;
using Model.Enemy;
using Model.Player;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject suicideEnemyPrefab;
    [SerializeField] private GameObject rangedEnemyPrefab;
    [SerializeField] private GameObject meleeEnemyPrefab;
    [SerializeField] private GameObject bossEnemyPrefab;
    private GameObject[] _enemyPrefabs;

    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private GameObject player;
    [SerializeField] private int waves = 4;
    private int _currentWave = 1;

    private PlayerModel _playerModel;

    private readonly IList<Enemy> _enemies = new List<Enemy>();

    private float _planetRadius;

    private int _lvl = 1; //TODO: load

    public void Start()
    {
        _playerModel = player.GetComponent<PlayerController>().PlayerModel;
        _enemyPrefabs = new[] { suicideEnemyPrefab, rangedEnemyPrefab, meleeEnemyPrefab };
        //StartCoroutine(SpawnSystem());
    }

    private bool _started = false;

    private void Update()
    {
        if (!_started)
        {
            _started = true;
            _planetRadius = 15; // TODO: GetComponent<GameObject>().transform.localScale.x;
            Debug.Log("radius" + _planetRadius);
            StartCoroutine(SpawnSystem());
        }
        Debug.Log("enemies" + _enemies.Count);
        if (_currentWave >= waves && _enemies.All(enemy => !enemy.IsAlive))
        {
            OnLvlCompleted();
        }
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
    public IList<GameObject> SpawnEnemies(float planetRadius, IEnumerable<GameObject> enemiesToSpawn)
    {
        var spawns = new List<GameObject>();
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
            var tries = 0;
            while (Physics.CheckSphere(spawnPosition, minDistance, layerMask) && !Physics.Raycast(spawnPosition,
                       -spawnPosition.normalized, out hit, planetRadius, layerMask2) && tries < 1000)
            {
                spawnPosition = GetRandomSpawnPosition(planetRadius);
                tries++;
            }

            if (tries > 1000)
            {
                continue;
            }

            spawnPosition = hit.point;

            // Calculate rotation so that the prefab is always facing outwards from the sphere
            var spawnRotation = Quaternion.FromToRotation(Vector3.up, spawnPosition.normalized);
            // Instantiate a random prefab at the sampled position and rotation
            var spawnedPrefab = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            spawnedPrefab.GetComponent<AbstractEnemyController>().player = player;
            spawns.Add(spawnedPrefab);
            // Randomize the rotation of the spawned prefab and parent it to the planet
            RandomizePrefabRotation(spawnedPrefab.transform);
        }

        return spawns;
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
        while (_currentWave <= waves && _playerModel.IsAlive)
        {
            var duration = 4f; //TODO: dynamic
            var spawnDelay = 4f;
            StartCoroutine(Wave(duration, spawnDelay));
            yield return new WaitForSeconds(duration);
            _currentWave += 1;
        }

        SpawnBoss();
        yield return null;
    }

    private IEnumerator Wave(float duration, float spawnDelay)
    {
        var endWave = Time.time + duration;
        while (endWave > Time.time && _playerModel.IsAlive)
        {
            var enemies = SpawnEnemies(_planetRadius, GenerateEnemies());
            _enemies.AddRange(enemies.Select(e => e.GetComponent<AbstractEnemyController>().GetEnemy()));
            yield return new WaitForSeconds(spawnDelay);
        }

        yield return null;
    }

    private IEnumerable<GameObject> GenerateEnemies() => Enumerable.Range(0, EnemiesInWave())
        .Select(x => _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);

    private int EnemiesInWave()
    {
        return 1; //TODO: dynamic
    }

    private void SpawnBoss()
    {
        var boss = SpawnEnemies(_planetRadius, new[] { bossEnemyPrefab }).First();
        Debug.Log(boss.GetComponent<AbstractEnemyController>().GetEnemy());
        _enemies.Add(boss.GetComponent<AbstractEnemyController>().GetEnemy());
    }
}