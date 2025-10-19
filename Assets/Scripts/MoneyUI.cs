using Game.Components;
using TMPro;
using UnityEngine;

namespace Game
{
    public class MoneyUI : MonoBehaviour
    {
        public BuildingManager BuildingManager;
        
        public TextMeshProUGUI moneyText;
        
        void SetMoneyText()
        {
            moneyText.text = "$" + BuildingManager.Funds;
        }

        private void FixedUpdate()
        {
            SetMoneyText();
        }
    }
}
