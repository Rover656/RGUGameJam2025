using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class CameraDragging : MonoBehaviour
    {
        public float DragSpeed = 6;
        private Vector3 _dragOrigin;
        private InputAction _moveAxis;
        private InputAction _zoom;

        void Start()
        {
            _moveAxis = InputSystem.actions.FindAction("Camera Movement");
            _zoom = InputSystem.actions.FindAction("Camera Zoom");
        }
        
        void Update()
        {
            var zoom = _zoom.ReadValue<float>();
            Camera.main.orthographicSize = Mathf.Min(10, Mathf.Max(3, Camera.main.orthographicSize + -zoom * 6 * Time.deltaTime));
            
            var movement = _moveAxis.ReadValue<Vector2>();
            if (movement.sqrMagnitude == 0)
            {
                return;
            }
            
            Vector3 move = new Vector3(movement.x * DragSpeed * Time.deltaTime, movement.y * DragSpeed * Time.deltaTime, 0);
            transform.Translate(move, Space.World);
        }
    }
}