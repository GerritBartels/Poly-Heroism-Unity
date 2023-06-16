using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private GameObject enemyPrefab;

    private const float _delay = 0.5f;
    private bool _alive = true;

    void Start()
    {
        StartCoroutine(SpawnSystem());
    }


    public void onPlayerDeath()
    {
        _alive = false;
    }

    IEnumerator SpawnSystem()
    {
        // SPAWNING
        while (_alive)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(-20f, 8f), 20f, 0), Quaternion.identity);
            yield return new WaitForSeconds(_delay);
        }

        yield return null;
    }
}