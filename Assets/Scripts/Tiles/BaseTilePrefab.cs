using System;
using UnityEngine;
using Zenject;

namespace Tiles
{
    public class BaseTilePrefab : TilePrefab
    {
        [SerializeField] GameObject door;

        [Inject] GameManager gameManager;

        bool AllCollected => gameManager.BoxesCollected == gameManager.CurrentLevelBoxes;
        
        void Start()
        {
            door.SetActive(false);
            gameManager.OnBoxCollected += OnBoxCollected;
        }

        void OnDestroy()
        {
            gameManager.OnBoxCollected -= OnBoxCollected;
        }

        void OnBoxCollected()
        {
            if (AllCollected)
            {
                door.SetActive(false);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (AllCollected)
            {
                gameManager.LevelComplete();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!AllCollected)
            {
                door.SetActive(true);
            }
        }
    }
}