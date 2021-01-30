using System.Collections.Generic;
using Traps;
using UnityEngine;
using Zenject;

namespace Generator
{
    public class MineTrapGenerator
    {
        [Inject]
        MineTrap.Factory mineTrapFactory;

        public void SpawnMines(List<Transform> positions)
        {
            foreach (var position in positions) {
                mineTrapFactory.Spawn(position.position).OnTrapTrigger += OnTrapTrigger;
            }
        }

        void OnTrapTrigger(MineTrap obj)
        {
            obj.OnTrapTrigger -= OnTrapTrigger;
            mineTrapFactory.Despawn(obj);
        }

        public void SpawnMine(Transform transform)
        {
            mineTrapFactory.Spawn(transform.position).OnTrapTrigger += OnTrapTrigger;
        }
    }
}
