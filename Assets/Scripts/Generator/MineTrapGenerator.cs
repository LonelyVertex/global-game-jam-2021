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

        [Inject]
        MineTrapEffect.EffectFactory mineTrapEffectFactory;

        public void SpawnMines(IEnumerable<Transform> parents)
        {
            foreach (var parent in parents) {
                SpawnMine(parent);
            }
        }

        void OnTrapTrigger(MineTrap obj)
        {
            obj.OnTrapTrigger -= OnTrapTrigger;
            
            mineTrapEffectFactory.Spawn(obj.transform.position);
            mineTrapFactory.Despawn(obj);
        }

        public void SpawnMine(Transform transform)
        {
            mineTrapFactory.Spawn(transform).OnTrapTrigger += OnTrapTrigger;
        }
    }
}
