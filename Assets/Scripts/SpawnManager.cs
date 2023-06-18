using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private const float Delay = 0.5f;
    private bool _alive = true;

    public void Start()
    {
        StartCoroutine(SpawnSystem());
    }

    public void OnPlayerDeath()
    {
        _alive = false;
    }

    private IEnumerator SpawnSystem()
    {
        // SPAWNING
        while (_alive)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(-20f, 8f), 20f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Delay);
        }

        yield return null;
    }
}