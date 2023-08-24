using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button m_ContinueButton;
        [SerializeField] private GameObject m_GeneralMenu;
        [SerializeField] private GameObject m_ConfirmWindow;

        private void Start()
        {
            if(m_GeneralMenu != null)
            {
                m_GeneralMenu.SetActive(true);
            }

            if (m_ConfirmWindow != null)
            {
                m_ConfirmWindow.SetActive(false);
            }

            if (m_ContinueButton != null)
            {
                m_ContinueButton.interactable = FileHandler.HasFile(MapCompletion.m_EpisodesFilename);
            }
        }

        public void NewGame()
        {
            if(FileHandler.HasFile(MapCompletion.m_EpisodesFilename) == true)
            {
                m_GeneralMenu.SetActive(false);
                m_ConfirmWindow.SetActive(true);
            }
            
            else
            {
                Confirmed();
            }
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void Continue()
        {
            SceneManager.LoadScene(1);
        }   

        public void Quit()
        {
            Application.Quit();
        }

        public void Confirmed()
        {
            FileHandler.Reset(MapCompletion.m_EpisodesFilename);
            FileHandler.Reset(Upgrades.m_UpgradesFilename);

            SceneManager.LoadScene(1);
        }

        public void BackToMenu()
        {
            m_GeneralMenu.SetActive(true);
            m_ConfirmWindow.SetActive(false);
        }
    }
}