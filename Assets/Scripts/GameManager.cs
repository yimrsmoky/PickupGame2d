using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    private Transform[] spawnPoints;
    public Transform carTransform;
    public Transform carRespawnTransform;

    public AudioSource audioSource;

    public AudioClip gameOverSound;
    public AudioClip startSound;

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
    public TextMeshProUGUI startText;

    private float distanceToCar = 2f;
    private float respawnBlinkTime = 1.5f;
    private float respawnBlinkInterval = 0.1f;
    private int score = 0;
    public int lifes;

    public bool isPaused = true;
    public bool isStarted;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }
    void Start()
    {
        spawnPoints = spawnPointsContainer.GetComponentsInChildren<Transform>();
        playerController = FindFirstObjectByType<PlayerController>();
    }
    void Update()
    {

    }
    public void StartGame(int startLifes)
    {
        isPaused = false;

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
        startText.gameObject.SetActive(true);
        audioSource.PlayOneShot(startSound);
        yield return new WaitForSeconds(1f);
        startText.gameObject.SetActive(false);
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

        StartCoroutine(RespawnCarWithBlink());
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
    public IEnumerator RespawnCarWithBlink()
    {
        float timer = 0f;
        SpriteRenderer carSprR = playerController.carSprRenderer;
        carTransform.position = carRespawnTransform.position;
        carTransform.rotation = carRespawnTransform.rotation;
        while (timer < respawnBlinkTime)
        {
            carSprR.enabled = !carSprR.enabled;
            yield return new WaitForSeconds(respawnBlinkInterval);

            timer += respawnBlinkInterval;
        }
        carSprR.enabled = true;
    }
    public void GameOver()
    {
        isPaused = true;
        gameOverPanel.gameObject.SetActive(true);
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
