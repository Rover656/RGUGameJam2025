using Game.Resources;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class BuildingManager : MonoBehaviour
    {
        public GameObject GhostPrefab;
        public Grid Grid;

        public int Funds;
        
        private Buildable _buildable;
        private GameObject _ghostPreview;
        private SpriteRenderer _ghostPreviewRenderer;
        private Mode _mode;
        private int _buildableConfigurationIndex;

        private SpriteRenderer _targettedForDestruction;
        
        private WorldGrid _worldGrid;

        private InputAction _mousePosition;
        private InputAction _rotateAction;
        private InputAction _placeAction;
        private InputAction _cancelAction;

        private void Awake()
        {
            _ghostPreview = Instantiate(GhostPrefab);
            _ghostPreview.SetActive(_mode == Mode.Building);
            _ghostPreviewRenderer = _ghostPreview.GetComponent<SpriteRenderer>();
            _mousePosition = InputSystem.actions.FindAction("Mouse");
            _rotateAction = InputSystem.actions.FindAction("Rotate");
            _placeAction = InputSystem.actions.FindAction("Place");
            _cancelAction = InputSystem.actions.FindAction("Cancel");
            
            _worldGrid = FindAnyObjectByType<WorldGrid>();
        }

        private void Update()
        {
            if (_cancelAction.WasPressedThisFrame())
            {
                Stop();
                return;
            }
            
            if (_mode == Mode.Building)
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

                bool isOccupied = _worldGrid.IsOccupied(_ghostPreview.transform.position);
                foreach (var gridObject in _buildable.Prefab.GetComponentsInChildren<GridMonoBehaviour>())
                {
                    isOccupied |= _worldGrid.IsOccupied(_ghostPreview.transform.position + _ghostPreview.transform.rotation * Vector3.Scale(gridObject.transform.position, _ghostPreview.transform.localScale));
                }

                if (isOccupied || Funds < _buildable.Cost)
                {
                    _ghostPreviewRenderer.color = Color.red;
                }
                else
                {
                    _ghostPreviewRenderer.color = Color.white;

                    if (_placeAction.IsInProgress() && !EventSystem.current.IsPointerOverGameObject())
                    {
                        var result = Instantiate(_buildable.Prefab, _ghostPreview.transform.position, _ghostPreview.transform.rotation);
                        result.transform.localScale = _ghostPreview.transform.localScale;
                        Funds -= _buildable.Cost;
                    }
                }
            }
            else if (_mode == Mode.Destroying)
            {
                var pos = Camera.main.ScreenToWorldPoint(_mousePosition.ReadValue<Vector2>());
                pos.x += 0.5f;
                pos.y += 0.5f;
                pos.z = 0;
                var gridPos = Grid.WorldToCell(pos);

                SpriteRenderer targetSpriteRenderer = null;
                var target = _worldGrid.GetAt(gridPos);
                if (target?.GetComponent<GridMonoBehaviour>()?.CanBeDestroyed ?? false)
                {
                    targetSpriteRenderer = target?.GetComponentInParent<SpriteRenderer>();
                    if (targetSpriteRenderer == null)
                    {
                        targetSpriteRenderer = target?.GetComponent<SpriteRenderer>();
                    }
                }
                
                if (targetSpriteRenderer != _targettedForDestruction)
                {
                    if (_targettedForDestruction != null)
                    {
                        // Reset colour before changing target
                        _targettedForDestruction.color = Color.white;
                    }
                    
                    _targettedForDestruction = targetSpriteRenderer;
                    targetSpriteRenderer.color = Color.red;
                }
                
                if (_targettedForDestruction != null && _placeAction.IsPressed() && !EventSystem.current.IsPointerOverGameObject())
                {
                    Destroy(_targettedForDestruction.gameObject);
                }
            }
        }

        public void StartBuilding(Buildable buildable)
        {
            // TODO: Defence against empty rotation config set.
            _buildable = buildable;
            _mode = Mode.Building;
            _buildableConfigurationIndex = 0;
            _ghostPreview.SetActive(true);
            var configuration = _buildable.Configurations[_buildableConfigurationIndex];
            _ghostPreview.transform.rotation = Quaternion.Euler(configuration.Rotation);
            _ghostPreview.transform.localScale = configuration.Scale;
            _ghostPreviewRenderer.sprite = buildable.Preview;
        }

        public void StartDestroying()
        {
            _mode = Mode.Destroying;
            _ghostPreview.SetActive(false);
        }

        public void Stop()
        {
            _mode = Mode.None;
            _ghostPreview.SetActive(false);
            
            if (_targettedForDestruction != null)
            {
                // Reset colour before changing target
                _targettedForDestruction.color = Color.white;
                _targettedForDestruction = null;
            }
        }

        private enum Mode
        {
            None,
            Building,
            Destroying
        }
    }
}