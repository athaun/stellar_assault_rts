using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private Economy economy;
    TextMeshProUGUI text;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        text.text = $@"Game Over

You survived for {EnemySpawner.Instance.CurrentRound} waves!
Destroyed {EnemySpawner.Instance.TotalEnemiesDestroyed} enemies
and lost {UnitController.Instance.TotalUnitsDestroyed} ships";

    }
}