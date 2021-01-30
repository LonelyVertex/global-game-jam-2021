using System;
using System.Collections.Generic;
using Resources;
using UnityEngine;

namespace Generator
{
    public class ResourceBoxGenerator
    {
        ResourceBox.Factory resourceBoxFactory;
        GameManager gameManager;

        public HashSet<ResourceBox> ResourceBoxes { get; private set; }
        public event Action<ResourceBox> OnBoxCollected;

        public ResourceBoxGenerator(ResourceBox.Factory resourceBoxFactory, GameManager gameManager)
        {
            this.resourceBoxFactory = resourceBoxFactory;
            this.gameManager = gameManager;
            ClearState();

            this.gameManager.OnGameOver += ClearState;
            this.gameManager.OnLevelComplete += ClearState;
            this.gameManager.OnGameComplete += ClearState;
        }

        void ClearState()
        {
            if (ResourceBoxes == null) {
                ResourceBoxes = new HashSet<ResourceBox>();
            } else {
                ResourceBoxes.Clear();
            }
        }

        public void SpawnBoxes(IEnumerable<Transform> parents)
        {
            foreach (var parent in parents) {
                SpawnBox(parent);
            }
        }

        void OnBoxDrilled(ResourceBox box)
        {
            box.OnBoxDrilled -= OnBoxDrilled;
            gameManager.ResourceBoxCollected(box);
            resourceBoxFactory.Despawn(box);
            ResourceBoxes.Remove(box);
            OnBoxCollected?.Invoke(box);
        }

        public void SpawnBox(Transform transform)
        {
            var box = resourceBoxFactory.Spawn(transform);
            box.OnBoxDrilled += OnBoxDrilled;
            ResourceBoxes.Add(box);
        }
    }
}
