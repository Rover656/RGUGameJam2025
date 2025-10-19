using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components;
using Game.Resources;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Money : MonoBehaviour
    {
        public int money;
        public TextMeshProUGUI moneyText;
        void SetMoneyText()
        {
            moneyText.text = "$" + money.ToString();
        }
        private void Start()
        {
            money = 2000;

            SetMoneyText();
        }

        

    }
}
