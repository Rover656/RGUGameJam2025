using Game.Resources;

namespace Game.Components
{
    public class ItemSink : GridMonoBehaviour, IItemSink
    {
        public bool InvertCompletedValues;
        
        private BuildingManager _buildingManager;
        
        private void Awake()
        {
            _buildingManager = FindAnyObjectByType<BuildingManager>();
        }
        
        public bool AcceptItem(Direction fromSide, Item stack)
        {
            _buildingManager.Funds += InvertCompletedValues ? -stack.CompletedValue : stack.CompletedValue;
            if (_buildingManager.Funds < 0)
            {
                _buildingManager.Funds = 0;
            }
            
            return true;
        }
    }
}