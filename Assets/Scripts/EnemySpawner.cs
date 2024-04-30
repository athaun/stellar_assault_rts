using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {
    public Ship[] shipPrefabs;
    public Ship spaceStation;

    public int numberOfShips = 10;
    public float spawnRadius = 10f;


    void Start() {
        SpawnShips();
        StartCoroutine(SpawnShipsRoutine());
    }

    IEnumerator SpawnShipsRoutine() {
        while (true) {
            SpawnShips();
            yield return new WaitForSeconds(20);
        }
    }

    void SpawnShips() {
        Debug.Log("Spawning ships");
        for (int i = 0; i < numberOfShips; i++) {
            Vector3 spawnPosition = Random.onUnitSphere * spawnRadius;
            spawnPosition.y = 0;

            Ship ship = Instantiate(shipPrefabs[Random.Range(0, shipPrefabs.Length)], spawnPosition, Quaternion.identity);

            ship.IsEnemy = true;
            ship.SpaceStation = spaceStation;
            ship.faction = 1;
        }
    }

}
