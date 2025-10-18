using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Grid))]
    public class WorldGrid : MonoBehaviour
    {
        private Grid _grid;
        private readonly Dictionary<Vector3Int, GameObject> _gridContents = new();
        
        private void Awake()
        {
            _grid = GetComponent<Grid>();
        }

        [CanBeNull]
        public GameObject GetNeighbor(GameObject self, Direction direction)
        {
            var cellPos = _grid.WorldToCell(self.transform.position);
            cellPos.x += direction.GetXOffset();
            cellPos.y += direction.GetYOffset();
            return _gridContents.GetValueOrDefault(cellPos);
        }

        public void Register(GameObject gameObject)
        {
            var cellPos = _grid.WorldToCell(gameObject.transform.position);
            if (!_gridContents.TryAdd(cellPos, gameObject))
            {
                throw new Exception("Grid space taken!");
            }
        }

        public void DeRegister(GameObject gameObject)
        {
            var cellPos = _grid.WorldToCell(gameObject.transform.position);
            _gridContents.Remove(cellPos);
        }
    }
}