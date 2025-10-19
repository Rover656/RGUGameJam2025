using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

namespace Game
{
    public class TimeSpan : MonoBehaviour
    {

        public float timeValue = 180;
        public TMP_Text timeText;

        private void Update()
        {
            if (timeValue > 0)
            {
                timeValue -= Time.deltaTime;
            }
            else
            {
                timeValue = 0;
                SceneManager.LoadScene(sceneBuildIndex:1);
            }

            DisplayTime(timeValue);
        }

        void DisplayTime(float timeToDisplay)
        {
            if(timeToDisplay < 0)
            {
                timeToDisplay = 0;
            }
            
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}