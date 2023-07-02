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

    private readonly IList<AbstractEnemyController> _enemies = new List<AbstractEnemyController>();

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
        
        Debug.Log("all alive" + _enemies.Any(enemy => enemy.GetEnemy().IsAlive));
        if (_currentWave < waves || _enemies.Any(enemy => enemy.GetEnemy().IsAlive)) return;
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
        var spawnsPerWave = 1f;
        var spawnDelay = 4f;
        var pauseAfterWave = 10f;
        // SPAWNING
        // TDOD: if player is alive
        for (var i = 0; i < waves; i++)
        {
            for (var j = 0; j < spawnsPerWave; j++)
            {
                yield return new WaitForSeconds(spawnDelay);
                SpawnEnemies(_planetRadius, GenerateEnemies());
                Debug.Log("enemies spawned");
            }

            yield return new WaitForSeconds(pauseAfterWave);
            Debug.Log("wave end:" + _currentWave);
            _currentWave += 1;
        }

        // spawn boss
        SpawnEnemies(_planetRadius, new[] { bossEnemyPrefab });
        // add boss health bar
    }

    private IEnumerable<GameObject> GenerateEnemies() => Enumerable.Range(0, EnemiesInWave())
        .Select(x => _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);

    private int EnemiesInWave()
    {
        return 1; //TODO: dynamic
    }
}