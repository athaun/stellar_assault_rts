using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {
    public Ship[] shipPrefabs;
    public Ship spaceStation;

    public int initialNumberOfShips = 2;
    public int increasePerRound = 1;
    public float spawnDelay = 2f;
    public float roundDelay = 5f;
    public Transform[] spawnPoints;

    private int currentRound = 1;
    private int remainingShips;
    private bool spawning;

    void Start() {
        remainingShips = initialNumberOfShips;
        StartCoroutine(SpawnShips());
    }

    IEnumerator SpawnShips() {
        spawning = true;
        yield return new WaitForSeconds(roundDelay); // Wait before starting a new round
        while (remainingShips > 0) {
            for (int i = 0; i < initialNumberOfShips + (increasePerRound * (currentRound - 1)); i++) {
                if (remainingShips <= 0)
                    break;
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

        ship.gameObject.GetComponent<Ship>().enabled = false;
        ship.gameObject.GetComponent<EnemyShip>().enabled = true;
        ship.gameObject.GetComponent<EnemyShip>().SpaceStation = spaceStation;

        Debug.Log("Created enemy at " + spawnPosition + " of type " + ship);
    }

    public void ShipDestroyed() {
        remainingShips--;
        if (remainingShips <= 0 && !spawning) {
            StartCoroutine(SpawnShips());
        }
    }
}
