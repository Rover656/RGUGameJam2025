using Game.Components;
using Game.Resources;
using UnityEngine;

namespace Game.UI
{
    public class BuildingAction : MonoBehaviour
    {
        public Buildable Buildable;
        
        private BuildingManager _buildingManager;

        private void Awake()
        {
            _buildingManager = FindAnyObjectByType<BuildingManager>();
        }

        public void Build()
        {
            _buildingManager.StartBuilding(Buildable);
        }
    }
}