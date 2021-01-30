using System;
using System.Collections.Generic;
using Resources;
using UnityEngine;
using Zenject;

namespace Generator
{
    public class ResourceBoxGenerator
    {
        [Inject]
        ResourceBox.Factory resourceBoxFactory;

        [Inject]
        GameManager gameManager;
        public event Action<ResourceBox> OnBoxSpawned;
        public event Action<ResourceBox> OnBoxDespawned;

        public void SpawnBoxes(IEnumerable<Transform> parents)
        {
            foreach (var parent in parents) {
                SpawnBox(parent);
            }
        }

        void OnBoxDrilled(ResourceBox obj)
        {
            obj.OnBoxDrilled -= OnBoxDrilled;
            gameManager.ResourceBoxCollected(obj);
            resourceBoxFactory.Despawn(obj);
            OnBoxDespawned?.Invoke(obj);
        }

        public void SpawnBox(Transform transform)
        {
            var box = resourceBoxFactory.Spawn(transform);
            box.OnBoxDrilled += OnBoxDrilled;
            OnBoxSpawned?.Invoke(box);
        }
    }
}
