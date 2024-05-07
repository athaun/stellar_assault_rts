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
        text.text = $"Scrap\t\t${economy.Scrap}\nWave\t\t{EnemySpawner.Instance.CurrentRound + 1}\nEnemies\t{EnemySpawner.Instance.TotalEnemies}";
    }
}