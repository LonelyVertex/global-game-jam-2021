using System;
using GamePlay;
using Generator;
using Player.Controls;
using Player.Energy;
using Resources;
using UnityEngine;
using Zenject;

public class GameManager : ITickable
{
    [Inject] GameConfiguration gameConfiguration;
    [Inject] LevelGenerator levelGenerator;
    [Inject] EnergyHandler energyHandler;
    [Inject] PlayerInputHandler playerInputHandler;

    [Inject]
    CharacterController playerCharacterController;

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnLevelComplete;
    public event Action OnGameComplete;
    public event Action OnBoxCollected;

    bool gameStarted;

    public int BoxesCollected { get; private set; }
    public int CurrentLevelBoxes { get; private set; }

    int currentLevel = 0;

    bool shouldStartNextLevel = false;
    float timeToNextLevel = 0;
    float startTime;
    float endTime;

    public float CurrentTime => endTime - startTime;
    
    public void StartGame()
    {
        currentLevel = 0;
        startTime = Time.time;
        StartLevel();
    }

    void StartLevel()
    {
        if (currentLevel >= gameConfiguration.Levels.Count)
        {
            GameComplete();
            return;
        }

        levelGenerator.Generate(gameConfiguration.Levels[currentLevel]);
        
        PlacePlayer();
        energyHandler.RefillEnergy();
        energyHandler.OnEnergyDepleeted += GameOver;
        
        playerInputHandler.EnablePlayerInput();
        gameStarted = true;
        BoxesCollected = 0;
        CurrentLevelBoxes = levelGenerator.SpawnedBoxes;
        OnGameStart?.Invoke();
    }

    void PlacePlayer()
    {
        playerCharacterController.enabled = false;
        playerCharacterController.transform.position = levelGenerator.SpawnPoint;
        playerCharacterController.enabled = true;
    }

    void GameOver()
    {
        energyHandler.OnEnergyDepleeted -= GameOver;
        playerInputHandler.DisablePlayerInput();
        OnGameOver?.Invoke();
    }

    public void LevelComplete()
    {
        currentLevel++;
        OnLevelComplete?.Invoke();
        
        shouldStartNextLevel = true;
        timeToNextLevel = 2;
    }

    void GenerateNextLevel()
    {
        shouldStartNextLevel = false;
        playerInputHandler.DisablePlayerInput();
        StartLevel();
    }

    void GameComplete()
    {
        endTime = Time.time;
        playerInputHandler.DisablePlayerInput();
        SaveBestTime();
        OnGameComplete?.Invoke();
    }

    void SaveBestTime()
    {
        var previousBest = PlayerPrefs.GetFloat("bestTime", Mathf.Infinity);
        PlayerPrefs.SetFloat("bestTime", Mathf.Min(CurrentTime, previousBest));
    }

    public void ResourceBoxCollected(ResourceBox resourceBox)
    {
        BoxesCollected++;
        OnBoxCollected?.Invoke();
    }

    public void Tick()
    {
        if (shouldStartNextLevel)
        {
            timeToNextLevel -= Time.deltaTime;
            if (timeToNextLevel <= 0)
            {
                GenerateNextLevel();
            }
        }
    }
}
