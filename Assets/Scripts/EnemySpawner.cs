using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {

    private static EnemySpawner instance;
    public static EnemySpawner Instance => instance;

    public Ship[] shipPrefabs;
    public Ship spaceStation;

    public int initialNumberOfShips = 2;
    public int increasePerRound = 1;
    public float spawnDelay = 2f;
    public float roundDelay = 5f;
    public Transform[] spawnPoints;

    private int currentRound = 0;
    private int remainingShips;
    private bool spawning;

    private int totalEnemies;

    public int TotalEnemies => totalEnemies;
    public int CurrentRound => currentRound;


    void Start() {
        instance = this;
        remainingShips = initialNumberOfShips;
        StartCoroutine(SpawnShips());
    }

    IEnumerator SpawnShips() {
        spawning = true;
        yield return new WaitForSeconds(roundDelay); // Wait before starting a new round
        while (remainingShips > 0) {
            for (int i = 0; i < remainingShips; i++) {
                if (remainingShips <= 0) break;
                SpawnShip();
                remainingShips--;
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(roundDelay); // Wait before starting a new round
            currentRound++;
        }
        spawning = false;
    }

    void SpawnShip() {
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        Ship ship = Instantiate(shipPrefabs[Random.Range(0, shipPrefabs.Length)], spawnPosition, Quaternion.identity);

        totalEnemies++;

        ship.IsEnemy = true;
        ship.SpaceStation = spaceStation;
        ship.faction = 1;

        // Debug.Log("Created enemy at " + spawnPosition + " of type " + ship);
    }

    public static void ShipDestroyed() {
        instance.remainingShips--;
        instance.totalEnemies--;
        if (instance.remainingShips <= 0 && !instance.spawning) {
            instance.remainingShips = instance.initialNumberOfShips + (instance.increasePerRound * instance.currentRound);
            instance.StartCoroutine(instance.SpawnShips());
        }
    }
}
