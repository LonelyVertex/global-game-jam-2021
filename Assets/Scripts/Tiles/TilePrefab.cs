using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class TilePrefab : MonoBehaviour
    {
        [SerializeField] List<Transform> boxPoints;
        [SerializeField] List<Transform> minePoints;

        public List<Transform> BoxPoints => boxPoints;
        public List<Transform> MinePoints => minePoints;

        void OnDrawGizmos()
        {
            DrawBoxPointsGizmos();
            DrawMinePointsGizmos();
        }

        void DrawBoxPointsGizmos()
        {
            if (boxPoints == null) return;
            
            Gizmos.color = Color.green;
            foreach (var boxPoint in boxPoints)
            {
                if (boxPoint)
                {
                    Gizmos.DrawWireCube(boxPoint.position, Vector3.one);
                }
            }
        }
        
        void DrawMinePointsGizmos()
        {
            if (minePoints == null) return;
            
            Gizmos.color = Color.red;
            foreach (var minePoint in minePoints)
            {
                if (minePoint)
                {
                    Gizmos.DrawWireSphere(minePoint.position, 1f);
                }
            }
        }
    }
}
