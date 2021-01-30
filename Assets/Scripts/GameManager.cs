using System;
using GamePlay;
using Generator;
using Player.Energy;
using UnityEngine;
using Utils.DI;
using Zenject;

public class GameManager
{
    [Inject] GameConfiguration gameConfiguration;
    [Inject] LevelGenerator levelGenerator;
    [Inject] EnergyHandler energyHandler;

    [Inject(Id = Identifiers.PlayerTransform)]
    Transform playerTransform;

    public event Action OnGameStart;
    public event Action OnGameOver;

    bool gameStarted;

    public void StartGame()
    {
        if (gameConfiguration.Levels.Count == 0) return;
        
        levelGenerator.Generate(gameConfiguration.Levels[0]);
        playerTransform.position = levelGenerator.SpawnPoint;
        energyHandler.RefillEnergy();
        energyHandler.OnEnergyDepleeted += GameOver;
        gameStarted = true;
        
        OnGameStart?.Invoke();
    }

    void GameOver()
    {
        energyHandler.OnEnergyDepleeted -= GameOver;
        OnGameOver?.Invoke();
    }
}