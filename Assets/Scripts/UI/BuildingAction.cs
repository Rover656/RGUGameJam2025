using Game.Components;
using Game.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BuildingAction : MonoBehaviour
    {
        public Buildable Buildable;
        public Image PreviewImage;
        public TextMeshProUGUI DescriptionLabel;
        public TextMeshProUGUI CostLabel;
        
        private BuildingManager _buildingManager;

        private void Awake()
        {
            _buildingManager = FindAnyObjectByType<BuildingManager>();
            PreviewImage.sprite = Buildable.Preview;
            DescriptionLabel.text = Buildable.Description;
            CostLabel.text = $"${Buildable.Cost}";
        }

        public void Build()
        {
            _buildingManager.StartBuilding(Buildable);
        }
    }
}