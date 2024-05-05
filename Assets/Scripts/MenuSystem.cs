using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour {
    private MenuSystem instance;
    public MenuSystem Instance => instance;

    public Ship spaceStation;

    private bool isPaused = false;
    private bool inGame = false;
    private bool justChangedState = false;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject hud;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    [Header("Transition Buttons")]
    public Button startGameButton;
    public Button returnToGameButton;
    public Button playAgainButton;

    public GameObject[] gameSystems;

    void Awake() {
        startGameButton.onClick.AddListener(startGame);
        returnToGameButton.onClick.AddListener(resumeGame);
        playAgainButton.onClick.AddListener(resetGame);
    }


    void Start() {
        mainMenu.SetActive(true);
        Time.timeScale = 0f;
        hud.SetActive(false);
    }

    void Update() {
        if (inGame && Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                resumeGame();
            } else {
                pauseGame();
            }
        }

        // Game over condition
        if (!spaceStation.gameObject.activeInHierarchy) {
            gameOver();
        }


        // Toggle game systems on and off if the game is paused/unpaused
        if (justChangedState != isPaused) {
            if (isPaused) {
                setSystems(false);
                Debug.Log("Systems off");
            } else {
                setSystems(true);
                Debug.Log("Systems on");
            }
        }

        justChangedState = isPaused;
    }

    void setSystems(bool state) {
        foreach (GameObject system in gameSystems) {
            system.SetActive(state);
        }
    }

    void pauseGame() {
        pauseMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void resumeGame() {
        pauseMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void gameOver() {
        inGame = false;
        StartCoroutine(gameOverCoroutine());
    }

    IEnumerator gameOverCoroutine() {
        yield return new WaitForSeconds(1f); // Delay of 2 seconds

        // Code to execute after the delay
        gameOverMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void startGame() {
        inGame = true;
        mainMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f;
    }

    public void resetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}