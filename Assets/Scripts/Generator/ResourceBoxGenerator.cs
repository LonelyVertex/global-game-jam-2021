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
        }

        public void SpawnBox(Transform transform)
        {
            resourceBoxFactory.Spawn(transform).OnBoxDrilled += OnBoxDrilled;
        }
    }
}
