using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Transform[] spawnPoints;
    public Transform carTransform;

    public GameObject spawnPointsContainer;
    public GameObject cargoPrefab;
    private GameObject currentCargo;
    public TextMeshProUGUI scoreText;

    private float distanceToCar = 2f;
    private float spawnInterval = 5f;
    private int score = 0;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 0;
    }
    void Start()
    {
        spawnPoints = spawnPointsContainer.GetComponentsInChildren<Transform>();
        InvokeRepeating("SpawnCargo", 0f, spawnInterval);
        AddScore(0);
    }
    void Update()
    {
 
    }
    void SpawnCargo ()
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
}
