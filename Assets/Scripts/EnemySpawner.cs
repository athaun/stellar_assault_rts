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
        for (int i = 0; i < numberOfShips; i++) {
            Vector3 spawnPosition = Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = 0;

            Ship ship = Instantiate(shipPrefabs[Random.Range(0, shipPrefabs.Length)], spawnPosition, Quaternion.identity);

            ship.gameObject.GetComponent<Ship>().enabled = false;
            ship.gameObject.GetComponent<EnemyShip>().enabled = true;
            ship.gameObject.GetComponent<EnemyShip>().SpaceStation = spaceStation;            

            Debug.Log("Created enemy at " + spawnPosition + " of type " + ship);
        }
    }
}
