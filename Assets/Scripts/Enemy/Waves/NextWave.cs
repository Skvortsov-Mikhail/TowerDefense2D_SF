using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class NextWave : MonoBehaviour
    {
        [SerializeField] private Button m_NextWaveButton;
        [SerializeField] private Text m_Title;
        [SerializeField] private Text m_BonusText;
        [SerializeField] private Image m_MoneyIcon;

        private Color titleAfterAllWavesColor = new Color(0.3f, 1f, 1f, 0.7f);

        private EnemyWaveManager m_EnemyWaveManager;

        private float m_TimeToNextWave;

        private void Start()
        {
            m_EnemyWaveManager = FindObjectOfType<EnemyWaveManager>();
            EnemyWave.OnWavePrepare += (float time) =>
            {
                m_TimeToNextWave = time;
            };
        }

        public void CallWave()
        {
            m_EnemyWaveManager.ForceNextWave();
        }

        private void Update()
        {
            if (m_EnemyWaveManager.IsLastWaveGone == false)
            {
                int reward = (int)(m_TimeToNextWave * m_EnemyWaveManager.BonusForEverySecond);

                if (reward < 0)
                {
                    reward = 0;
                }

                m_BonusText.text = "Бонус: " + reward.ToString();

                m_TimeToNextWave -= Time.deltaTime;
            }

            else
            {
                m_NextWaveButton.interactable = false;
                m_Title.color = titleAfterAllWavesColor;
                m_Title.text = "Волны закончились";
                m_BonusText.text = "";
                m_MoneyIcon.enabled = false;
            }
        }
    }
}