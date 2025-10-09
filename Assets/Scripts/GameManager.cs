using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private LocalizeStringEvent localizeEvent;
    private LevelButton levelButton;
    public int SelectedLevel { get; set; } = 1;

    private Transform[] spawnPoints;
    private Transform carTransform;
    private Transform carRespawnTransform;

    private SpriteRenderer carSprRenderer;
    private SpriteRenderer trunkBoxSprRenderer;

    private AudioSource audioSource;

    public AudioClip gameOverSound;
    public AudioClip startSound;
    public AudioClip levelCompletedSound;
    public AudioClip crashSound;
    public AudioClip pickupSound;

    private GameObject spawnPointsContainer;
    public GameObject cargoPrefab;
    private GameObject currentCargo;

    public GameObject languagePanel;
    public GameObject levelsPanel;
    public GameObject startPanel;
    public GameObject startTextPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject levelCompletedPanel;
    public Button pauseButton;
    public Image lifesImg;
    public TextMeshProUGUI lifesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI stageText;

    private float distanceToCar = 2f;
    private float respawnBlinkTime = 1.5f;
    private float respawnBlinkInterval = 0.1f;
    public float gameTimer;
    public int stageNumber;
    public int score = 0;
    public int scoreToWin;
    public int lifes;
    public int startLifes;

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
    private void Start()
    {
#if UNITY_ANDROID
        if (IsFirstLaunch())
        {
            OpenLangPanel();
            MarkAsLaunched();
        }
#endif

        //SceneManager.LoadScene(PlayerPrefs.GetInt("last_selected_level"));
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource = GameObject.FindFirstObjectByType<AudioSource>();
        //playerController = FindFirstObjectByType<PlayerController>();
        carTransform = GameObject.FindGameObjectWithTag("Player").transform;
        carSprRenderer = carTransform.GetComponent<SpriteRenderer>();
        trunkBoxSprRenderer = GameObject.FindGameObjectWithTag("Wagon").GetComponent<SpriteRenderer>();
        trunkBoxSprRenderer.enabled = false;
        carRespawnTransform = GameObject.Find("CarRespawnPoint").transform;
        spawnPointsContainer = GameObject.Find("SpawnPoints");
        spawnPoints = spawnPointsContainer.GetComponentsInChildren<Transform>();
        stageNumber = SceneManager.GetActiveScene().buildIndex + 1;
        localizeEvent = scoreText.GetComponent<LocalizeStringEvent>();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        isPaused = false;

        lifes = startLifes;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        startPanel.gameObject.SetActive(false);

        ShowExcessUI();

        //stageText.text = $"{sceneIndex + 1}/30";
        lifesText.text = $"{lifes}";

        SpawnCargo();
        AddScore(-score);

        StartCoroutine(StartGameWithDelay());
    }
    IEnumerator StartGameWithDelay()
    {
        startTextPanel.gameObject.SetActive(true);
        audioSource.PlayOneShot(startSound);
        yield return new WaitForSeconds(1.25f);
        startTextPanel.gameObject.SetActive(false);
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
        localizeEvent.RefreshString();
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
        //box enabled
        trunkBoxSprRenderer.enabled = true;

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
            //box disabled
            trunkBoxSprRenderer.enabled = false;
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
            if (isPaused || !isStarted) yield break;
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
        isStarted = false;
        trunkBoxSprRenderer.enabled = false;
        lifesImg.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);

        levelButton.UpdateAllLevelButtons();
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void LevelCompleted()
    {
        isStarted = false;
        levelCompletedPanel.gameObject.SetActive(true);
        HideExcessUI();
        CompleteLevel(stageNumber);
        audioSource.Stop();
        audioSource.PlayOneShot(levelCompletedSound);
    }
    public void CompleteLevel(int level)
    {
        int unlocked = PlayerPrefs.GetInt("unlocked_level", 1);
        if (level >= unlocked)
        {
            PlayerPrefs.SetInt("unlocked_level", level + 1);
            PlayerPrefs.Save();
        }
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
    public void OpenLangPanel()
    {
        startPanel.gameObject.SetActive(false);
        languagePanel.gameObject.SetActive(true);
    }
    public void CloseLangPanel()
    {
        languagePanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }
    public void OpenLevelsPanel()
    {
        startPanel.gameObject.SetActive(false);
        levelsPanel.gameObject.SetActive(true);
    }
    public void CloseLevelsPanel()
    {
        levelsPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }
    void MarkAsLaunched()
    {
        PlayerPrefs.SetInt("game_launched_before", 1);
        PlayerPrefs.Save();
    }
    bool IsFirstLaunch()
    {
        // ¬озвращает true если игра запущена впервые
        return !PlayerPrefs.HasKey("game_launched_before");
    }
}
