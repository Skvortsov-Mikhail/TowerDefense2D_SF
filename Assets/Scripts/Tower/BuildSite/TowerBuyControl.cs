using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public partial class TowerBuyControl : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_TA;

        [SerializeField] private Text m_Text;

        [SerializeField] private Button m_Button;

        [SerializeField] private Transform m_BuildSite;

        public void SetBuildSite(Transform value)
        {
            m_BuildSite = value;
        }

        private void Awake()
        {
            //TDPlayer.GoldUpdateSubscribe(GoldStatusCheck);
        }

        private void Start()
        {
            TDPlayer.GoldUpdateSubscribe(GoldStatusCheck);

            m_Text.text = m_TA.m_GoldCost.ToString();
            m_Button.GetComponent<Image>().sprite = m_TA.GUISprite;
        }

        private void GoldStatusCheck(int gold)
        {
            if (gold >= m_TA.m_GoldCost != m_Button.interactable)
            {
                m_Button.interactable = !m_Button.interactable;
                m_Text.color = m_Button.interactable ? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TA, m_BuildSite);
            BuildSite.HideControls();
        }

        private void OnDestroy()
        {
            TDPlayer.GoldUpdateUnSubscribe(GoldStatusCheck);
        }
    }
}