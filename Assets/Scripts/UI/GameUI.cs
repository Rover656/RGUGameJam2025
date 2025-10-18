using System.Collections.Generic;
using Game.Components;
using Game.Resources;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class GameUI : MonoBehaviour
    {
        private BuildingManager _buildingManager;

        public List<Buildable> Buildables;
        
        private void Start()
        {
            _buildingManager = FindAnyObjectByType<BuildingManager>();
        }
        
        private void OnEnable()
        {
            // The UXML is already instantiated by the UIDocument component
            var uiDocument = GetComponent<UIDocument>();
            
            var list = uiDocument.rootVisualElement.Q("ListView") as ListView;
            
            //uiDocument.rootVisualElement.dataSource = this;
            //uiDocument.rootVisualElement.Bind(new SerializedObject(this));
            list.itemsSource = Buildables;
        }
    }
}