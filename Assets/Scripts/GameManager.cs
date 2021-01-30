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

    bool gameStarted;

    // This should be in PlayerState
    public bool IsPlayerDrilling { get; private set; }

    public void StartGame()
    {
        if (gameConfiguration.Levels.Count == 0) return;

        levelGenerator.Generate(gameConfiguration.Levels[0]);
        
        PlacePlayer();
        energyHandler.RefillEnergy();
        energyHandler.OnEnergyDepleeted += GameOver;
        
        playerInputHandler.EnablePlayerInput();
        gameStarted = true;
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
    }

    public void ResourceBoxCollected(ResourceBox resourceBox)
    {
        Debug.Log("Resource box collected");
    }

    public void SetPlayerDrilling(bool isPlayerDrilling)
    {
        IsPlayerDrilling = isPlayerDrilling;
    }
}
