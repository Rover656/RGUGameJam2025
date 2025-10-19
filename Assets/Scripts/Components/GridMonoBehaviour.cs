using System;
using UnityEngine;

namespace Game.Components
{
    public class GridMonoBehaviour : MonoBehaviour
    {
        public bool CanBeDestroyed = true;
        protected WorldGrid WorldGrid;
        
        private void OnEnable()
        {
            WorldGrid = FindFirstObjectByType<WorldGrid>();
            if (WorldGrid is null)
            {
                throw new NullReferenceException("worldGrid is null");
            }
            
            WorldGrid.Register(gameObject);
        }
        
        private void OnDisable()
        {
            WorldGrid.DeRegister(gameObject);
        }

        public Direction GetAbsoluteDirection(Direction direction)
        {
            return direction.InvertWith(transform.localScale).Rotated(transform.rotation);
        }
    }
}