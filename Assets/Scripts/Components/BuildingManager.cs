using System;
using Game.Resources;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class BuildingManager : MonoBehaviour
    {
        public GameObject GhostPrefab;
        public Grid Grid;
        
        private Buildable _buildable;
        private GameObject _ghostPreview;
        private SpriteRenderer _ghostPreviewRenderer;
        private bool _isBuilding;
        private int _buildableConfigurationIndex;
        
        private WorldGrid _worldGrid;

        private InputAction _mousePosition;
        private InputAction _rotateAction;
        private InputAction _placeAction;

        private void Awake()
        {
            _ghostPreview = Instantiate(GhostPrefab);
            _ghostPreview.SetActive(_isBuilding);
            _ghostPreviewRenderer = _ghostPreview.GetComponent<SpriteRenderer>();
            _mousePosition = InputSystem.actions.FindAction("Mouse");
            _rotateAction = InputSystem.actions.FindAction("Rotate");
            _placeAction = InputSystem.actions.FindAction("Place");
            
            _worldGrid = FindAnyObjectByType<WorldGrid>();
        }

        private void Update()
        {
            if (_isBuilding)
            {
                if (_rotateAction.WasPressedThisFrame())
                {
                    _buildableConfigurationIndex = _buildableConfigurationIndex + 1 >= _buildable.Configurations.Count ? 0 : _buildableConfigurationIndex + 1;
                    var configuration = _buildable.Configurations[_buildableConfigurationIndex];
                    _ghostPreview.transform.rotation = Quaternion.Euler(configuration.Rotation);
                    _ghostPreview.transform.localScale = configuration.Scale;
                }
                
                var pos = Camera.main.ScreenToWorldPoint(_mousePosition.ReadValue<Vector2>());
                pos.x += 0.5f;
                pos.y += 0.5f;
                pos.z = 0;
                _ghostPreview.transform.position = Grid.WorldToCell(pos);

                if (_worldGrid.IsOccupied(_ghostPreview.transform.position))
                {
                    _ghostPreviewRenderer.color = Color.red;
                }
                else
                {
                    _ghostPreviewRenderer.color = Color.white;

                    if (_placeAction.WasPressedThisFrame())
                    {
                        var result = Instantiate(_buildable.Prefab, _ghostPreview.transform.position, _ghostPreview.transform.rotation);
                        result.transform.localScale = _ghostPreview.transform.localScale;
                        
                        StopBuilding();
                    }
                }
            }
        }

        public void StartBuilding(Buildable buildable)
        {
            // TODO: Defence against empty rotation config set.
            _buildable = buildable;
            _isBuilding = true;
            _buildableConfigurationIndex = 0;
            _ghostPreview.SetActive(true);
            var configuration = _buildable.Configurations[_buildableConfigurationIndex];
            _ghostPreview.transform.rotation = Quaternion.Euler(configuration.Rotation);
            _ghostPreview.transform.localScale = configuration.Scale;
            _ghostPreviewRenderer.sprite = buildable.Preview;
        }

        public void StopBuilding()
        {
            _isBuilding = false;
            _ghostPreview.SetActive(false);
        }
    }
}