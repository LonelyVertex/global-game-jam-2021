using System;
using GamePlay;
using Generator;
using Player.Controls;
using Player.Energy;
using Resources;
using UnityEngine;
using Utils.DI;
using Zenject;

public class GameManager
{
    [Inject] GameConfiguration gameConfiguration;
    [Inject] LevelGenerator levelGenerator;
    [Inject] EnergyHandler energyHandler;
    [Inject] PlayerInputHandler playerInputHandler;

    [Inject(Id = Identifiers.PlayerCharacterController)]
    CharacterController playerCharacterController;

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGameComplete;
    public event Action OnBoxCollected;

    bool gameStarted;

    public int BoxesCollected { get; private set; }
    public int CurrentLevelBoxes { get; private set; }

    int currentLevel = 0;
    
    public void StartGame()
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
        playerInputHandler.DisablePlayerInput();
        
        // TODO transition
        StartGame();
    }

    void GameComplete()
    {
        Debug.Log("Game Complete");
    }

    public void ResourceBoxCollected(ResourceBox resourceBox)
    {
        BoxesCollected++;
        OnBoxCollected?.Invoke();
    }
}
