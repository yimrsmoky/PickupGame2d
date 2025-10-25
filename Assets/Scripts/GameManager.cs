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

    private LocalizeStringEvent timerLocalizeEvent;
    private LocalizeStringEvent scoreLocalizeEvent;

    private PlayerController playerController;

    private LevelButton[] allLevelButtonComponents;
    public int SelectedLevel { get; set; } = 1;

    private Transform[] spawnPoints;
    private Transform carTransform;
    private Transform carRespawnTransform;

    private SpriteRenderer carSprRenderer;

    private AudioSource audioSource;
    private AudioSource UIAudioSource;

    public AudioClip gameOverSound;
    public AudioClip startSound;
    public AudioClip levelCompletedSound;
    public AudioClip gameCompletedSound;
    public AudioClip crashSound;
    public AudioClip pickupSound;
    public AudioClip clickSound;

    private GameObject spawnPointsContainer;
    public GameObject cargoPrefab;
    private GameObject currentCargo;

    public GameObject languagePanel;
    public GameObject infoPanel;
    public GameObject levelsPanel;
    public GameObject levelGrid;
    public GameObject startPanel;
    public GameObject startTextPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject levelCompletedPanel;
    public GameObject gameCompletedPanel;
    public Button pauseButton;
    public Image lifesImg;
    public TextMeshProUGUI lifesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI carDestroyedText;
    public TextMeshProUGUI timeOutText;

    private float distanceToCar = 2f;
    private float respawnBlinkTime = 0.5f;
    private float respawnBlinkInterval = 0.05f;
    private float gameTimer;
    [SerializeField] private float levelTime;
    public int stageNumber;
    public int stageIndex;
    public int score = 0;
    public int scoreToWin;
    private int lifes;
    public int startLifes;

    public string gameTimerString;

    bool timerStarted;
    public bool isPaused = true;
    public bool isStarted;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadCurrentLevelOnRun();
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
            //MarkAsLaunched();
        }
