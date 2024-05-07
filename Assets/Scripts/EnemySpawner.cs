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
    public int increasePerRound = 2;
    public float spawnDelay = 2f;
    public float roundDelay = 5f;
    public Transform[] spawnPoints;

    private int currentRound = 0;
    private int remainingShips;
    private bool spawning;

    private int totalEnemies;
    private int totalEnemiesDestroyed;
    public int TotalEnemiesDestroyed => totalEnemiesDestroyed;

    public int TotalEnemies => totalEnemies;
    public int CurrentRound => currentRound;


    void Start() {
        instance = this;
        currentRound = 0; // Ensure currentRound is initialized to 0
        if (currentRound == 0) {
            remainingShips = initialNumberOfShips * 2; // Double initialNumberOfShips for round 0
        } else {
            remainingShips = initialNumberOfShips + (increasePerRound * currentRound); // Initialize remainingShips according to currentRound for other rounds
        }
        StartCoroutine(SpawnShips());
    }

    IEnumerator SpawnShips() {
        spawning = true;
        yield return new WaitForSeconds(roundDelay); // Wait before starting a new round
        while (remainingShips > 0) {
            for (int i = 0; i < remainingShips; i++) {
                if (remainingShips <= 0) break;
                SpawnShip();
                yield return new WaitForSeconds(spawnDelay);
            }

            // Wait until all ships are destroyed
            while (totalEnemies > 0) {
                yield return null;
            }

            yield return new WaitForSeconds(roundDelay); // Wait before starting a new round

            currentRound++;
            if (currentRound == 0) {
                remainingShips = initialNumberOfShips * 2; // Double initialNumberOfShips for round 0
            } else {
                remainingShips = initialNumberOfShips + (increasePerRound * currentRound); // Reset remaining ships for the new round
            }

            increasePerRound += 2; // Increase the number of ships spawned per round
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
        if (instance.remainingShips > 0 && instance.totalEnemies > 0) {
            instance.remainingShips--;
            instance.totalEnemies--;
        }
        instance.totalEnemiesDestroyed++;
        if (instance.remainingShips <= 0 && !instance.spawning) {
            instance.remainingShips = instance.initialNumberOfShips + (instance.increasePerRound * instance.currentRound);
            instance.StartCoroutine(instance.SpawnShips());
        }
    }
}
