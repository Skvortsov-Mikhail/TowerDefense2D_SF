using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    [RequireComponent(typeof(MapLevel))]
    public class ExtraLevel : MonoBehaviour
    {
        [SerializeField] private MapLevel m_RootLevel;

        [SerializeField] private int m_NeedPoints;

        [SerializeField] private Text m_NeedPointsText;
        [SerializeField] private Button m_Button;
        [SerializeField] private GameObject m_LockerPanel;
        [SerializeField] private GameObject m_InfoPanel;

        /*
        private bool m_IsAvailable;
        public bool IsAvailable => m_IsAvailable;
        */

        public void TryActivate()
        {
            gameObject.SetActive(m_RootLevel.isComplete);

            SpriteRenderer sr = transform.Find("VisualModel").GetComponent<SpriteRenderer>();

            if (m_NeedPoints <= MapCompletion.Instance.TotalScore)
            {
                // m_IsAvailable = true;
                m_Button.interactable = true;
                m_LockerPanel.SetActive(false);
                m_InfoPanel.SetActive(true);

                sr.color = new Color(1, 1, 1, 1);

                GetComponent<MapLevel>().Initialise();
            }

            else
            {
                m_Button.interactable = false;
                m_LockerPanel.SetActive(true);
                m_InfoPanel.SetActive(false);
                m_NeedPointsText.text = "Нужно " + m_NeedPoints.ToString();

                sr.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
}