#endif

    }
    private void Update()
    {
        if (timerStarted)
        {
            gameTimer -= Time.deltaTime;

            if (gameTimer <= 0f)
            {
                gameTimer = 0f;
                GameOver("time_out");
            }

            UpdateTimerText();
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        UIAudioSource = GameObject.Find("Canvas").GetComponent<AudioSource>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        carTransform = GameObject.FindGameObjectWithTag("Player").transform;
        carSprRenderer = carTransform.GetComponent<SpriteRenderer>();
        carRespawnTransform = GameObject.Find("CarRespawnPoint").transform;
        spawnPointsContainer = GameObject.Find("SpawnPoints");
        spawnPoints = spawnPointsContainer.GetComponentsInChildren<Transform>();
        stageNumber = SceneManager.GetActiveScene().buildIndex + 1;
        stageIndex = SceneManager.GetActiveScene().buildIndex;
        timerLocalizeEvent = gameTimerText.GetComponent<LocalizeStringEvent>();
        scoreLocalizeEvent = scoreText.GetComponent<LocalizeStringEvent>();

        SelectedLevel = stageNumber;
        PrepareLevelSettings(stageNumber);
    }
    void LoadCurrentLevelOnRun()
    {
        if (PlayerPrefs.HasKey("last_selected_level"))
        {
            int savedLevel = PlayerPrefs.GetInt("last_selected_level");

            SelectedLevel = savedLevel;
            SceneManager.LoadScene(savedLevel - 1);
        }
    }
    void PrepareLevelSettings(int level)
    {
        switch (level)
        {
            case 1:
                levelTime = 30f;
                startLifes = 5;
                scoreToWin = 5;
                break;
            case 2:
                levelTime = 35f;
                startLifes = 5;
                scoreToWin = 6;
                break;
            case 3:
                levelTime = 40f;
                startLifes = 5;
                scoreToWin = 7;
                break;
            case 4:
                levelTime = 45f;
                startLifes = 5;
                scoreToWin = 8;
                break;
            case 5:
                levelTime = 50f;
                startLifes = 5;
                scoreToWin = 9;
                break;
            case 6:
                levelTime = 55f;
                startLifes = 5;
                scoreToWin = 10;
                break;
            case 7:
                levelTime = 60f;
                startLifes = 5;
                scoreToWin = 11;
                break;
            case 8:
                levelTime = 65f;
                startLifes = 5;
                scoreToWin = 12;
                break;
            case 9:
                levelTime = 70f;
                startLifes = 5;
                scoreToWin = 13;
                break;
            case 10:
                levelTime = 75f;
                startLifes = 5;
                scoreToWin = 14;
                break;
            case 11:
                levelTime = 80f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 12:
                levelTime = 85f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 13:
                levelTime = 90f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 14:
                levelTime = 95f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 15:
                levelTime = 100f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 16:
                levelTime = 105f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 17:
                levelTime = 110f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 18:
                levelTime = 115f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 19:
                levelTime = 120f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 20:
                levelTime = 120f;
                startLifes = 7;
                scoreToWin = 15;
                break;
            case 21:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 22:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 23:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 24:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 25:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 26:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 27:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 28:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 29:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
            case 30:
                levelTime = 120f;
                startLifes = 10;
                scoreToWin = 15;
                break;
        }
    }
    public void StartGame()
    {
        //isPaused = false;

        gameTimer = levelTime;
        lifes = startLifes;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        startPanel.gameObject.SetActive(false);
        infoPanel.gameObject.SetActive(false);

        //ShowExcessUI();

        lifesText.text = $"{lifes}";

        SpawnCargo();
        AddScore(-score);

        StartCoroutine(StartGameWithDelay());
    }
    IEnumerator StartGameWithDelay()
    {
        yield return new WaitForEndOfFrame();
        startTextPanel.gameObject.SetActive(true);
        audioSource.PlayOneShot(startSound);
        yield return new WaitForSeconds(1.25f);
        startTextPanel.gameObject.SetActive(false);
        isPaused = false;
        isStarted = true;
        ShowExcessUI();
        timerStarted = true;
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
            if (point == spawnPointsContainer.transform) continue;
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
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        gameTimerString = $"{minutes}:{seconds:00}";
        timerLocalizeEvent.RefreshString();
    }
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreLocalizeEvent.RefreshString();
    }
    public void UpdateLifes(int lifesToRemove)
    {
        lifes -= lifesToRemove;
        lifesText.text = $"{lifes}";
    }
    public void CollectCargo()
    {
        audioSource.PlayOneShot(pickupSound);
        AddScore(1);

        if (score == scoreToWin)
        {
            if (stageIndex == 29) GameCompleted();
            else LevelCompleted();
        }
        else
        {
            SpawnCargo();
        }
    }
    public void TouchingAnObstacle()
    {
        audioSource.PlayOneShot(crashSound);

        if (playerController.isBoosted) playerController.Tap();

        if (lifes == 1)
        {
            GameOver("car_destroyed");
        }
        else
        {
            UpdateLifes(1);
            StartCoroutine(RespawnCarWithBlink());
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
        playerController.targetRotation = carRespawnTransform.rotation;
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
    public void GameOver(string reason)
    {
        timerStarted = false;
        isPaused = true;
        gameOverPanel.gameObject.SetActive(true);
        HideExcessUI();
        gameTimerText.gameObject.SetActive(true);
        if (reason == "time_out") timeOutText.gameObject.SetActive(true);
        if (reason == "car_destroyed") carDestroyedText.gameObject.SetActive(true);

        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        isStarted = false;
        timerStarted = false;
        lifesImg.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        gameTimerText.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        timeOutText.gameObject.SetActive(false);
        carDestroyedText.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        gameCompletedPanel.gameObject.SetActive(false);

        SceneManager.LoadScene(stageNumber - 1);
        Time.timeScale = 1f;
    }
    public void LevelCompleted()
    {
        isStarted = false;
        timerStarted = false;

        levelCompletedPanel.gameObject.SetActive(true);
        HideExcessUI();
        CompleteLevel(stageNumber);
        audioSource.Stop();
        audioSource.PlayOneShot(levelCompletedSound);
    }
    public void UpdateAllLevelButtons()
    {
        allLevelButtonComponents = FindObjectsByType<LevelButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        //Debug.Log($"Found {allLevelButtonComponents.Length} level buttons to update");

        foreach (var btn in allLevelButtonComponents)
        {
            if (btn != null)
            {
                btn.UpdateAppearance();
            }
        }
    }
    public void CompleteLevel(int level)
    {
        int unlocked = PlayerPrefs.GetInt("unlocked_level", 1);
        if (level >= unlocked)
        {
            PlayerPrefs.SetInt("unlocked_level", level + 1);
            PlayerPrefs.Save();
        }
        if (stageIndex != 29)
        {
            PlayerPrefs.SetInt("last_selected_level", level + 1);
            PlayerPrefs.Save();

            SelectedLevel = level + 1;
        }
        //Debug.Log($"unlocked {PlayerPrefs.GetInt("unlocked_level")} selected {PlayerPrefs.GetInt("last_selected_level")}");
    }
    public void ToNextLevel()
    {
        isStarted = false;
        isPaused = true;

        levelCompletedPanel.gameObject.SetActive(false);

        startPanel.gameObject.SetActive(true);

        if (stageNumber != 29)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void HideExcessUI()
    {
        pauseButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        lifesImg.gameObject.SetActive(false);
        gameTimerText.gameObject.SetActive(false);
    }
    void ShowExcessUI()
    {
        pauseButton.gameObject.SetActive(true);
        gameTimerText.gameObject.SetActive(true);
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
    public void OpenInfoPanel()
    {
        infoPanel.gameObject.SetActive(true);
    }
    public void OpenLevelsPanel()
    {
        startPanel.gameObject.SetActive(false);
        levelsPanel.gameObject.SetActive(true);
        UpdateAllLevelButtons();
    }
    public void CloseLevelsPanel()
    {
        levelsPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }
    public void MarkAsLaunched()
    {
        PlayerPrefs.SetInt("game_launched_before", 1);
        PlayerPrefs.Save();
    }
    public bool IsFirstLaunch()
    {
        // ¬озвращает true если игра запущена впервые
        return !PlayerPrefs.HasKey("game_launched_before");
    }
    void GameCompleted()
    {
        isStarted = false;
        timerStarted = false;

        gameCompletedPanel.gameObject.SetActive(true);
        HideExcessUI();
        audioSource.Stop();
        audioSource.PlayOneShot(gameCompletedSound);
    }
    public void ButtonClickSound()
    {
        UIAudioSource.PlayOneShot(clickSound);
    }
}
