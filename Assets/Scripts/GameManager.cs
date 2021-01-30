using System;
using Generator;
using Player.Energy;
using UnityEngine;
using Utils.DI;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] int gridSize;
    [SerializeField] int minTiles;
    [SerializeField] int maxTiles;
    [SerializeField] int boxCount;
    [SerializeField] int mineCount;

    [Inject] LevelGenerator levelGenerator;
    [Inject] EnergyHandler energyHandler;
    [Inject(Id=Identifiers.PlayerTransform)] Transform playerTransform;

    public event Action OnGameOver;

    bool gameStarted = false;
    
    public void StartGame()
    {
        levelGenerator.Generate(gridSize, minTiles, maxTiles, boxCount, mineCount);
        playerTransform.position = levelGenerator.SpawnPoint;
        energyHandler.RefillEnergy();
        gameStarted = true;
    }

    void Update()
    {
        if (gameStarted && energyHandler.CurrentEnergy <= 0)
        {
            OnGameOver?.Invoke();
        }
    }
}
