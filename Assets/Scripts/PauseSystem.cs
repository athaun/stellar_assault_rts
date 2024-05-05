using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour {
    public PauseSystem instance;
    public PauseSystem Instance => instance;

    private bool isPaused = false;
    private bool isGameOver = false;

    public GameObject mainMenu;
    public GameObject hud;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public Ship spaceStation;

    void Start() {
        // Show main menu and hide HUD at the start
        mainMenu.SetActive(true);
        hud.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver) {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }

        // Game over condition
        if (!spaceStation.gameObject.activeInHierarchy) {
            GameOver();
        }
    }

    void PauseGame() {
        pauseMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void ResumeGame() {
        pauseMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void GameOver() {
        gameOverMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f;
        isGameOver = true;
    }

    public void StartGame() {
        mainMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f;
    }
}