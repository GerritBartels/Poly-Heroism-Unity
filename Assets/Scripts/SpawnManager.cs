using System.Collections;
using System.Collections.Generic;
using Controllers;
using Model.Player;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject suicideEnemyPrefab;
    [SerializeField] private GameObject rangedEnemyPrefab;
    [SerializeField] private GameObject meleeEnemyPrefab;
    [SerializeField] private GameObject bossEnemyPrefab;

    [SerializeField] private GameObject player;
    private PlayerModel _playerModel;

    private const float Delay = 0.5f;
    private bool _alive = true;


    public void Start()
    {
        _playerModel = player.GetComponent<PlayerController>().PlayerModel;
        StartCoroutine(SpawnSystem());
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

    public void OnPlayerDeath()
    {
        _alive = false;
    }

    private IEnumerator SpawnSystem()
    {
        // SPAWNING
        while (_alive)
        {
            // TODO spawn waves
            yield return new WaitForSeconds(Delay);
        }

        yield return null;
    }
}