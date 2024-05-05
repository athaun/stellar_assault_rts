using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EcoStatUpdater : MonoBehaviour {
    [SerializeField] private Economy economy;
    TextMeshProUGUI text;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        text.text = $"Scrap: ${economy.Scrap}\nWave: {EnemySpawner.Instance.CurrentRound}\nEnemies: {EnemySpawner.Instance.TotalEnemies}";
    }
}