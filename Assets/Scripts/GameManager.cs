using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Transform[] spawnPoints;
    public Transform carTransform;

    public AudioSource audioSource;

    public GameObject spawnPointsContainer;
    public GameObject cargoPrefab;
    private GameObject currentCargo;

    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public Button pauseButton;
    public Image lifesImg;
    public TextMeshProUGUI lifesText;
    public TextMeshProUGUI scoreText;

    private float distanceToCar = 2f;
    private int score = 0;
    public int lifes;

    public bool isStarted;
    public bool isPaused;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }
    void Start()
    {
        spawnPoints = spawnPointsContainer.GetComponentsInChildren<Transform>();

    }
    void Update()
    {

    }
    public void StartGame(int startLifes)
    {
        lifes = startLifes;
        startPanel.gameObject.SetActive(false);

        pauseButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        lifesImg.gameObject.SetActive(true);

        lifesText.text = $"{lifes}";

        SpawnCargo();
        AddScore(0);

        StartCoroutine(StartGameWithDelay());
    }
    IEnumerator StartGameWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        isStarted = true;
    }
    public void SpawnCargo ()
    {
        if (currentCargo != null)
        {
            Destroy(currentCargo);
        }
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach ( var point in spawnPoints)
        {
            bool isTooCloseToPlayer = Vector2.Distance(point.transform.position, carTransform.position) < distanceToCar;

            if (!isTooCloseToPlayer)
            {
                validSpawnPoints.Add(point.transform);
            }
        }
        if (validSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPoints.Count);

            currentCargo = Instantiate(cargoPrefab, validSpawnPoints[randomIndex].position, Quaternion.identity);
        }
    }
    public void AddScore (int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = $"Score: {score}";
    }
    public void UpdateLifes(int lifesToRemove)
    {
        lifes -= lifesToRemove;
        lifesText.text = $"{lifes}";
    }
    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            audioSource.Pause();
            pauseButton.gameObject.SetActive(false);
            pausePanel.gameObject.SetActive(true);
        }
        else
        { 
            isPaused = false;
            Time.timeScale = 1f;
            audioSource.UnPause();
            pauseButton.gameObject.SetActive(true);
            pausePanel.gameObject.SetActive(false);
        }
    }
    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
