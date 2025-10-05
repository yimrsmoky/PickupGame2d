using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerController playerController;
    private Transform[] spawnPoints;
    private Transform carTransform;
    private Transform carRespawnTransform;

    private SpriteRenderer carSprRenderer;

    private AudioSource audioSource;

    public AudioClip gameOverSound;
    public AudioClip startSound;
    public AudioClip levelCompletedSound;
    public AudioClip crashSound;
    public AudioClip pickupSound;

    private GameObject spawnPointsContainer;
    public GameObject cargoPrefab;
    private GameObject currentCargo;

    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject levelCompletedPanel;
    public Button pauseButton;
    public Image lifesImg;
    public TextMeshProUGUI lifesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI stageText;

    private float distanceToCar = 2f;
    private float respawnBlinkTime = 1.5f;
    private float respawnBlinkInterval = 0.1f;
    public int score = 0;
    public int scoreToWin;
    public int lifes;

    public bool isPaused = true;
    public bool isStarted;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource = GameObject.FindFirstObjectByType<AudioSource>();
        playerController = FindFirstObjectByType<PlayerController>();
        carTransform = GameObject.FindGameObjectWithTag("Player").transform;
        carSprRenderer = carTransform.GetComponent<SpriteRenderer>();
        carRespawnTransform = GameObject.Find("CarRespawnPoint").transform;
        spawnPointsContainer = GameObject.Find("SpawnPoints");
        spawnPoints = spawnPointsContainer.GetComponentsInChildren<Transform>();

        if (SceneManager.GetActiveScene().buildIndex !=0)
        {
            StartGame(lifes);
        }
    }
    public void StartGame(int startLifes)
    {
        isPaused = false;

        lifes = startLifes;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        startPanel.gameObject.SetActive(false);

        ShowExcessUI();

        stageText.text = $"LEVEL {sceneIndex+1}/30";
        lifesText.text = $"{lifes}";

        SpawnCargo();
        AddScore(-score);

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
    public void SpawnCargo()
    {
        if (currentCargo != null)
        {
            Destroy(currentCargo);
        }
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (var point in spawnPoints)
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
    public void AddScore(int scoreToAdd)
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
    public void CollectCargo()
    {
        audioSource.PlayOneShot(pickupSound);
        AddScore(1);
        if (score == scoreToWin)
        {
            LevelCompleted();
        }
        else
        {
            SpawnCargo();
        }
    }
    public void TouchingAnObstacle()
    {
        audioSource.PlayOneShot(crashSound);
        if (lifes == 1)
        {
            GameOver();
        }
        else
        {
            UpdateLifes(1);
        }
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
        carTransform.position = carRespawnTransform.position;
        carTransform.rotation = carRespawnTransform.rotation;
        while (timer < respawnBlinkTime)
        {
            carSprRenderer.enabled = !carSprRenderer.enabled;
            yield return new WaitForSeconds(respawnBlinkInterval);

            timer += respawnBlinkInterval;
        }
        carSprRenderer.enabled = true;
    }
    public void GameOver()
    {
        isPaused = true;
        gameOverPanel.gameObject.SetActive(true);
        HideExcessUI();
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void LevelCompleted()
    {
        isStarted = false;
        levelCompletedPanel.gameObject.SetActive(true);
        HideExcessUI();
        audioSource.Stop();
        audioSource.PlayOneShot(levelCompletedSound);
    }
    public void ToNextLevel()
    {
        levelCompletedPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void HideExcessUI()
    {
        pauseButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        lifesImg.gameObject.SetActive(false);
    }
    void ShowExcessUI()
    {
        pauseButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        lifesImg.gameObject.SetActive(true);
    }
}
