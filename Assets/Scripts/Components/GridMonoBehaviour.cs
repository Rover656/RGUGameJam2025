using System;
using UnityEngine;

namespace Game.Components
{
    public abstract class GridMonoBehaviour : MonoBehaviour
    {
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
            return direction.Rotated(transform.rotation);
        }
    }
}