using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Episode m_Episode;

        [SerializeField] private GameObject m_UIScore;
        [SerializeField] private Image[] m_RewardImages;

        public bool isComplete { get { return gameObject.activeSelf /*&& m_UIScore.activeSelf*/; } }

        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_Episode);
        }

        public int Initialise()
        {
            int score = MapCompletion.Instance.GetEpisodeScore(m_Episode);

            // m_UIScore.SetActive(score > 0);

            for (int i = 0; i < score; i++)
            {
                m_RewardImages[i].color = new Color(1, 1, 1, 1);
            }

            return score;
        }
    }
}
