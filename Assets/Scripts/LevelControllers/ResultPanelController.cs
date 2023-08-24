using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class ResultPanelController : SingletoneBase<ResultPanelController>
    {
        [SerializeField] private GameObject m_PanelSuccess;
        [SerializeField] private Sound m_WinSound;

        [SerializeField] private GameObject m_PanelFailure;
        [SerializeField] private Sound m_LoseSound;

        /*
        [SerializeField] private Text m_Kills;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_Time;
        [SerializeField] private Text m_ExtraBonus;

        [SerializeField] private Text m_Result;
        [SerializeField] private Text m_ButtonNextText;
        
        private bool m_Success;
        */

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(bool result)
        {
            m_PanelSuccess?.SetActive(result);
            m_PanelFailure?.SetActive(!result);

            if(result)
            {
                m_WinSound.Play();
            }

            else
            {
                m_LoseSound.Play();
            }
        }

        public void OnPlayNext()
        {
            LevelSequenceController.Instance.AdvanceLevel();
        }

        public void OnRestartLevel()
        {
            LevelSequenceController.Instance.RestartLevel();
        }

        /*
        public void ShowResults(PlayerStatistics levelResults, bool success)
        {
            gameObject.SetActive(true);

            m_Success = success;

            m_Result.text = success ? "Win" : "Lose";

            m_Kills.text = "Kills: " + levelResults.numKills.ToString();
            m_Score.text = "Score: " + levelResults.score.ToString();
            m_Time.text = "Time: " + levelResults.time.ToString();
            m_ExtraBonus.text = "Extra time bonus: " + levelResults.extraBonus.ToString();

            m_ButtonNextText.text = success ? "Next" : "Restart";

            Time.timeScale = 0;
        }
        */
    }
}
